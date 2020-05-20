using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels.Reservation;

namespace XamarinApp.Views.Reservation
{
    public class CreateReservationView : ContentPage
    { 
        public CreateReservationView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
            BackgroundColor = Color.Gray.MultiplyAlpha(0.7);
            
            var title = new Label
            {
                Text = "Create Reservation",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            var fromTimePicker = new TimePicker
            {
                HorizontalOptions = LayoutOptions.Center,
                Format = "HH:mm",
                
                Time = ((CreateReservationViewModel)BindingContext).fromTime
            };
            fromTimePicker.PropertyChanged += (sender, args) =>
                ((CreateReservationViewModel) BindingContext).FromTimePickerOnPropertyChanged(sender, args);
            
            
            var fromLabel = new Label
            {
                Text = "From:",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            Grid.SetColumn(fromLabel, 0);
            Grid.SetColumn(fromTimePicker , 1);

            var fromTimeGrid = new Grid
            {
                Children = {fromLabel, fromTimePicker}
            };

            var toTimePicker = new TimePicker
            {
                HorizontalOptions = LayoutOptions.Center,
                Format = "HH:mm",

                Time = ((CreateReservationViewModel) BindingContext).toTime
            };
            toTimePicker.PropertyChanged += (sender, args) =>
                ((CreateReservationViewModel) BindingContext).ToTimePickerOnPropertyChanged(sender, args);

            var toLabel = new Label
            {
                Text = "To:",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            Grid.SetColumn(toLabel, 0);
            Grid.SetColumn(toTimePicker , 1);

            var toTimeGrid = new Grid
            {
                Children = {toLabel, toTimePicker}
            };
            
            var createButton = new Button
            {
                Text = "Create",
                Command = ((CreateReservationViewModel)BindingContext).CreateReservation
            };

            var cancelButton = new Button
            {
                Text = "Cancel",
                Command = ((CreateReservationViewModel)BindingContext).NavigateBack
            };

            Grid.SetColumn(cancelButton, 0);
            Grid.SetColumn(createButton, 1);

            var grid = new Grid
            {
                Children = {cancelButton, createButton}
            };

            var stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.White,

                Children = {title, fromTimeGrid, toTimeGrid, grid}
            };

            Content = stackLayout;
        }
    }
}