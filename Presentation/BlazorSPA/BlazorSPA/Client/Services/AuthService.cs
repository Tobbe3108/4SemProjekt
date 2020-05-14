using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorSPA.Client.DTOs;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace BlazorSPA.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;
        private readonly AuthCredentialsKeeper _credentialsKeeper;
        private readonly IConfiguration _configuration;
        private readonly AuthStateProvider _authenticationStateProvider;

        public AuthService(HttpClient client, AuthenticationStateProvider authenticationStateProvider, AuthCredentialsKeeper credentialsKeeper, IConfiguration configuration)
        {
            _client = client;
            _credentialsKeeper = credentialsKeeper;
            _configuration = configuration;
            _authenticationStateProvider = (AuthStateProvider)authenticationStateProvider;
        }
        
        public async Task<(bool, string)> Login(LoginDTO loginDTO)
        {
            var mobileBffUrl = _configuration.GetValue<string>("WebBff:BaseUrl");
            HttpResponseMessage result;
            try
            {
                //result = await mobileBffUrl.AppendPathSegment("Auth/login").PostJsonAsync(new { usernameOrEmail = loginDTO.UsernameOrEmail, password = loginDTO.Password });
                _client.BaseAddress = new Uri(mobileBffUrl);
                result = await _client.PostAsJsonAsync("Auth/login", loginDTO);
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        
            if (!result.IsSuccessStatusCode)
            {
                return (false, "Det var ikke muligt at logge dig ind");
            }
            
            var token = await result.Content.ReadAsStringAsync();
            if (token == null)
            {
                return (false, new ArgumentNullException(nameof(token)).Message);
            }
            _credentialsKeeper.SetCredentials(token);
            _authenticationStateProvider.Refresh();
            return (true, null);
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }
    }
}