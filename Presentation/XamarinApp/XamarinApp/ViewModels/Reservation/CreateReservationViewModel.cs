using System;
using System.Collections.Generic;
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
    public class CreateReservationViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        #endregion

        private readonly Guid _resourceId;
        private readonly ObservableCollection<ScheduleReservation> _reservations;
        public DateTime FromDate { get; }
        public TimeSpan fromTime;
        public TimeSpan toTime;
        
        public CreateReservationViewModel(INavigationService navigator, string navigationPath, DateTime fromDate, Guid resourceId, ObservableCollection<ScheduleReservation> reservations)
        {
            #region Navigation
            NavigationPath = navigationPath;
            _navigator = navigator;
            #endregion
            
            FromDate = fromDate;
            _resourceId = resourceId;
            _reservations = reservations;
            fromTime = TimeSpan.Parse(FromDate
                .AddMinutes(-1)
                .ToString("HH:mm"));
            toTime = TimeSpan.Parse(FromDate
                .AddMinutes(14)
                .ToString("HH:mm"));
        }
        
        public ICommand CreateReservation=> new Command(async () =>
        {
            var mobileBffUrl = Xamarin.Forms.Application.Current.Properties["MobileBffUrl"] as string;
            var token = await SecureStorage.GetAsync("jwt_token");
            var user = JsonSerializer.Deserialize<Domain.Entities.User>(await SecureStorage.GetAsync("current_user"));
            
            var fromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, fromTime.Hours, fromTime.Minutes, 0);
            var toDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, toTime.Hours, toTime.Minutes, 0);

            var reservation = new Domain.Entities.Reservation
            {
                UserId = user.Id,
                ResourceId = _resourceId,
                From = fromDate,
                To = toDate
            };

            if (!ValidateReservation(_reservations, reservation))
            {
                await _navigator.DisplayAlert("Error!", "You cannot create overlapping reservation", "Ok");
                return;
            };
            
            try
            {
                await mobileBffUrl.AppendPathSegment("Reservation").WithOAuthBearerToken(token).PostJsonAsync(reservation);
                await _navigator.PopModal();
            }
            catch (Exception e)
            {
                await _navigator.DisplayAlert("Error!", e.Message, "Ok");
            }
        });

        private bool ValidateReservation(ObservableCollection<ScheduleReservation> reservations, Domain.Entities.Reservation reservationToValidate)
        {
            if (!reservations.Any()) return true;

            if (reservationToValidate.From == reservationToValidate.To) return false;

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
    }
}