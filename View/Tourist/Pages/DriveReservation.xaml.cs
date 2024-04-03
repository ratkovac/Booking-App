using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Tourist.Pages
{
    public partial class DriveReservation : Page
    {
        public User Tourist;
        public Location SelectedLocation { get; set; }
        //public int DetailedStartAddressId { get; set; }
        //public int DetailedEndAddressId { get; set; }
        public Address DetailedStartAddress { get; set; }
        public Address DetailedEndAddress { get; set; }
        public User SelectedDriver { get; set; }
        public int AddressId { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }

        public LocationRepository locationRepository { get; set; }
        public AddressRepository addressRepository { get; set; }

        public DriveReservation(User user)
        {
            InitializeComponent();
            Tourist = user;
            InputCountries(CountryComboBox);
            locationRepository = new LocationRepository();
            addressRepository = new AddressRepository();
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
                    MessageBox.Show("Neispravan format unosa. Molimo unesite ulicu i broj odvojene zarezom.");
                    return;
                }

                string streetName = parts[0].Trim();
                string streetNumber = parts[1].Trim();

                var streets = addressRepository.GetAll()
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
                SelectedLocation = locationRepository.GetLocationByCityAndCountry(CityName, CountryName);
            }
        }


        private void SetDetailedAddressId(TextBox streetTextBox, bool isStartAddress)
        {
            string input = streetTextBox.Text.Trim();

            string[] parts = input.Split(',');

            string streetName = parts[0].Trim();
            string streetNumber = parts[1].Trim();

            var address = addressRepository.GetByAddress(streetName, streetNumber);
            if (address != null)
            {
                AddressId = address.Id;
                if (isStartAddress) DetailedStartAddress = addressRepository.GetAddressById(AddressId);
                else DetailedEndAddress = addressRepository.GetAddressById(AddressId);
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

                // Formatiramo datum i vrijeme u željeni format
                string formattedDateTime = $"{dateDp.SelectedDate.Value.Day}.{dateDp.SelectedDate.Value.Month}.{dateDp.SelectedDate.Value.Year}. {hour.ToString("00")}:{minute.ToString("00")}:00";

                // Parsiramo formatirani string u DateTime objekt
                DateTime resultDateTime;
                if (DateTime.TryParseExact(formattedDateTime, "d.M.yyyy. HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out resultDateTime))
                {
                    return resultDateTime;
                }
                else
                {
                    MessageBox.Show("Nije moguće pretvoriti datum i vrijeme.");
                    return DateTime.MinValue;
                }
            }
            else
            {
                MessageBox.Show("Molimo unesite validan datum i sat.");
                return DateTime.MinValue;
            }
        }

        private void UpdateDriverList()
        {
            if (!AreAllCriteriaMet())
            {
                MessageBox.Show("Molimo popunite sva polja.");
                return;
            }

            VehicleRepository vehicleRepository = new VehicleRepository();
            List<int> drivers = vehicleRepository.GetDriverIdsByLocationId(SelectedLocation.Id);
            DateTime? date = CreateDateTimeFromSelections();
            drivers = FilterDrivers(drivers, date);
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

            SetDetailedAddressId(startStreetTextBox, true);
            SetDetailedAddressId(endStreetTextBox, false);
            if (DetailedStartAddress.Id == 0)
            {
                DetailedStartAddress = AddNewAddress(startStreetTextBox.Text.Trim());
            }

            if (DetailedEndAddress.Id == 0)
            {
                DetailedEndAddress = AddNewAddress(endStreetTextBox.Text.Trim());
            }

            Drive drive = new(DetailedStartAddress, DetailedEndAddress, departure, SelectedDriver, Tourist, 2, 0);
            DriveRepository driveRepository = new DriveRepository();
            driveRepository.Save(drive);
            MessageBox.Show("Rezervacija uspješna");
        }

        private Address AddNewAddress(string address)
        {
            string[] parts = address.Split(',');

            if (parts.Length != 2)
            {
                MessageBox.Show("Neispravan format unosa. Molimo unesite ulicu i broj odvojene zarezom.");
                return null;
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

            return newAddress;
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
