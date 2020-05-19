using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XamarinApp.Domain.Entities
{
    public class Resource : INotifyPropertyChanged
    {
        private Guid _id;
        private string _name;
        private string _description;
        private List<DayAndTime> _available;
        private List<Reservation> _reservations;
        
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

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

        public List<DayAndTime> Available
        {
            get => _available;
            set
            {
                _available = value;
                OnPropertyChanged("Available");
            }
        }
        
        public List<Reservation> Reservations
        {
            get => _reservations;
            set
            {
                _reservations = value;
                OnPropertyChanged("Reservations");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}