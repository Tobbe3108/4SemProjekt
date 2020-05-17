using Syncfusion.SfBusyIndicator.XForms;
using Xamarin.Forms;
using XamarinApp.Domain.Common;

namespace XamarinApp.Views
{
    public class BusyView : ContentPage
    {
        public BusyView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
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