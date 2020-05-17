using System;
using XamarinApp.Application.Common.Interfaces;
using XamarinApp.Domain.Common;

namespace XamarinApp.ViewModels.Resource
{
    public class CreateReservationViewModel : ViewModelBase
    {
        #region Navigation
        public string NavigationPath { get; }
        private readonly INavigationService _navigator;
        #endregion

        public readonly DateTime FromDate;

        public CreateReservationViewModel(INavigationService navigator, string navigationPath, DateTime fromDate)
        {
            #region Navigation
            NavigationPath = navigationPath;
            _navigator = navigator;
            #endregion
            
            FromDate = fromDate;
        }
    }
}