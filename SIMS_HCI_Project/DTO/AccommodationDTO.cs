﻿using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BookingApp.Model.AccommodationTypeEnum;


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

        private string city;
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string country;
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                if (value != country)
                {
                    country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        private Location location;
        public Location Location
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

        private string displayLocation;

        public string DisplayLocation
        {
            get
            {
                return $"{Location.City}, {Location.Country}"; 
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

        private string imagePath;
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                if (imagePath != value)
                {
                    imagePath = value;
                    OnPropertyChanged("Image path");
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
        private Image frontImage;
        public Image FrontImage
        {
            get
            {
                return frontImage;
            }
            set
            {
                if (frontImage != value)
                {
                    frontImage = value;
                    OnPropertyChanged("FrontImage");
                }
            }
        }
        public AccommodationDTO(Accommodation accommodation)
        {
            Id = accommodation.Id;
            name = accommodation.Name;
            location = accommodation.Location;
            type = accommodation.Type;
            capacity = accommodation.Capacity;
            minReservationDays = accommodation.MinReservationDays;
            daysBeforeCancel = accommodation.DaysBeforeCancel;
            user = accommodation.User;
        }
        

        public AccommodationDTO()
        {
        }

        public Accommodation ToAccommodation()
        {
            Accommodation accomodation = new Accommodation(Id, name, location, type, capacity, minReservationDays, daysBeforeCancel,user);
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
