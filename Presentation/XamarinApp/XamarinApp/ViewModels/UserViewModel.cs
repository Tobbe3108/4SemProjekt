using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Utf8Json;
using Xamarin.Essentials;
using XamarinApp.Annotations;
using XamarinApp.Data;

namespace XamarinApp.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }

        public UserViewModel()
        {
            try
            {
                User = JsonSerializer.Deserialize<User>(SecureStorage.GetAsync("current_user").Result);
            }
            catch (Exception)
            {
                User = null;
            }
        }
    }
}