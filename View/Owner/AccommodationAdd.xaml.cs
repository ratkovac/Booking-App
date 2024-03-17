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
using BookingApp.View;
using System.Reflection.Metadata;

namespace BookingApp.View.Owner
{
    public partial class AccommodationAdd : Window, INotifyPropertyChanged
    {
        private AccommodationRepository accommodationRepository;
        private LocationRepository locationRepository;
        private ImageRepository imageRepository;
        private UserRepository userRepository;
        public User LoggedInUser { get; set; }
        public AccommodationDTO accommodationDTO { get; set; }
        ObservableCollection<AccommodationDTO> accommodations;
        List<string> pathImage = new List<string>();

        public DataGrid AccomodationGrid;
        public AccommodationAdd(User user)
        {
            InitializeComponent();
            DataContext = this;
            accommodationDTO = new AccommodationDTO();
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
            imageRepository = new ImageRepository();
            userRepository = new UserRepository();
            LoggedInUser = user;
        }
        public AccommodationAdd(AccommodationRepository accommodationRepository, ObservableCollection<AccommodationDTO> accommodations, DataGrid accomodationGrid)
        {

            InitializeComponent();
            DataContext = this;
            this.accommodationRepository = accommodationRepository;
            this.accommodations = accommodations;
            AccomodationGrid = accomodationGrid;
            locationRepository = new LocationRepository();
            imageRepository = new ImageRepository();
            userRepository = new UserRepository();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            pathImage.Add(accommodationDTO.ImagePath);
            MessageBox.Show("Unesite jos slika ako zelite!");
            path.Text = "";
            //imagePaths.Items.Add(pathImage).ToString();
        }
        
        public void SetAccommodationType()
        {
            if (Item11.IsSelected == true)
                accommodationDTO.Type = AccommodationTypeEnum.AccommodationType.Apartment;
            if (Item12.IsSelected == true)
                accommodationDTO.Type = AccommodationTypeEnum.AccommodationType.House;
            if (Item13.IsSelected == true)
                accommodationDTO.Type = AccommodationTypeEnum.AccommodationType.Hut;
        }

        /*private void ImagesAdding()
        {
            int tourId = -1;
            foreach (string path in pathImage)
            {
                imageRepository.Save(new Model.Image(path, accommodation.Id, tourId));
            }
            pathImage.Clear();
        }*/

        private void SetNewLocation()
        {
            string city = accommodationDTO.City;
            string country = accommodationDTO.Country;
            accommodationDTO.Location = new Location(city, country);
        }
        private void AccommodationAdding_Click(object sender, RoutedEventArgs e)
        {  
            SetAccommodationType();

            SetNewLocation();
            
            accommodationDTO.User = LoggedInUser;
            Accommodation accommodation = accommodationDTO.ToAccommodation();
            locationRepository.Save(accommodation.Location);
            accommodationRepository.Save(accommodation);

            int tourId = -1;
            foreach (string path in pathImage)
            {
                imageRepository.Save(new Model.Image(path, accommodation.Id, tourId));
            }
            pathImage.Clear();
            //kandidat za novu funkciju

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            pathImage.Clear();
        }

    }
}
