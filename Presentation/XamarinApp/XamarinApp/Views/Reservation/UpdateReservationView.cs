using System;
using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels.Reservation;

namespace XamarinApp.Views.Reservation
{
    public class UpdateReservationView : ContentPage
    { 
        public UpdateReservationView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
            BackgroundColor = Color.Gray.MultiplyAlpha(0.7);
            
            var title = new Label
            {
                Text = "Reservation",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            var notes = new Label
            {
                Text = ((UpdateReservationViewModel)BindingContext).ScheduleReservation.Notes,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            
            var fromTimePicker = new TimePicker
            {
                HorizontalOptions = LayoutOptions.Center,
                Format = "HH:mm",
                
                Time = ((UpdateReservationViewModel)BindingContext).fromTime
            };
            fromTimePicker.PropertyChanged += (sender, args) =>
                ((UpdateReservationViewModel) BindingContext).FromTimePickerOnPropertyChanged(sender, args);
            
            
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

                Time = ((UpdateReservationViewModel) BindingContext).toTime
            };
            toTimePicker.PropertyChanged += (sender, args) =>
                ((UpdateReservationViewModel) BindingContext).ToTimePickerOnPropertyChanged(sender, args);

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
            
            var chatButton = new Button
            {
                Text = "Customer service",
                Command = ((UpdateReservationViewModel)BindingContext).NavigateToChatView
            };
            
            var createButton = new Button
            {
                Text = "Create",
                Command = ((UpdateReservationViewModel)BindingContext).UpdateReservation
            };

            var cancelButton = new Button
            {
                Text = "Cancel",
                Command = ((UpdateReservationViewModel)BindingContext).NavigateBack
            };

            Grid.SetColumnSpan(chatButton, 2);
            Grid.SetRow(chatButton, 0);
            
            Grid.SetColumn(cancelButton, 0);
            Grid.SetRow(cancelButton, 1);
            
            Grid.SetColumn(createButton, 1);
            Grid.SetRow(createButton, 1);

            var btnGrid = new Grid
            {
                Children = {chatButton, cancelButton, createButton}
            };

            var stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.White,

                Children = {title, notes, fromTimeGrid, toTimeGrid, btnGrid}
            };

            Content = stackLayout;
        }
    }
}