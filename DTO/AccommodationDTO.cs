using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class AccommodationDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id { get; set; }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if(value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private string location;
        public string Location
        {
            get 
            {
                return location; 
            }
            set
            {
                if(value != location)
                {
                    location = value;
                    OnPropertyChanged("Location");
                }
            }
        }

        private AccommodationType type;
        public AccommodationType Type
        {
            get
            {
                return type;
            }
            set
            {
                if(value != type)
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private int capacity;
        public int Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                if(capacity != value)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }

        private int minReservationDays;
        public int MinReservationDays
        {
            get
            {
                return minReservationDays;
            }
            set
            {
                if(minReservationDays != value)
                {
                    minReservationDays = value;
                    OnPropertyChanged("MinReservationDays");
                }
            }
        }

        private int daysBeforeCancel;
        public int DaysBeforeCancel
        {
            get
            {
                return daysBeforeCancel;
            }
            set
            {
                if(daysBeforeCancel != value)
                {
                    daysBeforeCancel = value;
                    OnPropertyChanged("DaysBeforeCancel");
                }
            }
        }

        private User user;
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                if(user != value)
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }
        public AccommodationDTO(Accommodation accommodation)
        {
            name = accommodation.Name;
            location = accommodation.Location;
            type = accommodation.Type;
            capacity = accommodation.Capacity;
            minReservationDays = accommodation.MinReservationDays;
            daysBeforeCancel = accommodation.DaysBeforeCancel;
            user = accommodation.User;
        }
        public Accommodation ToAccommodation()
        {
            Accommodation accomodation = new Accommodation(Id, name, location, type, capacity, minReservationDays, daysBeforeCancel, user);
            return accomodation;
        }
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
