using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Xamarin.Essentials;

namespace XamarinApp.ViewModels
{
    public class LoginViewModel
    {
        public string UsernameOrEmail { get; set; } = "Tobbe3108";
        public string Password { get; set; } = "Zxasqw12";
        public string ErrorMessage { get; private set; }
        
        public async Task<bool> Login()
        {
            if (string.IsNullOrWhiteSpace(UsernameOrEmail))
            {
                ErrorMessage = "User Id cannot be empty";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Password cannot be empty";
                return false;
            }
            
            var result = await "http://10.0.2.2:5000/".AppendPathSegment("Auth/login").PostJsonAsync(new { UsernameOrEmail, Password });
            if (result.IsSuccessStatusCode)
            {
                var token = await result.Content.ReadAsStringAsync();
                if (token == null)
                {
                    ErrorMessage = new ArgumentNullException(nameof(token)).Message;
                    return false;
                }
                
                var (sucess, message) = await SaveCredentials(token);
                if (!sucess)
                {
                    ErrorMessage = message;
                }
                
                return true;
            }

            return false;
        }

        private async Task<(bool, string)> SaveCredentials(string token)
        {
            try
            {
                await SecureStorage.SetAsync("jwt_token", token);
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }

            return (true, null);
        }
    }
}