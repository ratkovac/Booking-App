using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Serializer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace BookingApp.View.Driver
{

    public partial class VehicleRegistrationWindow : Window
    {
        private readonly VehicleRepository _vehicleRepository;

        private readonly LocationRepository _locationRepository;

        private readonly Serializer<Vehicle> _serializer;

        public VehicleDTO vehicleDTO { get; set; }

        ObservableCollection<VehicleDTO> vehicles;

        private readonly User LoggedInUser;

        public DataGrid VehicleGrid;

        public List<Language> Language;

        public LanguageRepository _languageRepository;

        public DriverFrontPage driverFrontPage;
        public ObservableCollection<VehicleDTO> VehicleDTOList { get; set; } = new ObservableCollection<VehicleDTO>();


        public VehicleRegistrationWindow(User user)
        {
            LoggedInUser = user;
            InitializeComponent();
            DataContext = this;

            _vehicleRepository = new VehicleRepository();
            _locationRepository = new LocationRepository();
            vehicleDTO = new VehicleDTO();
            _languageRepository = new LanguageRepository();
            Window_Loaded(this, null);


            driverFrontPage = new DriverFrontPage(user);

        }

        public VehicleRegistrationWindow(VehicleRepository vehicleRepository, ObservableCollection<VehicleDTO> vehicles, DataGrid vehicleGrid)
        {
            InitializeComponent();
            DataContext = this;
            this._vehicleRepository = vehicleRepository;
            this.vehicles = vehicles;
            VehicleGrid = vehicleGrid;
            _locationRepository = new LocationRepository();
            Window_Loaded(this, null);

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VehicleDTOList.Clear();

            var vehicles = _vehicleRepository.GetVehiclesByDriver(LoggedInUser);

            foreach (var vehicle in vehicles)
            {
                VehicleDTO vehicleDTO = new VehicleDTO(vehicle);
                VehicleDTOList.Add(vehicleDTO);
            }
        }

        List<string> ImageList = new List<string>();

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            bool? response = dlg.ShowDialog();

            if (response == true)
            {
                foreach (string s in dlg.FileNames)
                {
                    ImageList.Add(s);
                }
            }

        }

        private bool ValidateMaxCapacity()
        {
            if (string.IsNullOrWhiteSpace(MaxCapacityTextBox.Text) && !CanParseToInt(MaxCapacityTextBox.Text))
            {
                MaxCapacityLabelError.Content = "!";
                MaxCapacityLabelError.Foreground = Brushes.Red;
                MaxCapacityLabelError.FontWeight = FontWeights.Bold;
                return false;
            }
            else
            {
                MaxCapacityLabelError.Content = "";
                return true;
            }
        }
        public static bool CanParseToInt(string text)
        {
            int result;
            return int.TryParse(text, out result);
        }


        List<Location> locations = new List<Location>();

        private void AddLocation_Click(object sender, RoutedEventArgs e)
        {
            string city = CityTextBox.Text;
            string country = CountryTextBox.Text;

            if (!string.IsNullOrWhiteSpace(city) && !string.IsNullOrWhiteSpace(country))
            {
                int locationId = _locationRepository.ExistsLocation(city, country);

                if (locationId != 0)
                {
                    Location location = new Location { Id = locationId, City = city, Country = country };
                    locations.Add(location);

                    CityTextBox.Text = "";
                    CountryTextBox.Text = "";
                    CountryLabelError.Content = "Location added successfully. ";
                    CountryLabelError.Foreground = Brushes.Black;
                    CountryLabelError.FontSize = 10;
                }
                else
                {
                    Location newLocation = new Location { City = city, Country = country };
                    _locationRepository.Save(newLocation); 
                    locations.Add(newLocation); 

                    CityTextBox.Text = "";
                    CountryTextBox.Text = "";
                    CountryLabelError.Content = "Location added successfully. ";
                    CountryLabelError.Foreground = Brushes.Black;
                    CountryLabelError.FontSize = 10;
                }
            }
            else
            {
                LocationError();
            }
        }

        private void LocationError()
        {
            CountryLabelError.Content = "Type Country and City first. ";
            CountryLabelError.Foreground = Brushes.Red;
            CountryLabelError.FontSize = 10;
        }

        List<Language> languages = new List<Language>();
        private void AddLanguage_Click(object sender, RoutedEventArgs e)
        {
            string languageName = LanguagesTextBox.Text;

            if (!string.IsNullOrWhiteSpace(languageName))
            {
                int languageId = _languageRepository.ExistsLanguage(languageName);
                
                if (languageId != 0)
                {
                    Language language = new Language(languageId, languageName);
                    languages.Add(language);
                    LanguagesTextBox.Text = "";
                    LanguagesLabelError.Content = "Language added successfully. ";
                    LanguagesLabelError.Foreground = Brushes.Black;
                    LanguagesLabelError.FontSize = 10;
                }
                else
                {
                    LanguagesLabelError.Content = "Language does not exist. Try again. ";
                    LanguagesLabelError.Foreground = Brushes.Red;
                    LanguagesLabelError.FontSize = 10;
                }
            }
            else
            {
                LanguagesLabelError.Content = "Type language first. ";
                CountryLabelError.Foreground = Brushes.Red;
                LanguagesLabelError.FontSize = 10;
            }
        }
        private void LanguageError()
        {
            LanguagesLabelError.Content = "Type language first. ";
            CountryLabelError.Foreground = Brushes.Red;
            LanguagesLabelError.FontSize = 10;
        }
    
        private Vehicle CreateVehicle()
        {
            Vehicle vehicle = new Vehicle();

            if (languages.Count == 0)
            {
                LanguageError();
                return null;
            }
            if( locations.Count == 0)
            {
                LocationError();
                return null;
            }
            vehicle.Languages = languages;
            vehicle.Locations = locations;
            vehicle.DriverId = LoggedInUser.Id;
            vehicle.Capacity = int.Parse(MaxCapacityTextBox.Text);
            vehicle.ImagePaths = ImageList;
            vehicle.User = LoggedInUser;

            return vehicle;
        }

        private void RegisterVehicle(Vehicle vehicle)
        {
            _vehicleRepository.Save(vehicle);
        }

        private void btnRegisterVehicle_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateMaxCapacity())
            {
                Vehicle vehicle = CreateVehicle();
                RegisterVehicle(vehicle);
                ClearWindow();
            }
        }

        private void ClearWindow()
        {
            MaxCapacityTextBox.Text = "";
            MessageBox.Show("Vehicle successfully registered. ");
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
            driverFrontPage.Show();
        }

    }
}
