using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApp.Annotations;
using XamarinApp.Data;

namespace XamarinApp.ViewModels
{
    public class ResourceViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Resource> Resources { get; set; }
        
        public ResourceViewModel()
        {
            Resources = new ObservableCollection<Resource>();
        }

        public async Task GenerateResources()
        {
            await Task.Delay(5000);
            for (var i = 0; i < 12; i++)
            {
                Resources.Add(new Resource($"Name {i}", $"Description {i}"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}