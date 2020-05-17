using System;
using System.Linq;
using Syncfusion.SfBusyIndicator.XForms;
using Syncfusion.SfSchedule.XForms;
using Syncfusion.XForms.Cards;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels.Resource;

namespace XamarinApp.Views.Resource
{
    public class ResourceView : ContentPage
    {
        public ResourceView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;

            BackgroundColor = Color.White;
            
            var name = new Label
            {
                Text = ((ResourceViewModel) BindingContext).Resource.Name,
                FontAttributes = FontAttributes.Bold, 
                Padding = new Thickness(10,0),
                FontSize = 21
            };
            
            var description = new Label
            {
                Text = ((ResourceViewModel) BindingContext).Resource.Description,
                Padding = new Thickness(10,0),
                FontSize = 15
            };
            
            var schedule = new SfSchedule
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                ScheduleView = ScheduleView.DayView,
                TimeInterval = 15,
                ShowCurrentTimeIndicator = true,
                CurrentTimeIndicatorColor = Color.BlueViolet,
                DataSource =  ((ResourceViewModel) BindingContext).BlockedAppointments,
                DayViewSettings = new DayViewSettings
                {
                    WorkStartHour = 8,
                    WorkEndHour = 16,
                    StartHour = ((ResourceViewModel) BindingContext).MinFromTime,
                    EndHour = ((ResourceViewModel) BindingContext).MaxToTime,
                    DayLabelSettings = new DayLabelSettings
                    {
                        DayFormat = "EEEE",
                        TimeFormat = "HH:mm"
                    }
                }
            };
            schedule.CellTapped += ((ResourceViewModel) BindingContext).ScheduleOnCellTapped;
            
            var stackLayout = new StackLayout
            {
                Spacing = 10,
                Children = { name, description, schedule }
            };
            
            Content = stackLayout;
        }
    }
}