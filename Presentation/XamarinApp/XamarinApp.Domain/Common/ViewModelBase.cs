using System.Threading.Tasks;

namespace XamarinApp.Domain.Common
{
    public abstract class ViewModelBase : PropertyChangedBase, IViewModelLifecycle
    {
        public virtual Task BeforeFirstShown()
        {
            return Task.CompletedTask;
        }

        public virtual Task AfterDismissed()
        {
            return Task.CompletedTask;
        }
    }
}