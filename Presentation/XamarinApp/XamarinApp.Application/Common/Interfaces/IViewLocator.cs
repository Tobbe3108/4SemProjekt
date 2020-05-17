using Xamarin.Forms;
using XamarinApp.Domain.Common;

namespace XamarinApp.Application.Common.Interfaces
{
    public interface IViewLocator
    {
        Page CreateAndBindPageFor<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase;
    }
}