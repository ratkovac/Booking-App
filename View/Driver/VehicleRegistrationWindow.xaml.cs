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


        public VehicleRegistrationWindow(User user)
        {
            LoggedInUser = user;
            InitializeComponent();
            _vehicleRepository = new VehicleRepository();
            _locationRepository = new LocationRepository();
            vehicleDTO = new VehicleDTO();
            _languageRepository = new LanguageRepository();

            Language = _languageRepository.GetAllLanguages();
            foreach (Language language in Language)
            {
                LanguagesComboBox.Items.Add(language.Name);
            }
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

        private bool ValidateCity()
        {
            if (string.IsNullOrWhiteSpace(CityTextBox.Text))
            {
                CityLabelError.Content = "Niste uneli grad";
                CityLabelError.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                CityLabelError.Content = "";
                return true;
            }
        }

        private bool ValidateCountry()
        {
            if (string.IsNullOrWhiteSpace(CountryTextBox.Text))
            {
                CountryLabelError.Content = "Niste uneli zemlju";
                CountryLabelError.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                CountryLabelError.Content = "";
                return true;
            }
        }

        private bool ValidateMaxCapacity()
        {
            if (string.IsNullOrWhiteSpace(MaxCapacityTextBox.Text))
            {
                MaxCapacityLabelError.Content = "Niste uneli Maksimalni Kapacitet";
                MaxCapacityLabelError.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                MaxCapacityLabelError.Content = "";
                return true;
            }
        }

        private bool ValidationForm()
        {
            bool isValid = true;

            isValid &= ValidateCity();
            isValid &= ValidateCountry();
            isValid &= ValidateMaxCapacity();

            return isValid;
        }

        private Vehicle CreateVehicle()
        {
            Vehicle vehicle = new Vehicle();

            string languageName = LanguagesComboBox.SelectedItem.ToString();
            vehicle.Language = _languageRepository.GetLanguageByName(languageName);

            string city = CityTextBox.Text;
            string country = CountryTextBox.Text;
            Location location = new Location(city, country);
            _locationRepository.Save(location);
            vehicle.Location = location;

            vehicle.Capacity = int.Parse(MaxCapacityTextBox.Text);
            vehicle.ImagePaths = ImageList;
            vehicle.User = LoggedInUser;

            return vehicle;
        }

        private void RegisterVehicle(Vehicle vehicle)
        {
            _vehicleRepository.Save(vehicle);
            Window.GetWindow(this)?.Close();
            driverFrontPage.Show();
        }

        private void btnRegisterVehicle_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationForm())
            {
                Vehicle vehicle = CreateVehicle();
                RegisterVehicle(vehicle);
            }
        }
    }
}
