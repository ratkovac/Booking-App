using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookingApp.Command;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using Microsoft.AspNetCore.Components.Forms;
using static BookingApp.Model.AccommodationTypeEnum;

namespace BookingApp.View.ViewModel.Guest
{
    public class FilterAndSortViewModel
    {
        ICollectionView Accommodations { get; set; }
        private System.Windows.Navigation.NavigationService NavigationService { get; set; }
        private Cache.Cache Cache { get; set; }
        public FilterAndSortViewModel(ICollectionView accommodations, ObservableCollection<string> locations, System.Windows.Navigation.NavigationService navigationService, Cache.Cache cache)
        {
            Accommodations = accommodations;
            Locations = locations;
            NavigationService = navigationService;
            Cache = cache;
            LoadCachedData();
        }

        private ObservableCollection<string> locations;
        public ObservableCollection<string> Locations
        {
            get => locations;
            set
            {
                locations = value;
                OnPropertyChanged(nameof(Locations));
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
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged(nameof(Location));
                    Cache.Add("Location", location);
                    Accommodations.Refresh();
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

        private string selectedLocation;
        public string SelectedLocation
        {
            get => selectedLocation;
            set
            {
                if (selectedLocation != value)
                {
                    selectedLocation = value;
                    OnPropertyChanged(nameof(SelectedLocation));
                    Cache.Add("SelectedLocation", selectedLocation);
                    Accommodations.Refresh();
                }
            }
        }

        private ObservableCollection<string> _filteredLocations;
        public ObservableCollection<string> FilteredLocations
        {
            get => _filteredLocations;
            set { _filteredLocations = value; OnPropertyChanged(nameof(FilteredLocations)); }
        }

        private string capacity;
        public string Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                if (capacity != value)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                    Cache.Add("Capacity", capacity);
                    Accommodations.Refresh();
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
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged(nameof(Type));
                    Cache.Add("Type", type);
                    Accommodations.Refresh();
                }
            }
        }

        private string minReservationDays;
        public string MinReservationDays
        {
            get
            {
                return minReservationDays;
            }
            set
            {
                if (value != minReservationDays)
                {
                    minReservationDays = value;
                    OnPropertyChanged("MinReservationDays");
                    Cache.Add("MinReservationDays", minReservationDays);
                    Accommodations.Refresh();
                }
            }
        }

        private bool isHutChecked;
        public bool IsHutChecked
        {
            get => isHutChecked;
            set
            {
                if (isHutChecked != value)
                {
                    isHutChecked = value;
                    OnPropertyChanged(nameof(IsHutChecked));
                    Cache.Add("IsHutChecked", isHutChecked);
                    Accommodations.Refresh();
                }
            }
        }

        private bool isApartmentChecked;
        public bool IsApartmentChecked
        {
            get => isApartmentChecked;
            set
            {
                if (isApartmentChecked != value)
                {
                    isApartmentChecked = value;
                    OnPropertyChanged(nameof(IsApartmentChecked));
                    Cache.Add("IsApartmentChecked", isApartmentChecked);
                    Accommodations.Refresh();
                }
            }
        }

        private bool isHouseChecked;
        public bool IsHouseChecked
        {
            get => isHouseChecked;
            set
            {
                if (isHouseChecked != value)
                {
                    isHouseChecked = value;
                    OnPropertyChanged(nameof(IsHouseChecked));
                    Cache.Add("IsHouseChecked", isHouseChecked);
                    Accommodations.Refresh();
                }
            }
        }

        public ICommand CheckBoxChangedCommand { get; }

        public FilterAndSortViewModel()
        {
            CheckBoxChangedCommand = new RelayCommand(param => OnCheckBoxChanged());
        }

