using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorSPA.Client.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthCredentialsKeeper _credentialsKeeper;
        private readonly HttpClient _httpClient;

        public AuthStateProvider(AuthCredentialsKeeper credentialsKeeper, HttpClient httpClient)
        {
            _credentialsKeeper = credentialsKeeper;
            _httpClient = httpClient;
        }
        
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!_credentialsKeeper.HasCredentials())
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(_credentialsKeeper.Token);
            var claims = token.Claims;
            var claimsList = claims.ToList();
            
            var name = claimsList.First(c => c.Type == "sub").Value;
            claimsList.Add(new Claim(ClaimTypes.Name, name));
            
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_credentialsKeeper.Token}");
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(claimsList, "ApiWebAuth"));
            return Task.FromResult(new AuthenticationState(user));
        }
        
        public void Refresh()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}