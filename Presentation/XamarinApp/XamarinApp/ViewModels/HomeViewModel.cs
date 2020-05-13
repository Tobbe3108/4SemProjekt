using Xamarin.Essentials;
using XamarinApp.Data;

namespace XamarinApp.ViewModels
{
    public class HomeViewModel
    {
        public User User { get; set; }

        public HomeViewModel()
        {
            User = new UserViewModel().User;
        }
    }
}