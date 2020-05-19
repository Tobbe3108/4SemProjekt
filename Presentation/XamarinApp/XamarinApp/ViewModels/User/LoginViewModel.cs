using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Flurl;
using Flurl.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;
using XamarinApp.Domain.Entities;
using XamarinApp.ViewModels.Resource;
using XamarinApp.Views.Resource;
using JsonSerializer = Utf8Json.JsonSerializer;

namespace XamarinApp.ViewModels.User
{
    public class LoginViewModel : ViewModelBase
    {
        #region Navigation
        private readonly INavigationService _navigator;
        public string NavigationPath { get; }
        public ICommand NavigateToRegisterPageCommand { get; }
        public ICommand NavigateToResourceListPageCommand { get; }
        #endregion
        
        public LoginInformation LoginInformation { get; set; }
        public string ErrorMessage { get; private set; }

        public LoginViewModel(INavigationService navigator, string navigationPath)
        {
            #region Navigation
            _navigator = navigator;
            NavigationPath = navigationPath;
            NavigateToRegisterPageCommand = new Command(() => { _navigator.NavigateTo(new RegisterViewModel(_navigator, $"{NavigationPath}/Register")); });
            NavigateToResourceListPageCommand = new Command(async () =>
            {
                await _navigator.NavigateToModal(new BusyViewModel(_navigator, $"{NavigationPath}/Busy"));
                if (await Login())
                {
                    await _navigator.PopModal();
                    var resourceListViewModel = new ResourceListViewModel(_navigator, $"{NavigationPath}/ResourceList");
                    await resourceListViewModel.BeforeFirstShown();
                    _navigator.PresentAsNavigatableMainPage(new HomeViewModel(_navigator, new ResourceListView(resourceListViewModel)));
                }
                else
                {
                    await _navigator.PopModal();
                    await _navigator.DisplayAlert("Alert", ErrorMessage, "OK");
                }
            });
            #endregion
            
            LoginInformation = new LoginInformation();
        }
        
        public async Task<bool> Login()
        {
            if (!ValidateStringNotNull(nameof(LoginInformation.UserId), LoginInformation.UserId)) return false;
            if (!ValidateStringNotNull(nameof(LoginInformation.Password), LoginInformation.Password)) return false;

            var mobileBffUrl = Xamarin.Forms.Application.Current.Properties["MobileBffUrl"] as string;
            HttpResponseMessage result;
            try
            {
                result = await mobileBffUrl.AppendPathSegment("Auth/login").PostJsonAsync(new { usernameOrEmail = LoginInformation.UserId, password = LoginInformation.Password });
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
        
            if (!result.IsSuccessStatusCode)
            {
                ErrorMessage = "Det var ikke muligt at logge dig ind";
                return false;
            }
            
            var token = await result.Content.ReadAsStringAsync();
            if (token == null)
            {
                ErrorMessage = new ArgumentNullException(nameof(token)).Message;
                return false;
            }
                
            var success = await SaveCredentials("jwt_token", token);
            if (success)
                if (await SetActiveUser())
                    return true;
        
            return false;
        }
        
        private bool ValidateStringNotNull(string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) return true;
            ErrorMessage = $"{name} cannot be empty";
            return false;
        }
        
        public async Task<bool> SetActiveUser()
        {
            var mobileBffUrl = Xamarin.Forms.Application.Current.Properties["MobileBffUrl"] as string;
            var token = await SecureStorage.GetAsync("jwt_token");
            var result = await mobileBffUrl.AppendPathSegment("User").WithOAuthBearerToken(token).GetJsonAsync<UserViewModel>();
            if (result.User != null)
            {
                result.User.password = LoginInformation.Password;
                await SaveCredentials("current_user", JsonSerializer.ToJsonString<Domain.Entities.User>(result.User));
                return true;
            }
        
            ErrorMessage = "User not created";
            return false;
        }
        
        private async Task<bool> SaveCredentials(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
                //var token = await SecureStorage.GetAsync("jwt_token");
                //var user = JsonSerializer.Deserialize<string>(await SecureStorage.GetAsync("current_user"));
                //SecureStorage.Remove("jwt_token");
                //SecureStorage.RemoveAll();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                return false;
            }
        
            return true;
        }
    }
}