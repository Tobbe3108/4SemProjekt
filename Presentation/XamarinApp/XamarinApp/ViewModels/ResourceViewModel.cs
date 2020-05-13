using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using XamarinApp.Annotations;
using XamarinApp.Data;

namespace XamarinApp.ViewModels
{
    public class ResourceViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Resource> Resources { get; set; }
        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        public ResourceViewModel()
        {
            Resources = new ObservableCollection<Resource>();
        }

        public async Task GenerateResources()
        {
            IsLoading = true;
            await Task.Delay(5000);
            for (var i = 0; i < 12; i++)
            {
                Resources.Add(new Resource($"Name {i}", $"Description {i}"));
            }
            IsLoading = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}