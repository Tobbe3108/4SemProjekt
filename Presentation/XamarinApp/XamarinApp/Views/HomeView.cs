using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Syncfusion.XForms.Backdrop;
using Syncfusion.XForms.Buttons;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Domain.Common;
using XamarinApp.ViewModels;
using Color = System.Drawing.Color;

namespace XamarinApp.Views
{
    public class HomeView : SfBackdropPage
    {
        private HomeViewModel _homeViewModel;
        public HomeView(ViewModelBase bindingContext)
        {
            BindingContext = bindingContext;
            
            Title = "Home Page";
            IsBackLayerRevealed = false;
            BackLayerRevealOption = RevealOption.Auto;

            var profile = new SfButton
            {
                Text = "Profile",
                BackgroundColor = Color.Transparent,
                Command = ((HomeViewModel)BindingContext).NavigateToProfileViewCommand
            };
            
            var reservations = new SfButton
            {
                Text = "Reservations",
                BackgroundColor = Color.Transparent,
                Command = ((HomeViewModel)BindingContext).NavigateToReservationsViewCommand
            };

            var logout = new SfButton
            {
                Text = "Logout",
                BackgroundColor = Color.Transparent,
                Command = ((HomeViewModel)BindingContext).LogoutCommand
            };

            var backLayer = new BackdropBackLayer
            {
                Content = new StackLayout
                {
                    Padding = new Thickness(10,10,10,10),
                    Children = {profile,reservations, logout}
                }
            };
            BackLayer = backLayer;
            
            var frontLayer = new BackdropFrontLayer
            {
                Content = ((HomeViewModel)BindingContext).FrontLayerContentPage.Content
            };
            FrontLayer = frontLayer;
        }
    }
}