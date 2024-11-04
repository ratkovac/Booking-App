using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class DriveReservationView : Page, INotifyPropertyChanged
    {
        public User Tourist;
        public Location SelectedLocation { get; set; }
        public int DetailedStartAddressId { get; set; }
        public int DetailedEndAddressId { get; set; }
        public User SelectedDriver { get; set; }
        public int AddressId { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }

        public LocationRepository _locationRepository { get; set; }
        public AddressRepository _addressRepository { get; set; }

        public DriveReservationView(User user)
        {
            InitializeComponent();
            Tourist = user;
            InputCountries(CountryComboBox);
            _locationRepository = new LocationRepository();
            _addressRepository = new AddressRepository();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CountryComboBox.SelectedItem = null;
            CityComboBox.SelectedItem = null;
            minuteComboBox.SelectedItem = null;
            driversComboBox.SelectedItem = null;

            startStreetTextBox.Text = string.Empty;
            endStreetTextBox.Text = string.Empty;
            hourTextBox.Text = string.Empty;

            dateDp.SelectedDate = null;

            SelectedLocation = null;
            DetailedStartAddressId = 0;
            DetailedEndAddressId = 0;
            SelectedDriver = null;
            AddressId = 0;
            CountryName = string.Empty;
            CityName = string.Empty;

            OnPropertyChanged(nameof(SelectedLocation));
            OnPropertyChanged(nameof(DetailedStartAddressId));
            OnPropertyChanged(nameof(DetailedEndAddressId));
            OnPropertyChanged(nameof(SelectedDriver));
            OnPropertyChanged(nameof(AddressId));
            OnPropertyChanged(nameof(CountryName));
            OnPropertyChanged(nameof(CityName));
        }

        private void InputCountries(ComboBox comboBox)
        {
            var countries = GetDistinctCountries();
            UpdateComboBoxItems(comboBox, countries);
        }

        private List<string> GetDistinctCountries()
        {
            return new LocationRepository().GetAll()
                                            .Select(loc => loc.Country)
                                            .Distinct()
                                            .OrderBy(c => c)
                                            .ToList();
        }

        private void UpdateComboBoxItems(ComboBox comboBox, List<string> items)
        {
            comboBox.ItemsSource = items;
        }

        private void InputCities(ComboBox countryComboBox, ComboBox cityComboBox)
        {
            string selectedCountry = countryComboBox.SelectedItem?.ToString();
            CountryName = selectedCountry;

            if (string.IsNullOrEmpty(selectedCountry))
            {
                ClearComboBox(cityComboBox);
                return;
            }

            var cities = GetCitiesByCountry(selectedCountry);

            if (cities.Count > 0)
            {
                UpdateCityComboBox(cityComboBox, cities);
            }
            else
            {
                ClearComboBox(cityComboBox);
            }
        }

        private List<KeyValuePair<int, string>> GetCitiesByCountry(string country)
        {
            return new LocationRepository().GetAll()
                                            .Where(location => location.Country == country)
                                            .Select(location => new KeyValuePair<int, string>(location.Id, location.City))
                                            .Distinct()
                                            .OrderBy(pair => pair.Value)
                                            .ToList();
        }

        private void UpdateCityComboBox(ComboBox cityComboBox, List<KeyValuePair<int, string>> items)
        {
            cityComboBox.ItemsSource = items;
            cityComboBox.DisplayMemberPath = "Value";
            cityComboBox.SelectedValuePath = "Key";
        }

        private void ClearComboBox(ComboBox comboBox)
        {
            comboBox.ItemsSource = null;
        }

        private void InputAddress(ComboBox cityComboBox, TextBox streetTextBox)
        {
            if (cityComboBox.SelectedItem != null)
            {
                var selectedCity = (KeyValuePair<int, string>)cityComboBox.SelectedItem;
                CityName = selectedCity.Value;

                string input = streetTextBox.Text.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    streetTextBox.Text = "";
                    return;
                }

                string[] parts = input.Split(',');
                if (parts.Length != 2)
                {
                    if (App.CurrentLanguage == "en-US")
                    {
                        MessageBox.Show("Invalid input format. Please enter the street and number separated by a comma.");
                    }
                    else
                    {
                        MessageBox.Show("Neispravan format unosa. Molimo unesite ulicu i broj odvojene zarezom.");
                    }
                    return;
                }

                string streetName = parts[0].Trim();
                string streetNumber = parts[1].Trim();

                var streets = _addressRepository.GetAll()
                    .Where(address => address.LocationId == selectedCity.Key && address.Street.Equals(streetName, StringComparison.OrdinalIgnoreCase) && address.Number.Equals(streetNumber, StringComparison.OrdinalIgnoreCase))
                    .Select(address => address.Street)
                    .Distinct()
                    .OrderBy(street => street)
                    .ToList();

                streetTextBox.Text = string.Join(Environment.NewLine, streets);
            }
            else
            {
                streetTextBox.Text = "";
            }
        }

        private void InputAddressForCity(ComboBox cityComboBox, TextBox streetTextBox)
        {
            InputAddress(cityComboBox, streetTextBox);
            if (cityComboBox.SelectedItem is KeyValuePair<int, string> selectedCity)
            {
                SelectedLocation = _locationRepository.GetLocationByCityAndCountry(CityName, CountryName);
            }
        }


        private void SetDetailedAddressId(TextBox streetTextBox, bool isStartAddress)
        {
            string input = streetTextBox.Text.Trim();

            string[] parts = input.Split(',');

            string streetName = parts[0].Trim();
            string streetNumber = parts[1].Trim();

            AddressRepository addressRepository = new AddressRepository();
            var address = addressRepository.GetByAddress(streetName, streetNumber);
            if (address != null)
            {
                if (isStartAddress) DetailedStartAddressId = address.Id;
                else DetailedEndAddressId = address.Id;
            }
        }

        private DateTime CreateDateTimeFromSelections()
        {
            if (dateDp.SelectedDate.HasValue &&
                !string.IsNullOrEmpty(hourTextBox.Text) &&
                int.TryParse(hourTextBox.Text, out int hour) &&
                hour >= 0 && hour < 24 &&
                minuteComboBox.SelectedItem is ComboBoxItem selectedMinuteItem)
            {
                int minute = int.Parse(selectedMinuteItem.Content.ToString());

                string formattedDateTime = $"{dateDp.SelectedDate.Value.Day}.{dateDp.SelectedDate.Value.Month}.{dateDp.SelectedDate.Value.Year}. {hour.ToString("00")}:{minute.ToString("00")}:00";

                DateTime resultDateTime;
                if (DateTime.TryParseExact(formattedDateTime, "d.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDateTime))
                {
                    return resultDateTime;
                }
                else
                {
                    if (App.CurrentLanguage == "en-US")
                    {
                        MessageBox.Show("It is not possible to convert the date and time.");
                    }
                    else
                    {
                        MessageBox.Show("Nije moguće pretvoriti datum i vrijeme.");
                    }
                    return DateTime.MinValue;
                }
            }
            else
            {
                /*if (App.CurrentLanguage == "en-US")
                {
                    MessageBox.Show("Please enter a valid date and time.");
                }
                else
                {
                    MessageBox.Show("Molimo unesite validan datum i sat.");
                }*/
                return DateTime.MinValue;
            }
        }

        private void UpdateDriverList()
        {
            VehicleRepository vehicleRepository = new VehicleRepository();
            List<int> drivers = vehicleRepository.GetAllDriverIds();
            /*DateTime? date = CreateDateTimeFromSelections();
            drivers = FilterDrivers(drivers, date);*/
            InputDriverComboBox(drivers);
        }

        private void InputDriverComboBox(List<int> driverIds)
        {
            UserRepository userRepository = new UserRepository();
            List<User> drivers = userRepository.GetByIDs(driverIds);

            driversComboBox.Items.Clear();

            foreach (var driver in drivers)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = driver.Username,
                    Tag = driver
                };
                driversComboBox.Items.Add(item);
            }
        }

        private void driversComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (driversComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                SelectedDriver = (User)selectedItem.Tag;
            }
        }

        private List<int> FilterDrivers(List<int> driverIds, DateTime? targetStartTime)
        {
            DriveRepository driveRepository = new DriveRepository();
            var scheduledDrivers = driveRepository.GetAll()
                                    .Where(reservation => reservation.Date == targetStartTime && driverIds.Contains(reservation.DriverId))
                                    .Select(reservation => reservation.DriverId)
                                    .Distinct()
                                    .ToList();

            return driverIds.Except(scheduledDrivers).ToList();
        }


        private bool AreAllCriteriaMet()
        {
            return dateDp.SelectedDate != null &&
                   CountryComboBox.SelectedItem != null &&
                   CityComboBox.SelectedItem != null &&
                   startStreetTextBox != null &&
                   endStreetTextBox != null &&
                   minuteComboBox.SelectedItem != null;
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            DateTime departure = CreateDateTimeFromSelections();

            if (!AreAllCriteriaMet())
            {
                if (App.CurrentLanguage == "en-US")
                {
                    MessageBox.Show("Please fill in all fields.");
                }
                else
                {
                    MessageBox.Show("Molimo popunite sva polja.");
                }
                return;
            }

            if (!IsValidStreetFormat(startStreetTextBox.Text) || !IsValidStreetFormat(endStreetTextBox.Text))
            {
                string errorMessage;
                if (App.CurrentLanguage == "en-US")
                {
                    errorMessage = "Invalid street format! Correct format is 'Name, number'";
                }
                else
                {
                    errorMessage = "Pogrešan format ulice! Ispravno je 'Naziv, broj'";
                }
                if (!IsValidStreetFormat(startStreetTextBox.Text) && !IsValidStreetFormat(endStreetTextBox.Text))
                {
                    startStreetTextBox.Text = string.Empty;
                    endStreetTextBox.Text = string.Empty;
                }
                else if (!IsValidStreetFormat(startStreetTextBox.Text))
                {
                    startStreetTextBox.Text = string.Empty;
                }
                else if (!IsValidStreetFormat(endStreetTextBox.Text))
                {
                    endStreetTextBox.Text = string.Empty;
                }
                MessageBox.Show(errorMessage);
                return;
            }

            if (departure == DateTime.MinValue)
            {
                string errorMessage;
                if (App.CurrentLanguage == "en-US")
                {
                    errorMessage = "Invalid hours.";
                }
                else
                {
                    errorMessage = "Nevažeći sati.";
                }

                hourTextBox.Text = string.Empty;
                MessageBox.Show(errorMessage);
                return;
            }

            SetDetailedAddressId(startStreetTextBox, true);
            SetDetailedAddressId(endStreetTextBox, false);
            if (DetailedStartAddressId == 0)
            {
                DetailedStartAddressId = AddNewAddress(startStreetTextBox.Text.Trim());
            }

            if (DetailedEndAddressId == 0)
            {
                DetailedEndAddressId = AddNewAddress(endStreetTextBox.Text.Trim());
            }

            Drive drive = new(DetailedStartAddressId, DetailedEndAddressId, departure, SelectedDriver, Tourist, 2, 0, 0);
            DriveRepository driveRepository = new DriveRepository();
            driveRepository.Save(drive);
            if (App.CurrentLanguage == "en-US")
            {
                MessageBox.Show("Reservation successful!");
            }
            else
            {
                MessageBox.Show("Rezervacija uspješna!");
            }
        }

        private bool IsValidStreetFormat(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                return false;
            }
            string[] parts = street.Split(',');

            if (parts.Length != 2)
            {
                return false;
            }

            if (int.TryParse(parts[0].Trim(), out _))
            {
                return false;
            }

            if (!int.TryParse(parts[1].Trim(), out _))
            {
                return false;
            }

            return true;
        }

        private int AddNewAddress(string address)
        {
            string[] parts = address.Split(',');

            if (parts.Length != 2)
            {
                if (App.CurrentLanguage == "en-US")
                {
                    MessageBox.Show("Invalid input format. Please enter the street and number separated by a comma.");
                }
                else
                {
                    MessageBox.Show("Neispravan format unosa. Molimo unesite ulicu i broj odvojene zarezom.");
                }
                return 0;
            }

            string streetName = parts[0].Trim();
            string streetNumber = parts[1].Trim();

            AddressRepository addressRepository = new AddressRepository();

            Address newAddress = new Address
            {
                Id = addressRepository.NextId(),
                Location = SelectedLocation,
                Street = streetName,
                Number = streetNumber
            };

            addressRepository.Save(newAddress);

            return newAddress.Id;
        }

        private void minuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDriverList();
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputCities(CountryComboBox, CityComboBox);
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputAddressForCity(CityComboBox, startStreetTextBox);
        }
    }
}
