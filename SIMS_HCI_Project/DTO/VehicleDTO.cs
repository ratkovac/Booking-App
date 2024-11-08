﻿using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class VehicleDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id { get; set; }

        private string city;
        public string City
        {
            get { return city; }
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
            get { return country; }
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
            get { return location; }
            set
            {
                if (value != location)
                {
                    location = value;
                    OnPropertyChanged("Location");
                }
            }
        }
        private List<Location> locations; 
        public List<Location> Locations
        {
            get { return locations; }
            set
            {
                if (value != locations)
                {
                    locations = value;
                    OnPropertyChanged("Locations");
                }
            }
        }

        private int capacity;
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }


        private Language language;
        public Language Language
        {
            get { return language; }
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged("Language");
                }
            }
        }
        private List<Language> languages;
        public List<Language> Languages
        {
            get { return languages; }
            set
            {
                if (value != languages)
                {
                    languages = value;
                    OnPropertyChanged("Languages");
                }
            }
        }

        private List<string> imagePaths;
        public List<string> ImagePaths
        {
            get { return imagePaths; }
            set
            {
                if (value != imagePaths)
                {
                    imagePaths = value;
                    OnPropertyChanged("ImagePaths");
                }
            }
        }

        private User user;
        public User User
        {
            get { return user; }
            set
            {
                if (value != user)
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }

        public VehicleDTO(Vehicle vehicle)
        {
            Id = vehicle.Id;
            Locations = vehicle.Locations;
            Capacity = vehicle.Capacity;
            Languages = vehicle.Languages;
            ImagePaths = vehicle.ImagePaths;
            User = vehicle.User;
        }

        public VehicleDTO()
        {
        }

        public Vehicle ToVehicle()
        {
            Vehicle vehicle = new Vehicle
            {
                Id = Id,
                Locations = Locations,
                Capacity = Capacity,
                Languages = Languages,
                ImagePaths = ImagePaths,
                User = User
            };
            return vehicle;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
