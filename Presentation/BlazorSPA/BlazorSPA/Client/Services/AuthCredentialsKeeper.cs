using Blazored.LocalStorage;

namespace BlazorSPA.Client.Services
{
    public class AuthCredentialsKeeper
    {
        private const string TokenKey = "Token";
        public string Token => _localStorage.ContainKey(TokenKey) ? _localStorage.GetItem<string>(TokenKey) : null;

        private readonly ISyncLocalStorageService _localStorage;

        public AuthCredentialsKeeper(ISyncLocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public void SetCredentials(string token)
        {
            _localStorage.SetItem(TokenKey, token);
        }

        public void ClearCredentials()
        {
            _localStorage.Clear();
        }

        public bool HasCredentials()
        {
            return _localStorage.ContainKey(TokenKey);
        }
    }
}