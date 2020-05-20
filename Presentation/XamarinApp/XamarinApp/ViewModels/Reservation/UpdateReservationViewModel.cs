using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using Flurl;
using Flurl.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.Domain.Entities;

namespace XamarinApp.ViewModels.Reservation
{
    public class UpdateReservationViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        #endregion
        
        private readonly Domain.Entities.Reservation _reservation;
        private readonly ObservableCollection<ScheduleReservation> _reservations;
        public readonly ScheduleReservation ScheduleReservation;
        public TimeSpan fromTime;
        public TimeSpan toTime;

        public UpdateReservationViewModel(INavigationService navigator, string navigationPath, Domain.Entities.Reservation reservation, ObservableCollection<ScheduleReservation> reservations)
        {
            #region Navigation
            NavigationPath = navigationPath;
            _navigator = navigator;
            #endregion
            
            _reservation = reservation;
            fromTime = new TimeSpan(reservation.From.Hour, reservation.From.Minute, 0);
            toTime = new TimeSpan(reservation.To.Hour, reservation.To.Minute, 0);
            _reservations = reservations;
            ScheduleReservation = _reservations.FirstOrDefault(r => r.ReservationId == reservation.Id);
        }
        
        public ICommand UpdateReservation=> new Command(async () =>
        {
            var mobileBffUrl = Xamarin.Forms.Application.Current.Properties["MobileBffUrl"] as string;
            var token = await SecureStorage.GetAsync("jwt_token");
            var user = JsonSerializer.Deserialize<Domain.Entities.User>(await SecureStorage.GetAsync("current_user"));
            
            _reservation.From = new DateTime(_reservation.From.Year, _reservation.From.Month, _reservation.From.Day, fromTime.Hours, fromTime.Minutes, 0);
            _reservation.To = new DateTime(_reservation.To.Year, _reservation.To.Month, _reservation.To.Day, toTime.Hours, toTime.Minutes, 0);
            
            if (!ValidateReservation(_reservations, _reservation))
            {
                await _navigator.DisplayAlert("Error!", "You cannot create overlapping reservation", "Ok");
                return;
            };
            
            try
            {
                await mobileBffUrl.AppendPathSegment($"Reservation/{_reservation.Id}").WithOAuthBearerToken(token).PutJsonAsync(_reservation);
            }
            catch (Exception e)
            {
                await _navigator.DisplayAlert("Error!", e.Message, "Ok");
            }
        });

        private bool ValidateReservation(ObservableCollection<ScheduleReservation> reservations, Domain.Entities.Reservation reservationToValidate)
        {
            if (!reservations.Any()) return true;

            foreach (var reservation in reservations)
            {
                if (reservation.StartTime < reservationToValidate.To && reservationToValidate.From < reservation.EndTime) return false;
            }
            
            return true;
        }
        
        public void ToTimePickerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName != TimePicker.TimeProperty.PropertyName) return;
            if (sender is TimePicker timePicker) toTime = timePicker.Time;
        }
        
        public void FromTimePickerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName != TimePicker.TimeProperty.PropertyName) return;
            if (sender is TimePicker timePicker) fromTime = timePicker.Time;
        }
        
        public ICommand NavigateBack => new Command(() => _navigator.PopModal());
        public ICommand NavigateToChatView => new Command(() =>
        {
            _navigator.PopModal();
            _navigator.NavigateTo(new ChatViewModel(_navigator, $"{NavigationPath}/Chat"));
        });
    }
}