        private void OnCheckBoxChanged()
        {
            Accommodations.Refresh();
        }
        private bool FilterAccommodations(object item)
        {
            if (!(item is AccommodationDTO accommodation))
                return false;

            //bool matchesSearchText = string.IsNullOrWhiteSpace(SearchText) || accommodation.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            bool matchesCapacity = string.IsNullOrWhiteSpace(Capacity) || accommodation.Capacity >= int.Parse(Capacity);
            bool matchesMinReservationDays = string.IsNullOrWhiteSpace(MinReservationDays) || accommodation.MinReservationDays.ToString().Contains(MinReservationDays, StringComparison.OrdinalIgnoreCase);
            bool matchesLocation = string.IsNullOrWhiteSpace(SelectedLocation) || accommodation.Location.ToString().Equals(SelectedLocation, StringComparison.OrdinalIgnoreCase);
            bool matchesIsCheckedAccomodationType = IsCheckedAccomodationType(accommodation);


            return matchesLocation && matchesCapacity && matchesMinReservationDays && matchesIsCheckedAccomodationType;
        }

        private bool AnyCheckBoxChecked => IsHutChecked || IsApartmentChecked || IsHouseChecked;

        bool IsCheckedAccomodationType(AccommodationDTO accommodation)
        {
            if (!AnyCheckBoxChecked) return true; 

            bool matchesTypeApartment = IsApartmentChecked &&
                                        accommodation.Type == AccommodationTypeEnum.AccommodationType.Apartment;

            bool matchesTypeHut = IsHutChecked &&
                                  accommodation.Type == AccommodationTypeEnum.AccommodationType.Hut;

            bool matchesTypeHouse = IsHouseChecked &&
                                    accommodation.Type == AccommodationTypeEnum.AccommodationType.House;

            return matchesTypeApartment || matchesTypeHut || matchesTypeHouse;
        }

        public void OnClick_Confirm()
        {
            Accommodations.Filter = FilterAccommodations;
            NavigationService.GoBack();
        }

        public void LoadCachedData()
        {
            if (Cache.ContainsKey("Location"))
            {
                location = (Location)Cache.Get("Location");
                OnPropertyChanged(nameof(Location));
            }

            if (Cache.ContainsKey("SelectedLocation"))
            {
                selectedLocation = (string)Cache.Get("SelectedLocation");
                OnPropertyChanged(nameof(SelectedLocation));
            }

            if (Cache.ContainsKey("Capacity"))
            {
                capacity = (string)Cache.Get("Capacity");
                OnPropertyChanged("Capacity");
            }

            if (Cache.ContainsKey("Type"))
            {
                type = (AccommodationType)Cache.Get("Type");
                OnPropertyChanged(nameof(Type));
            }

            if (Cache.ContainsKey("MinReservationDays"))
            {
                minReservationDays = (string)Cache.Get("MinReservationDays");
                OnPropertyChanged("MinReservationDays");
            }

            if (Cache.ContainsKey("IsHutChecked"))
            {
                isHutChecked = (bool)Cache.Get("IsHutChecked");
                OnPropertyChanged(nameof(IsHutChecked));
            }

            if (Cache.ContainsKey("IsApartmentChecked"))
            {
                isApartmentChecked = (bool)Cache.Get("IsApartmentChecked");
                OnPropertyChanged(nameof(IsApartmentChecked));
            }

            if (Cache.ContainsKey("IsHouseChecked"))
            {
                isHouseChecked = (bool)Cache.Get("IsHouseChecked");
                OnPropertyChanged(nameof(IsHouseChecked));
            }
        }

        private void UpdateAutoComplete()
        {
            var suggestion = Locations.FirstOrDefault(loc => loc.StartsWith(SelectedLocation, StringComparison.OrdinalIgnoreCase));
            if (suggestion != null && !suggestion.Equals(SelectedLocation, StringComparison.OrdinalIgnoreCase))
            {
                SelectedLocation = suggestion;
                MoveCursorToEnd?.Invoke(SelectedLocation.Length);
            }
        }

        public event Action<int> MoveCursorToEnd;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
