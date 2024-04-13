using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static BookingApp.Model.AccommodationTypeEnum;


namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Guest.xaml
    /// </summary>
    public partial class Guest : Window
    {

        private AccommodationRepository AccommodationRepository { get; set; }
        public AccommodationDTO? SelectedAccommodation { get; set; }
        public ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        public ObservableCollection<string> Locations { get; set; }
        public ICollectionView FilteredAccommodations { get; set; }
        public User LoggedInUser { get; set; }
        public Guest(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;

            Accommodations = new ObservableCollection<AccommodationDTO>();
            AccommodationRepository = new AccommodationRepository();

            Locations = new ObservableCollection<string>();

            FilteredAccommodations = CollectionViewSource.GetDefaultView(Accommodations);
            FilteredAccommodations.Filter = FilterAccommodations;

            SelectedAccommodation = new AccommodationDTO();

            Update();
            LoggedInUser = loggedInUser;
        }

        private string searchText;
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                if(searchText != value)
                {
                    searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilteredAccommodations.Refresh();
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
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged(nameof(Location));
                    FilteredAccommodations.Refresh();
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
                    FilteredAccommodations.Refresh(); 
                }
            }
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
                if(capacity != value) 
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                    FilteredAccommodations.Refresh();
                }
            }
        }

        private string daysBeforeCancel;
        public string DaysBeforeCancel
        {
            get
            {
                return daysBeforeCancel;
            }
            set
            {
                if(value != daysBeforeCancel)
                {
                    daysBeforeCancel = value;
                    OnPropertyChanged("DaysBeforeCancel");
                    FilteredAccommodations.Refresh();
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
                    OnPropertyChanged(nameof(Type));
                    FilteredAccommodations.Refresh();
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
                    FilteredAccommodations.Refresh();
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
                if (user != value)
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }

        private bool FilterAccommodations(object item)
        {
            if (!(item is AccommodationDTO accommodation))
                return false;

            bool matchesSearchText = string.IsNullOrWhiteSpace(SearchText) || accommodation.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            bool matchesCapacity = string.IsNullOrWhiteSpace(Capacity) || accommodation.Capacity >= int.Parse(Capacity);
            bool matchesDaysBeforeCancel = string.IsNullOrWhiteSpace(DaysBeforeCancel) || accommodation.DaysBeforeCancel >= int.Parse(DaysBeforeCancel);
            bool matchesMinReservationDays = string.IsNullOrWhiteSpace(MinReservationDays) || accommodation.MinReservationDays.ToString().Contains(MinReservationDays, StringComparison.OrdinalIgnoreCase);
            bool matchesLocation = string.IsNullOrWhiteSpace(SelectedLocation) || accommodation.Location.ToString().Equals(SelectedLocation, StringComparison.OrdinalIgnoreCase);
            bool matchesIsCheckedAccomodationType = IsCheckedAccomodationType(accommodation);

           
            return matchesSearchText && matchesLocation && matchesCapacity && matchesDaysBeforeCancel && matchesMinReservationDays && matchesIsCheckedAccomodationType;
        }
        private void CheckBoxOption1_Changed(object sender, RoutedEventArgs e)
        {
            FilteredAccommodations.Refresh();
        }

        bool IsCheckedAccomodationType(AccommodationDTO accommodation)
        {
            bool anyChecked = checkBoxOption1.IsChecked.GetValueOrDefault() ||
                              checkBoxOption2.IsChecked.GetValueOrDefault() ||
                              checkBoxOption3.IsChecked.GetValueOrDefault();

            if (!anyChecked) return true;

            bool matchesTypeApartment = checkBoxOption1.IsChecked.GetValueOrDefault() &&
                                        accommodation.Type == AccommodationType.Apartment;

            bool matchesTypeHut = checkBoxOption2.IsChecked.GetValueOrDefault() &&
                                  accommodation.Type == AccommodationType.Hut;

            bool matchesTypeHouse = checkBoxOption3.IsChecked.GetValueOrDefault() &&
                                    accommodation.Type == AccommodationType.House;

            return matchesTypeApartment || matchesTypeHut || matchesTypeHouse;
        }
        public void Update()
        {
            Accommodations.Clear();
            var allLocations = new HashSet<Location>();
            foreach(Accommodation accommodation in AccommodationRepository.GetAll())
            {
                Accommodations.Add(new AccommodationDTO(accommodation));
                allLocations.Add(accommodation.Location);
            }

            Locations.Clear();
            Locations.Add(" ");
            foreach (Location location in allLocations)
            {
                Locations.Add(location.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnClickButton(object sender, RoutedEventArgs e)
        {
            Reservation reservation = new Reservation(SelectedAccommodation, user);
            reservation.Show();
        }
    }
}