using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels.Reservation;
using SelectionMode = Syncfusion.SfSchedule.XForms.SelectionMode;

namespace XamarinApp.Views.Reservation
{
    public class ReservationsView : ContentPage
    {
        public ReservationsView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
            var name = new Label
            {
                Text = "Your reservations",
                FontAttributes = FontAttributes.Bold,
                Padding = new Thickness(10, 0),
                FontSize = 21
            };

            var schedule = new SfSchedule
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ScheduleView = ScheduleView.DayView,
                TimeInterval = 60,
                ShowCurrentTimeIndicator = true,
                ShowResourceView = true,
                CurrentTimeIndicatorColor = Color.BlueViolet,
                DataSource = ((ReservationsViewModel) BindingContext).ScheduleReservations,
                ScheduleResources = ((ReservationsViewModel) BindingContext).ScheduleResources,
                ResourceViewSettings = new ResourceViewSettings
                {
                    SelectionMode = SelectionMode.Multiple
                },
                DayViewSettings = new DayViewSettings
                {
                    WorkStartHour = 8,
                    WorkEndHour = 16,
                    StartHour = ((ReservationsViewModel) BindingContext).MinFromTime,
                    EndHour = ((ReservationsViewModel) BindingContext).MaxToTime,
                    DayLabelSettings = new DayLabelSettings
                    {
                        DayFormat = "EEEE",
                        TimeFormat = "HH:mm"
                    }
                }
            };
            schedule.CellTapped += ((ReservationsViewModel) BindingContext).ScheduleOnCellTapped;

            var stackLayout = new StackLayout
            {
                Spacing = 10,
                Children = {name, schedule}
            };
            
            Content = stackLayout;
        }
    }
}