using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Xamarin.Forms;
using XamarinApp.Data;

namespace XamarinApp.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterInformation RegisterInformation { get; set; }
        public string ErrorMessage { get; private set; }

        public RegisterViewModel()
        {
            RegisterInformation = new RegisterInformation();
        }
        
        public async Task<bool> Register()
        {
            if (!ValidateStringNotNull(nameof(RegisterInformation.Email), RegisterInformation.Email)) return false;
            if (!ValidateStringNotNull(nameof(RegisterInformation.Password), RegisterInformation.Password)) return false;
            if (!ValidateStringNotNull(nameof(RegisterInformation.Username), RegisterInformation.Username)) return false;
            if (!ValidateStringNotNull(nameof(RegisterInformation.FirstName), RegisterInformation.FirstName)) return false;
            if (!ValidateStringNotNull(nameof(RegisterInformation.LastName), RegisterInformation.LastName)) return false;

            var mobileBffUrl = Application.Current.Properties["MobileBffUrl"] as string;
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

            var loginViewModel = new LoginViewModel
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