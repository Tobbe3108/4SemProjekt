using System.Net.Http;
using Blazored.LocalStorage;

namespace BlazorSPA.Client.Services
{
    public class AuthCredentialsKeeper
    {
        private const string TokenKey = "Token";
        public string Token => _localStorage.ContainKey(TokenKey) ? _localStorage.GetItem<string>(TokenKey) : null;

        private readonly ISyncLocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public AuthCredentialsKeeper(ISyncLocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        public void SetCredentials(string token)
        {
            _localStorage.SetItem(TokenKey, token);

            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public void ClearCredentials()
        {
            _localStorage.Clear();
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }

        public bool HasCredentials()
        {
            return _localStorage.ContainKey(TokenKey);
        }
    }
}