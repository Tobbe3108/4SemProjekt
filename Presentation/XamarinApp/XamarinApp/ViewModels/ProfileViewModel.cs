using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Data;

namespace XamarinApp.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileInformation ProfileInformation { get; set; }
        public string ErrorMessage { get; private set; }
        private readonly User _user;
        
        public ProfileViewModel()
        {
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
            var mobileBffUrl = Application.Current.Properties["MobileBffUrl"] as string;
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
            
            var loginViewModel = new LoginViewModel
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