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
        public ObservableCollection<Location> Locations { get; set; }
        public ICollectionView FilteredAccommodations { get; set; }
        public Guest()
        {
            InitializeComponent();
            DataContext = this;

            Accommodations = new ObservableCollection<AccommodationDTO>();
            AccommodationRepository = new AccommodationRepository();

            Locations = new ObservableCollection<Location>();

            FilteredAccommodations = CollectionViewSource.GetDefaultView(Accommodations);
            FilteredAccommodations.Filter = FilterAccommodations;


            Update();
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

        private string selectedLocation;
        public string SelectedLocation
        {
            get 
            { 
                return selectedLocation; 
            }
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

        private bool FilterAccommodations(object item)
        {
            if (!(item is AccommodationDTO accommodation))
                return false;

            bool matchesSearchText = string.IsNullOrWhiteSpace(SearchText) || accommodation.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            bool matchesLocation = string.IsNullOrWhiteSpace(SelectedLocation) || accommodation.Location.City.Equals(SelectedLocation, StringComparison.OrdinalIgnoreCase);
            //proveri metodu iznad


            return matchesSearchText && matchesLocation;
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
            foreach(Location location in allLocations)
            {
                Locations.Add(location);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
