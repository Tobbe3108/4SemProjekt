using System;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Syncfusion.SfSchedule.XForms;
using Syncfusion.XForms.DataForm.Editors;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.Domain.Entities;

namespace XamarinApp.ViewModels.Resource
{
    public class ResourceViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        #endregion
        
        public readonly Domain.Entities.Resource Resource;
        public ScheduleAppointmentCollection BlockedAppointments { get; set; }
        public double MinFromTime = 0;
        public double MaxToTime = 24;

        public ResourceViewModel(INavigationService navigator, string navigationPath, Domain.Entities.Resource resource)
        {
            #region Navigation
            NavigationPath = navigationPath;
            _navigator = navigator;
            #endregion
            
            Resource = resource;
            BlockedAppointments = GenerateNonAccessibleBlocks();
        }

        public void ScheduleOnCellTapped(object sender, CellTappedEventArgs e)
        {
            if (e.Appointment != null) _navigator.DisplayAlert("Error", "You cannot create an overlapping reservation", "Ok");
            else
            {
                _navigator.NavigateToModal(new CreateReservationViewModel(_navigator, $"{NavigationPath}/CreateReservation", e.Datetime));
            }
            
        }
        
        private ScheduleAppointmentCollection GenerateNonAccessibleBlocks()
        {
            var minFrom = Resource.Available.Min(x => x.From);
            var maxTo = Resource.Available.Max(x => x.To);
            MinFromTime = double.Parse($"{minFrom.Hour}.{minFrom.Minute}");
            MaxToTime = double.Parse($"{maxTo.Hour}.{maxTo.Minute}");
            
            var blockedAppointments = new ScheduleAppointmentCollection();
            for (var i = 0; i <= 7; i++)
            {
                var lastToTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,0,0,0);
                foreach (var dayAndTime in Resource.Available.Where(d => d.DayOfWeek == (DayOfWeek)i).OrderBy(x => x.From))
                {
                    Enum.TryParse<WeekDays>(dayAndTime.DayOfWeek.ToString(), out var weekDays);
                    var recurrenceProperties = new RecurrenceProperties
                    {
                        RecurrenceType = RecurrenceType.Weekly,
                        Interval = 1,
                        WeekDays = weekDays,
                        RecurrenceRange = RecurrenceRange.NoEndDate
                    };
                    
                    var scheduleAppointment = new ScheduleAppointment
                    {
                        StartTime = lastToTime,
                        EndTime = dayAndTime.From,
                        Subject = "Not Available",
                        Color = Color.LightGray,
                    };
                    scheduleAppointment.RecurrenceRule = DependencyService.Get<IRecurrenceBuilder>().RRuleGenerator(recurrenceProperties, scheduleAppointment.StartTime, scheduleAppointment.EndTime);
                    
                    blockedAppointments.Add(scheduleAppointment);
                    
                    lastToTime = dayAndTime.To;
                }
            }
            
            return blockedAppointments;
        }
    }
}