using System.Net.Http.Headers;
using Syncfusion.XForms.Pickers;
using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels.Resource;

namespace XamarinApp.Views.Resource
{
    public class CreateReservationView : ContentPage
    {
        public CreateReservationView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
            var fromTimePicker = new SfTimePicker
            {
                HeaderText = "From",
                MinuteInterval = 15,
                Format = TimeFormat.HH_mm
            };
            
            var toTimePicker = new SfTimePicker
            {
                HeaderText = "To",
                MinuteInterval = 15,
                Format = TimeFormat.HH_mm
            };
            
            var stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.White,
                
                Children = { label, fromTimePicker, toTimePicker }
            };
            
            Content = stackLayout;
        }
    }
}