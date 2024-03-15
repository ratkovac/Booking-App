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
        public  string Capacity
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

        private bool FilterAccommodations(object item)
        {
            if (!(item is AccommodationDTO accommodation))
                return false;

            bool matchesSearchText = string.IsNullOrWhiteSpace(SearchText) || accommodation.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
<<<<<<< HEAD:View/Guest/Guest.xaml.cs
            bool matchesCapacity = string.IsNullOrWhiteSpace(Capacity) || accommodation.Capacity.ToString().Contains(Capacity, StringComparison.OrdinalIgnoreCase);
            bool matchesDaysBeforeCancel = string.IsNullOrWhiteSpace(DaysBeforeCancel) || accommodation.DaysBeforeCancel.ToString().Contains(DaysBeforeCancel, StringComparison.OrdinalIgnoreCase);
            bool matchesMinReservationDays = string.IsNullOrWhiteSpace(MinReservationDays) || accommodation.MinReservationDays.ToString().Contains(MinReservationDays, StringComparison.OrdinalIgnoreCase);
            bool matchesLocation = string.IsNullOrWhiteSpace(SelectedLocation) || accommodation.Location.ToString().Equals(SelectedLocation, StringComparison.OrdinalIgnoreCase);
=======
            //bool matchesLocation = string.IsNullOrWhiteSpace(SelectedLocation) || accommodation.Location.Equals(SelectedLocation, StringComparison.OrdinalIgnoreCase);
            bool matchesCapacity = (Capacity == 0 || accommodation.Capacity == Capacity);
            bool matchesDaysBeforeCancel = (DaysBeforeCancel == 0 || accommodation.DaysBeforeCancel == DaysBeforeCancel);
            bool matchesMinReservationDays = (MinReservationDays == 0 || accommodation.MinReservationDays == MinReservationDays); 
            bool matchesLocation = string.IsNullOrWhiteSpace(SelectedLocation) || accommodation.Location.City.Equals(SelectedLocation, StringComparison.OrdinalIgnoreCase);
            //proveri metodu iznad
>>>>>>> dfb4d4ae8cba5d34c4990390381d7828e48b784e:View/Guest.xaml.cs

            return matchesSearchText && matchesLocation && matchesCapacity && matchesDaysBeforeCancel && matchesMinReservationDays;
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
    }
}
