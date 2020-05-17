using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Flurl;
using Flurl.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.Domain.Entities;

namespace XamarinApp.ViewModels.User
{
    public class ProfileViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        public ICommand NavigateBack { get; }
        public ICommand UpdateCommand { get; set; }
        #endregion
        
        public ProfileInformation ProfileInformation { get; set; }
        public string ErrorMessage { get; private set; }
        private readonly Domain.Entities.User _user;
        
        public ProfileViewModel(INavigationService navigator, string navigationPath)
        {
            #region Navigation
            NavigationPath = navigationPath;
            _navigator = navigator;
            NavigateBack = new Command(() => { _navigator.NavigateBack(); });
            UpdateCommand = new Command(async () =>
            {
                if (!await Update()) await _navigator.DisplayAlert("Alert", ErrorMessage, "OK");
            });
            #endregion
            
            _user = new UserViewModel().User;
            ProfileInformation = new ProfileInformation
            {
                Username = _user.Username,
                Email = _user.Email,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                Address = _user.Address,
                City = _user.City,
                Country = _user.Country,
                ZipCode = _user.ZipCode
            };
        }


        public async Task<bool> Update()
        {
            var mobileBffUrl = Xamarin.Forms.Application.Current.Properties["MobileBffUrl"] as string;
            var token = await SecureStorage.GetAsync("jwt_token");
            HttpResponseMessage result;
            try
            {
                result = await mobileBffUrl.AppendPathSegment($"User/{_user.Id}").WithOAuthBearerToken(token).PutJsonAsync(new
                {
                    id = _user.Id, 
                    username = ProfileInformation.Username,
                    email = ProfileInformation.Email,
                    password = ProfileInformation.Password,
                    firstName = ProfileInformation.FirstName,
                    lastName = ProfileInformation.LastName,
                    address = ProfileInformation.Address,
                    city = ProfileInformation.City,
                    country = ProfileInformation.Country,
                    zipCode = ProfileInformation.ZipCode
                });
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }

            if (!result.IsSuccessStatusCode)
            {
                ErrorMessage = "Det var ikke muligt at opdatere dine oplysninger";
                return false;
            }
            
            Thread.Sleep(100);
            
            var loginViewModel = new LoginViewModel(_navigator, $"{NavigationPath}/Login")
            {
                LoginInformation = new LoginInformation
                {
                    UserId = ProfileInformation.Username,
                    Password = ProfileInformation.Password ?? _user.password
                }
            };

            if (await loginViewModel.Login()) return true;
            
            ErrorMessage = loginViewModel.ErrorMessage;
            return false;
        }
    }
}