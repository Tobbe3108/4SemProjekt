using System.ComponentModel;
using System.Runtime.CompilerServices;
using XamarinApp.Annotations;

namespace XamarinApp.Data
{
    public class Resource : INotifyPropertyChanged
    {
        private string _name;
        private string _description;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public Resource(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}