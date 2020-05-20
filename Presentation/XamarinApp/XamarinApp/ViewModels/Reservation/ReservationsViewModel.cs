using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.Domain.Entities;
using XamarinApp.ViewModels.Resource;
using Color = System.Drawing.Color;

namespace XamarinApp.ViewModels.Reservation
{
    public class ReservationsViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        #endregion

        public double MinFromTime = 0;
        public double MaxToTime = 24;
        public readonly ObservableCollection<object> ScheduleResources;
        public readonly ObservableCollection<ScheduleReservation> ScheduleReservations;
        private List<Domain.Entities.Reservation> _reservations;
        private readonly List<Domain.Entities.Resource> _resources;
        
        public ReservationsViewModel(INavigationService navigator, string navigationPath)
        {
            #region Navigation
            NavigationPath = navigationPath;
            _navigator = navigator;
            #endregion
            
            ScheduleResources = new ObservableCollection<object>();
            ScheduleReservations = new ObservableCollection<ScheduleReservation>();
            _reservations = new List<Domain.Entities.Reservation>();
            _resources = new List<Domain.Entities.Resource>();
        }

        public override async Task BeforeFirstShown()
        {
            await _navigator.NavigateToModal(new BusyViewModel(_navigator, $"{NavigationPath}/Busy"));
            await GetReservations();
            await GetResources();
            GenerateScheduleReservations();
            GenerateScheduleResources();
            await _navigator.PopModal();
        }
        
        private async Task GetReservations()
        {
            var mobileBffUrl = Xamarin.Forms.Application.Current.Properties["MobileBffUrl"] as string;
            var token = await SecureStorage.GetAsync("jwt_token");
            try
            {
                var result = await mobileBffUrl.AppendPathSegment($"Reservation/UserId/{new UserViewModel().User.Id}").WithOAuthBearerToken(token).GetJsonAsync<List<Domain.Entities.Reservation>>();
                if (result.Any())
                {
                    _reservations = result.ToList();
                }
            }
            catch (Exception e)
            {
                await _navigator.DisplayAlert("Error!", e.Message, "Ok");
            }
        }
        
        private async Task GetResources()
        {
            var mobileBffUrl = Xamarin.Forms.Application.Current.Properties["MobileBffUrl"] as string;
            var token = await SecureStorage.GetAsync("jwt_token");
            try
            {
                var result = await mobileBffUrl.AppendPathSegment("Resource").WithOAuthBearerToken(token).GetJsonAsync<List<Domain.Entities.Resource>>();
                if (result.Any())
                {
                    foreach (var resource in result.ToList())
                    {
                        _resources.Add(resource);
                    }
                }
            }
            catch (Exception e)
            {
                await _navigator.DisplayAlert("Error!", e.Message, "Ok");
            }
        }

        private void GenerateScheduleReservations()
        {
            foreach (var reservation in _reservations)
            {
                ScheduleReservations.Add(new ScheduleReservation
                {
                    StartTime = reservation.From,
                    EndTime = reservation.To,
                    Subject = "Reservation",
                    Color = Color.RoyalBlue,
                    ReservationId = reservation.Id,
                    ResourceIds = new ObservableCollection<object>
                    {
                        reservation.ResourceId
                    }
                });
            }
        }

        private void GenerateScheduleResources()
        {
            var rand = new Random();
            var max = byte.MaxValue + 1;
            foreach (var reservation in _reservations)
            {
                if (ScheduleResources.FirstOrDefault(r => (Guid)((ScheduleResource)r).Id == reservation.ResourceId) != null) continue;
                ScheduleResources.Add(new ScheduleResource
                {
                    Name = _resources.FirstOrDefault(r => r.Id == reservation.ResourceId)?.Name,
                    Id = reservation.ResourceId,
                    Color = Color.FromArgb(rand.Next(max), rand.Next(max), rand.Next(max))
                });
            }
        }

        public void ScheduleOnCellTapped(object sender, CellTappedEventArgs e)
        {
            if (e.Appointment != null)
            {
                var viewModel = new ResourceViewModel(_navigator, $"{NavigationPath}/Resource", 
                    _resources.First(r =>  
                        r.Id == _reservations.First(x => 
                            x.Id == ((ScheduleReservation)e.Appointment).ReservationId).ResourceId));
                _navigator.NavigateToModal(new UpdateReservationViewModel(_navigator, $"{NavigationPath}/EditReservation", _reservations.FirstOrDefault(r => r.Id == ((ScheduleReservation)e.Appointment).ReservationId), viewModel.Reservations));
            }
            
        }
    }
}