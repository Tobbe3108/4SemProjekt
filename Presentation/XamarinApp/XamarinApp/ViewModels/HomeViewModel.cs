using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.Domain.Entities;
using XamarinApp.ViewModels.User;

namespace XamarinApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        public ICommand NavigateToProfileCommand { get; }
        public ICommand LogoutCommand { get; }
        #endregion
        
        public readonly ContentPage FrontLayerContentPage;
        public Domain.Entities.User User { get; set; }

        public HomeViewModel(INavigationService navigator, ContentPage frontLayerContentPage)
        {
            #region Navigation
            NavigationPath = "/Home";
            _navigator = navigator;
            NavigateToProfileCommand = new Command(() =>
            {
                _navigator.NavigateTo(new ProfileViewModel(_navigator, $"{NavigationPath}/Login"));
            });
            LogoutCommand = new Command(() =>
            {
                SecureStorage.RemoveAll();
                _navigator.PresentAsMainPage(new LoginViewModel(_navigator, $"{NavigationPath}/Login"));
            });
            #endregion
            
            FrontLayerContentPage = frontLayerContentPage;
            User = new UserViewModel().User;
        }
    }
}