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
using System.Windows;



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
        public ObservableCollection<VehicleDTO> VehicleDTOList { get; set; } = new ObservableCollection<VehicleDTO>();
        public ObservableCollection<Language> Languages { get; set; } = new ObservableCollection<Language>();
        public ObservableCollection<Language> SelectedLanguages { get; set; } = new ObservableCollection<Language>();
        private List<int> selectedLanguageIndexes = new List<int>();
        public ObservableCollection<Language> Languages1 { get; set; } = new ObservableCollection<Language>();
        public ObservableCollection<Language> AllLanguages { get; set; } = new ObservableCollection<Language>();


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

            
            AllLanguages = new ObservableCollection<Language>(_languageRepository.GetAllLanguages());
            int i = 1;
            foreach (Language language in AllLanguages)
            {
                if (i % 2 == 0)
                {
                    Languages1.Add(language); 
                    i++;
                }
                else
                {
                    Languages.Add(language);
                    i++;
                }
            }
        }

        public VehicleRegistrationWindow(VehicleRepository vehicleRepository, ObservableCollection<VehicleDTO> vehicles, DataGrid vehicleGrid)
        {
            InitializeComponent();
            CenterWindowOnScreen();
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
                /*MaxCapacityLabelError.Content = "!";
                MaxCapacityLabelError.Foreground = Brushes.Red;
                MaxCapacityLabelError.FontWeight = FontWeights.Bold;
                */
                MessageBox.Show("Capacity was not entered correctly.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else
            {
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
                    CountryLabelError.Content = "Location added. ";
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
                    CountryLabelError.Content = "Location added. ";
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
            /*CountryLabelError.Content = "Type Country and City first. ";
            CountryLabelError.Foreground = Brushes.Red;
            CountryLabelError.FontSize = 10;*/
            MessageBox.Show("Location is not entered correctly.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        List<Language> languages = new List<Language>();

        private void LanguageError()
        {
            /*
            LanguagesLabelError.Content = "Type language first. ";
            CountryLabelError.Foreground = Brushes.Red;
            LanguagesLabelError.FontSize = 10;
            */
            MessageBox.Show("You have to select at least one language.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private Vehicle CreateVehicle()
        {
            Vehicle vehicle = new Vehicle();

            if( locations.Count == 0)
            {
                LocationError();
                return null;
            }
            var selectedLanguages = LanguagesListBox.SelectedItems.Cast<Language>().ToList();
            foreach (Language language in LanguagesListBox1.SelectedItems.Cast<Language>().ToList())
            {
                selectedLanguages.Add(language);
            }
            if (selectedLanguages.Count == 0)
            {
                // Obavijestite korisnika ako nije odabrao jezik
                LanguageError();
                return null;
            }
            vehicle.Locations = locations;
            vehicle.Languages = selectedLanguages;
            vehicle.DriverId = LoggedInUser.Id;
            vehicle.Capacity = int.Parse(MaxCapacityTextBox.Text);
            vehicle.ImagePaths = ImageList;
            vehicle.User = LoggedInUser;

            return vehicle;
        }

        private void RegisterVehicle(Vehicle vehicle)
        {
 
            _vehicleRepository.Save(vehicle);
            RefreshList();
        }


        internal void RefreshList()
        {
            VehicleDTOList.Clear();

            var vehicles = _vehicleRepository.GetVehiclesByDriver(LoggedInUser);

            foreach (var vehicle in vehicles)
            {
                VehicleDTO vehicleDTO = new VehicleDTO(vehicle);
                VehicleDTOList.Add(vehicleDTO);
            }
        }

        private void btnRegisterVehicle_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateMaxCapacity())
            {
                Vehicle vehicle = CreateVehicle();
                if (vehicle != null)
                {
                    RegisterVehicle(vehicle);
                    ClearWindow();
                    CountryLabelError.Content = "";
                }
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
            Driver.Example example = new Driver.Example(LoggedInUser);
            example.Show();
            Close();
        }
        private void MaxCapacityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            // Proverava da li je uneseni tekst broj
            return int.TryParse(text, out _);
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth - windowWidth) / 2;
            Top = (screenHeight - windowHeight) / 2;
        }

        private void btnEditLocations_Click(object sender, RoutedEventArgs e)
        {

        }

        /*private void LanguageCheckBox_Checked(object sender, RoutedEventArgs e)
{
   var checkBox = sender as CheckBox;
   var language = checkBox.DataContext as Language;

   // Dodajemo odabrani jezik u listu SelectedLanguages
   SelectedLanguages.Add(language);
}

// Metoda koja se poziva kada se odznači jezik
private void LanguageCheckBox_Unchecked(object sender, RoutedEventArgs e)
{
   var checkBox = sender as CheckBox;
   var language = checkBox.DataContext as Language;

   // Uklanjamo odabrani jezik iz liste SelectedLanguages
   SelectedLanguages.Remove(language);
}*/

    }
}
