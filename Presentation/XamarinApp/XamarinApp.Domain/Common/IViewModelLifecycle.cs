using System.Threading.Tasks;

namespace XamarinApp.Domain.Common
{
    public interface IViewModelLifecycle
    {
        Task BeforeFirstShown();
        Task AfterDismissed();
    }
}