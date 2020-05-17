using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Utf8Json;
using Xamarin.Essentials;
using XamarinApp.Domain.Entities;

namespace XamarinApp.ViewModels
{
    public class UserViewModel
    {
        public Domain.Entities.User User { get; set; }

        public UserViewModel()
        {
            try
            {
                User = JsonSerializer.Deserialize<Domain.Entities.User>(SecureStorage.GetAsync("current_user").Result);
            }
            catch (Exception)
            {
                User = null;
            }
        }
    }
}