using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Xml;

namespace XamarinApp.Models
{
    public class LoginViewModel
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
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
            
            // var client = new RestClient("https://localhost:5001/");
            // var request = new RestRequest("Auth/login", Method.POST)
            //     .AddJsonBody(new {UsernameOrEmail, Password});
            //
            // var result = await client.GetAsync<string>(request, CancellationToken.None);
            
            //Temp
            ErrorMessage = result;
            return false;

            //return true;
        }
    }
}