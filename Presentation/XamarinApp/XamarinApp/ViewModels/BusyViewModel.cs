using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;

namespace XamarinApp.ViewModels
{
    public class BusyViewModel : ViewModelBase
    {
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;

        public BusyViewModel(INavigationService navigator, string navigationPath)
        {
            NavigationPath = navigationPath;
            _navigator = navigator;
        }
    }
}