using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.Domain.Entities;
using XamarinApp.ViewModels.Reservation;
using Type = XamarinApp.Domain.Enums.Type;

namespace XamarinApp.ViewModels.Resource
{
    public class ResourceViewModel : ViewModelBase
    {
        #region Navigation

        public string NavigationPath { get; }
        private readonly INavigationService _navigator;

        #endregion

        public readonly Domain.Entities.Resource Resource;
            
        public ObservableCollection<ScheduleReservation> Reservations { get; set; }
        private HubConnection _hubConnection;

        public ResourceViewModel(INavigationService navigator, string navigationPath, Domain.Entities.Resource resource)
        {
            #region Navigation

            NavigationPath = navigationPath;
            _navigator = navigator;

            #endregion

            Resource = resource;
            Reservations = new ObservableCollection<ScheduleReservation>();
            GenerateNonAccessibleBlocks(Reservations);
            foreach (var reservation in Resource.Reservations)
            {
                Reservations.Add(CreateAppointment(reservation));
            }
        }

        public override Task BeforeFirstShown()
        {
            #region SignalR

            var signalRUrl = Xamarin.Forms.Application.Current.Properties["SignalRUrl"] as string;
            _hubConnection = new HubConnectionBuilder().WithUrl($"{signalRUrl}/reservationHub").Build();

            _hubConnection.On<Type, Domain.Entities.Reservation>("ReceiveReservation", (type, reservation) =>
            {
                if (reservation.ResourceId != Resource.Id) return;
                var reservationToCheck = Resource.Reservations.FirstOrDefault(r => r.Id == reservation.Id);
                switch (type)
                {
                    case Type.Create:
                    {
                        if (reservationToCheck != null) break;
                        Resource.Reservations.Add(reservation);
                        Reservations.Add(CreateAppointment(reservation));
                        break;
                    }
                    case Type.Update:
                    {
                        if (reservationToCheck == null) break;
                        Resource.Reservations.Remove(reservationToCheck);
                        Resource.Reservations.Add(reservation);
                        Reservations.Clear();
                        GenerateNonAccessibleBlocks(Reservations);
                        foreach (var resourceReservation in Resource.Reservations)
                        {
                            Reservations.Add(CreateAppointment(resourceReservation));
                        }

                        break;
                    }
                    case Type.Delete:
                    {
                        if (reservationToCheck == null) break;
                        Resource.Reservations.Remove(reservationToCheck);
                        Reservations.Clear();
                        GenerateNonAccessibleBlocks(Reservations);
                        foreach (var resourceReservation in Resource.Reservations)
                        {
                            Reservations.Add(CreateAppointment(resourceReservation));
                        }

                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            });

            _hubConnection.StartAsync();

            #endregion

            return base.BeforeFirstShown();
        }

        public void ScheduleOnCellTapped(object sender, CellTappedEventArgs e) 
        {
            if (e.Appointment != null)
            {
                if (((ScheduleReservation) e.Appointment).Subject == "Not Available")
                    _navigator.DisplayAlert("Error", "You cannot create a reservation at this time", "Ok");
                else
                    _navigator.NavigateToModal(new UpdateReservationViewModel(_navigator, $"{NavigationPath}/EditReservation", Resource.Reservations.FirstOrDefault(r => r.Id == ((ScheduleReservation)e.Appointment).ReservationId), Reservations));
            }
            else
                _navigator.NavigateToModal(new CreateReservationViewModel(_navigator, $"{NavigationPath}/CreateReservation", e.Datetime, Resource.Id, Reservations));
        }

        private ScheduleReservation CreateAppointment(Domain.Entities.Reservation reservation)
        {
            return new ScheduleReservation
            {
                StartTime = reservation.From,
                EndTime = reservation.To,
                Subject = "Reservation",
                Notes = $"Reservation by user: {reservation.UserId}",
                Color = Color.RoyalBlue,
                ReservationId = reservation.Id
            };
        } 
        
        private void GenerateNonAccessibleBlocks(ObservableCollection<ScheduleReservation> appointmentCollection)
        {
            for (var i = 0; i < 7; i++)
            {
                var lastToTime = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
                var dayOfWeekDate = DateTime.Now;
                var weekDay = WeekDays.Sunday;
                RecurrenceProperties recurrenceProperties;
                ScheduleReservation scheduleAppointment;
                foreach (var dayAndTime in Resource.Available.Where(d => d.DayOfWeek == (DayOfWeek)i).OrderBy(x => x.From))
                {
                    dayOfWeekDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)dayAndTime.DayOfWeek);
                    lastToTime = new DateTime(dayOfWeekDate.Year, dayOfWeekDate.Month, dayOfWeekDate.Day, lastToTime.Hour, lastToTime.Minute,0);
                    Enum.TryParse<WeekDays>(dayAndTime.DayOfWeek.ToString(), out weekDay);
                    
                    recurrenceProperties = new RecurrenceProperties
                    {
                        RecurrenceType = RecurrenceType.Weekly,
                        Interval = 1,
                        WeekDays = weekDay,
                        RecurrenceRange = RecurrenceRange.NoEndDate
                    };
                    
                    scheduleAppointment = new ScheduleReservation()
                    {
                        ReservationId = Guid.NewGuid(),
                        StartTime = lastToTime,
                        EndTime = new DateTime(dayOfWeekDate.Year, dayOfWeekDate.Month, dayOfWeekDate.Day, dayAndTime.From.Hour, dayAndTime.From.Minute,0),
                        Subject = "Not Available",
                        Color = Color.LightGray,
                    };
                    appointmentCollection.Add(scheduleAppointment);
                    
                    scheduleAppointment.RecurrenceRule = DependencyService.Get<IRecurrenceBuilder>().RRuleGenerator(recurrenceProperties, scheduleAppointment.StartTime, scheduleAppointment.EndTime);
                    
                    lastToTime = new DateTime(dayOfWeekDate.Year, dayOfWeekDate.Month, dayOfWeekDate.Day, dayAndTime.To.Hour, dayAndTime.To.Minute,0);
                }
                
                recurrenceProperties = new RecurrenceProperties
                {
                    RecurrenceType = RecurrenceType.Weekly,
                    Interval = 1,
                    WeekDays = weekDay,
                    RecurrenceRange = RecurrenceRange.NoEndDate
                };
                    
                scheduleAppointment = new ScheduleReservation()
                {
                    ReservationId = Guid.NewGuid(),
                    StartTime = lastToTime,
                    EndTime = new DateTime(dayOfWeekDate.Year, dayOfWeekDate.Month, dayOfWeekDate.Day, 23, 59,59),
                    Subject = "Not Available",
                    Color = Color.LightGray,
                };
                appointmentCollection.Add(scheduleAppointment);
                    
                scheduleAppointment.RecurrenceRule = DependencyService.Get<IRecurrenceBuilder>().RRuleGenerator(recurrenceProperties, scheduleAppointment.StartTime, scheduleAppointment.EndTime);

            }
        }
    }
}