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
using System.Windows.Media.Imaging;
using System.Windows.Input;

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
        private List<string> PathImages = new List<string>();
        private List<string> Cities = new List<string>();
        private List<Location> Locations = new List<Location>();

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
            Locations = locationRepository.GetAll();
            getAllCities();
        }
        public AccommodationAdd(AccommodationRepository accommodationRepository, ObservableCollection<AccommodationDTO> accommodations, DataGrid accomodationGrid)
        {

            InitializeComponent();
            DataContext = this;
            this.accommodationRepository = accommodationRepository;
            AccomodationGrid = accomodationGrid;
            locationRepository = new LocationRepository();
            imageRepository = new ImageRepository();
            userRepository = new UserRepository();
            getAllCities();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void getAllCities()
        { 
            foreach(var location in Locations)
            {
                Cities.Add(location.City);
            }
        }
        private void SaveImages(List<string> pathImages, int accommodationId)
        {
            foreach (string pathImage in pathImages)
            {
                Model.Image image = new Model.Image(pathImage, accommodationId, -1);
                imageRepository.Create(image);
            }
        }
        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Multiselect = true;

            bool? response = ofd.ShowDialog();

            if (response == true)
            {
                foreach (var item in ofd.FileNames)
                {
                    PathImages.Add(item);
                }
            }
            showImages();
        }
        private void showImages()
        {
            WrapPanel imageWrapPanel = FindName("imageeWrapPanel") as WrapPanel;
            imageWrapPanel.Children.Clear();

            foreach (var imagePath in PathImages)
            {
                System.Windows.Controls.Image newImage = new System.Windows.Controls.Image();
                newImage.Source = new BitmapImage(new Uri(imagePath));
                newImage.Height = 50;
                newImage.Margin = new Thickness(5);
                newImage.MouseEnter += (sender, e) => HighlightImage(sender, e, true);
                newImage.MouseLeave += (sender, e) => HighlightImage(sender, e, false);
                newImage.MouseDown += (sender, e) => RemoveImageOnClick(sender, e, imagePath);
                imageWrapPanel.Children.Add(newImage);
            }
        }
        private void HighlightImage(object sender, MouseEventArgs e, bool highlight)
        {
            if (sender is System.Windows.Controls.Image image)
            {
                if (highlight)
                {
                    image.Effect = new System.Windows.Media.Effects.DropShadowEffect
                    {
                        Color = System.Windows.Media.Colors.Black,
                        Direction = 0,
                        ShadowDepth = 0,
                        Opacity = 0.5,
                        BlurRadius = 10
                    };
                }
                else
                {
                    // Uklanjanje efekta osenčavanja
                    image.Effect = null;
                }
            }
        }
        private void RemoveImageOnClick(object sender, MouseButtonEventArgs e, string imagePath)
        {
            if (sender is System.Windows.Controls.Image clickedImage)
            {
                PathImages.Remove(imagePath);
                showImages();
            }
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
            if(ExsitingLocation())
            {
                accommodation.Location = locationRepository.GetLocationByCityAndCountry(City.Text, Country.Text);
            }
            else
            {
                locationRepository.Save(accommodation.Location);
            }
            accommodationRepository.Save(accommodation);
            SaveImages(PathImages, accommodation.Id);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
  //          pathImage.Clear();
        }

        private bool ExsitingLocation()
        {
            Location? location = locationRepository.GetLocationByCityAndCountry(City.Text, Country.Text);
            if(location == null)
                return false;
            else
                return true;
        }

        private void City_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (City.SelectedItem != null)
            {
                string selectedCity = City.SelectedItem.ToString();
                City.Text = selectedCity;
                Country.Text = locationRepository.GetLocationByCity(selectedCity).Country;
            }
        }
        private List<string> SuggestedCities(string typedCity)
        {
            List<string> suggestedCities = Cities.Where(city => city.StartsWith(typedCity, StringComparison.OrdinalIgnoreCase)).ToList();
            return suggestedCities;
        }
        

        private void City_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string typedCity = City.Text;
            List<string> suggestedCities = SuggestedCities(typedCity);

            if (suggestedCities.Any())
            {
                City.ItemsSource = suggestedCities;
            }
        }

        
    }
}
