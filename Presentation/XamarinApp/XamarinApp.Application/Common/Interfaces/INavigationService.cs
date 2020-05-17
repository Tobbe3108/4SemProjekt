using System.Threading.Tasks;
using XamarinApp.Domain.Common;

namespace XamarinApp.Application.Common.Interfaces
{
    public interface INavigationService
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task DisplayAlert(string title, string message, string accept, string cancel);
        void PresentAsMainPage(ViewModelBase viewModel);
        void PresentAsNavigatableMainPage(ViewModelBase viewModel);
        Task NavigateTo(ViewModelBase viewModel);
        Task NavigateToModal(ViewModelBase viewModel);
        Task NavigateBack();
        Task PopModal();
        Task NavigateBackToRoot();

    }
}