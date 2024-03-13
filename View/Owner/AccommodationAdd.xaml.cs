using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BookingApp.Model;

namespace BookingApp.View.Owner
{
    public partial class AccommodationAdd : Window, INotifyPropertyChanged
    {
        private AccommodationRepository accommodationRepository;
        private LocationRepository locationRepository;
        public AccommodationDTO accommodationDTO { get; set; }
        ObservableCollection<AccommodationDTO> accommodations;

        public DataGrid AccomodationGrid;
        public AccommodationAdd()
        {
            InitializeComponent();
            DataContext = this;
            accommodationDTO = new AccommodationDTO();
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();  
        }
        public AccommodationAdd(AccommodationRepository accommodationRepository, ObservableCollection<AccommodationDTO> accommodations, DataGrid accomodationGrid)
        {

            InitializeComponent();
            DataContext = this;
            this.accommodationRepository = accommodationRepository;
            this.accommodations = accommodations;
            AccomodationGrid = accomodationGrid;
            locationRepository = new LocationRepository();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void AccommodationAdd1_Click(object sender, RoutedEventArgs e)
        {
            if (Item11.IsSelected == true)
                accommodationDTO.Type = "Apartment";
            if (Item12.IsSelected == true)
                accommodationDTO.Type = "House";
            if (Item13.IsSelected == true)
                accommodationDTO.Type = "Hut";
            string city = accommodationDTO.City;
            string country = accommodationDTO.Country;
            accommodationDTO.Location = new Location(city, country);
            Accommodation accommodation = accommodationDTO.ToAccommodation();
            locationRepository.Save(accommodation.Location);
            accommodationRepository.Save(accommodation);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
