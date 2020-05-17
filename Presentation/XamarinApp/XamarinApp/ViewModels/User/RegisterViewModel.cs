using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Flurl;
using Flurl.Http;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.Domain.Entities;
using XamarinApp.ViewModels.Resource;
using XamarinApp.Views.Resource;

namespace XamarinApp.ViewModels.User
{
    public class RegisterViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        public ICommand NavigateToResourceListPageCommand { get; }
        public ICommand NavigateBack { get; }
        #endregion
        
        public RegisterInformation RegisterInformation { get; set; }
        public string ErrorMessage { get; private set; }


        public RegisterViewModel(INavigationService navigator, string navigationPath)
        {
            #region Navigation
            _navigator = navigator;
            NavigationPath = navigationPath;
            NavigateToResourceListPageCommand = new Command(async () =>
            {
                await _navigator.NavigateToModal(new BusyViewModel(_navigator, $"{NavigationPath}/Busy"));
                if (await Register())
                {
                    await _navigator.PopModal();
                    _navigator.PresentAsNavigatableMainPage(new HomeViewModel(_navigator, new ResourceListView(new ResourceListViewModel(_navigator, $"{NavigationPath}/ResourceList"))));
                }
                else
                {
                    await _navigator.PopModal();
                    await _navigator.DisplayAlert("Alert", ErrorMessage, "OK");
                }
            });
            NavigateBack = new Command(() => { _navigator.NavigateBack(); });
            #endregion

            RegisterInformation = new RegisterInformation();
        }

        public async Task<bool> Register()
        {
            if (!ValidateStringNotNull(nameof(RegisterInformation.Email), RegisterInformation.Email)) return false;
            if (!ValidateStringNotNull(nameof(RegisterInformation.Password), RegisterInformation.Password)) return false;
            if (!ValidateStringNotNull(nameof(RegisterInformation.Username), RegisterInformation.Username)) return false;
            if (!ValidateStringNotNull(nameof(RegisterInformation.FirstName), RegisterInformation.FirstName)) return false;
            if (!ValidateStringNotNull(nameof(RegisterInformation.LastName), RegisterInformation.LastName)) return false;

            var mobileBffUrl = Xamarin.Forms.Application.Current.Properties["MobileBffUrl"] as string;
            HttpResponseMessage result;
            try
            {
                result = await mobileBffUrl.AppendPathSegment("User").PostJsonAsync(new
                {
                    RegisterInformation.Username,
                    RegisterInformation.Email,
                    RegisterInformation.FirstName,
                    RegisterInformation.LastName,
                    RegisterInformation.Password
                });
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
        
            if (!result.IsSuccessStatusCode)
            {
                ErrorMessage = "Det var ikke muligt at registrere dig";
                return false;
            }

            var loginViewModel = new LoginViewModel(_navigator, $"{NavigationPath}/Login")
            {
                LoginInformation = new LoginInformation
                {
                    UserId = RegisterInformation.Username,
                    Password = RegisterInformation.Password
                }
            };
            
            if (await loginViewModel.Login()) return true;
            
            ErrorMessage = loginViewModel.ErrorMessage;
            return false;

        }

        private bool ValidateStringNotNull(string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) return true;
            ErrorMessage = $"{name} cannot be empty";
            return false;
        }
    }
}