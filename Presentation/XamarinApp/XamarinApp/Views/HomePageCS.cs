using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Syncfusion.XForms.Backdrop;
using Syncfusion.XForms.Buttons;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.ViewModels;
using Color = System.Drawing.Color;

namespace XamarinApp.Views
{
    public class HomePageCS : SfBackdropPage
    {
        private HomeViewModel _homeViewModel;
        public HomePageCS(ContentPage frontLayerContentPage)
        {
            BindingContext = _homeViewModel
                = new HomeViewModel();
            
            Title = "Home Page";
            IsBackLayerRevealed = false;
            BackLayerRevealOption = RevealOption.Auto;

            var profile = new SfButton
            {
                Text = "Profile",
                BackgroundColor = Color.Transparent
            };
            profile.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new ProfilePageCS());
            };
            
            var logout = new SfButton
            {
                Text = "Logout",
                BackgroundColor = Color.Transparent
            };
            logout.Clicked += async (sender, e) =>
            {
                SecureStorage.RemoveAll();
                Navigation.InsertPageBefore(new LoginPageCS(),this);
                await Navigation.PopToRootAsync();
            };
            
            var backLayer = new BackdropBackLayer
            {
                Content = new StackLayout
                {
                    Padding = new Thickness(10,10,10,10),
                    Children = {profile, logout}
                }
            };
            BackLayer = backLayer;
            
            var frontLayer = new BackdropFrontLayer
            {
                Content = frontLayerContentPage.Content
            };
            FrontLayer = frontLayer;
        }
    }
}