using Syncfusion.SfBusyIndicator.XForms;
using Xamarin.Forms;

namespace XamarinApp.Views
{
    public class BusyModalCS : ContentPage
    {
        public BusyModalCS()
        {
            BackgroundColor = Color.Transparent;
            var busyIndicator = new SfBusyIndicator
            {
                AnimationType = AnimationTypes.Cupertino,
                ViewBoxHeight = 150,
                ViewBoxWidth = 150,
                Title = "Loading..."
            };
            Content = busyIndicator;
        }
    }
}