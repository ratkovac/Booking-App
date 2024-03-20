using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for DriveReservation.xaml
    /// </summary>
    public partial class DriveReservation : Page
    {
        public User Tourist;
        public int StartAddressId { get; set; }
        public int DetailedStartAddressId { get; set; }
        public int EndAddressId { get; set; }
        public int DetailedEndAddressId { get; set; }
        public int SelectedDriverId { get; set; }

        public DriveReservation(User user)
        {
            InitializeComponent();
            Tourist = user;
            InputCountries(startCountryComboBox);
            InputCountries(endCountryComboBox);
            InputTime();
        }

        private void InputCountries(ComboBox comboBox)
        {
            var countries = GetDistinctCountries();
            UpdateComboBoxItems(comboBox, countries);
        }

        private List<string> GetDistinctCountries()
        {
            LocationRepository locationRepository = new LocationRepository();
            List<Location> locations = locationRepository.GetAll();

            return locations
                .Select(loc => loc.Country)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        private void UpdateComboBoxItems(ComboBox comboBox, List<string> items)
        {
            comboBox.Items.Clear();
            foreach (var item in items)
            {
                comboBox.Items.Add(item);
            }
        }

        private void InputCities(ComboBox countryComboBox, ComboBox cityComboBox)
        {
            string selectedCountry = countryComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedCountry))
            {
                ClearCityComboBox(cityComboBox);
                return;
            }

            var cities = GetCitiesByCountry(selectedCountry);

            if (cities.Any())
            {
                UpdateCityComboBox(cityComboBox, cities);
            }
            else
            {
                ClearCityComboBox(cityComboBox);
            }
        }

        private List<KeyValuePair<int, string>> GetCitiesByCountry(string country)
        {
            LocationRepository locationRepository = new LocationRepository();

            return locationRepository.GetAll()
                .Where(location => location.Country == country)
                .Select(location => new KeyValuePair<int, string>(location.Id, location.City))
                .Distinct()
                .OrderBy(pair => pair.Value)
                .ToList();
        }

        private void UpdateCityComboBox(ComboBox cityComboBox, List<KeyValuePair<int, string>> cities)
        {
            cityComboBox.ItemsSource = cities;
            cityComboBox.DisplayMemberPath = "Value";
            cityComboBox.SelectedValuePath = "Key";
        }

        private void ClearCityComboBox(ComboBox cityComboBox)
        {
            cityComboBox.ItemsSource = null;
        }

        private void InputAddress(ComboBox cityComboBox, ComboBox streetComboBox, ComboBox numberComboBox)
        {
            if (cityComboBox.SelectedItem != null)
            {
                var selectedCity = (KeyValuePair<int, string>)cityComboBox.SelectedItem;
                AddressRepository _addressRepository = new AddressRepository();

                var streets = _addressRepository.GetAll()
                    .Where(address => address.LocationId == selectedCity.Key)
                    .Select(address => address.Street)
                    .Distinct()
                    .OrderBy(street => street)
                    .ToList();

                streetComboBox.ItemsSource = streets;

                if (streetComboBox.SelectedItem != null)
                {
                    var selectedStreet = streetComboBox.SelectedItem.ToString();

                    var numbers = _addressRepository.GetAll()
                        .Where(address => address.LocationId == selectedCity.Key && address.Street == selectedStreet)
                        .Select(address => address.Number)
                        .Distinct()
                        .OrderBy(number => number)
                        .ToList();

                    numberComboBox.ItemsSource = numbers;
                }
                else
                {
                    numberComboBox.ItemsSource = null;
                }
            }
            else
            {
                streetComboBox.ItemsSource = null;
                numberComboBox.ItemsSource = null;
            }
        }

        private void InputTime()
        {
            for (int hour = 0; hour < 24; hour++)
            {
                string hourText = hour.ToString("00");
                hourComboBox.Items.Add(hourText);
            }
        }

        private void InputAddressForCity(ComboBox cityComboBox, ComboBox streetComboBox, ComboBox numberComboBox)
        {
            InputAddress(cityComboBox, streetComboBox, numberComboBox);
            if (cityComboBox.SelectedItem is KeyValuePair<int, string> selectedCity)
            {
                if (cityComboBox == startCityComboBox) StartAddressId = selectedCity.Key;
                else if (cityComboBox == endCityComboBox) EndAddressId = selectedCity.Key;
            }
        }

        private void SetDetailedAddressId(ComboBox streetComboBox, ComboBox numberComboBox, bool isStartAddress)
        {
            if (streetComboBox.SelectedItem != null && numberComboBox.SelectedItem != null)
            {
                var selectedStreet = streetComboBox.SelectedItem.ToString();
                var selectedNumber = numberComboBox.SelectedItem.ToString();
                AddressRepository _addressRepository = new AddressRepository();
                var address = _addressRepository.GetByAddress(selectedStreet, selectedNumber);
                if (address != null)
                {
                    if (isStartAddress) DetailedStartAddressId = address.Id;
                    else DetailedEndAddressId = address.Id;
                }
            }
        }

        private DateTime CreateDateTimeFromSelections()
        {
            if (dateDp.SelectedDate.HasValue &&
                hourComboBox.SelectedItem is string selectedHour &&
                minuteComboBox.SelectedItem is ComboBoxItem selectedMinuteItem)
            {
                int hour = int.Parse(selectedHour);
                int minute = int.Parse(selectedMinuteItem.Content.ToString());

                return new DateTime(dateDp.SelectedDate.Value.Year, dateDp.SelectedDate.Value.Month, dateDp.SelectedDate.Value.Day, hour, minute, 0);
            }
            else
            {
                MessageBox.Show("Molimo unesite validan datum.");
                return DateTime.MinValue;
            }
        }

        private void UpdateDriverList()
        {
            /*if (!AreAllCriteriaMet())
            {
                MessageBox.Show("Enter all values.");
                return;
            }*/

            VehicleRepository vehicleRepository = new VehicleRepository();
            List<int> drivers = vehicleRepository.GetDriverIdsByLocationId(StartAddressId);
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
                    Tag = driver.Id
                };
                driversComboBox.Items.Add(item);
            }
        }

        private void driversComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (driversComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                SelectedDriverId = (int)selectedItem.Tag;
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

        /*private bool AreAllCriteriaMet()
        {
            return dpDepartureDate.SelectedDate != null &&
                   cbStartCountry.SelectedItem != null &&
                   cbStartCity.SelectedItem != null &&
                   cbStartStreet.SelectedItem != null &&
                   cbDestinationCountry.SelectedItem != null &&
                   cbDestinationCity.SelectedItem != null &&
                   cbDestinationStreet.SelectedItem != null &&
                   cbDepartureHour.SelectedItem != null &&
                   cbDepartureMinute.SelectedItem != null;
        }*/

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            DateTime departure = CreateDateTimeFromSelections();

            Drive drive = new(DetailedStartAddressId, DetailedEndAddressId, departure, SelectedDriverId, Tourist.Id, 2, 0);
            DriveRepository driveRepository = new DriveRepository();
            driveRepository.Save(drive);
            MessageBox.Show("Rezervacija uspjesna");
            //this.Close();
        }

        private void minuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDriverList();
        }

        private void startCountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputCities(startCountryComboBox, startCityComboBox);
        }

        private void endCountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputCities(endCountryComboBox, endCityComboBox);
        }

        private void startCityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputAddressForCity(startCityComboBox, startStreetComboBox, startNumberComboBox);
        }

        private void endCityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputAddressForCity(endCityComboBox, endStreetComboBox, endNumberComboBox);
        }

        private void startStreetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDetailedAddressId(startStreetComboBox, startNumberComboBox, true);
        }

        private void endStreetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDetailedAddressId(endStreetComboBox, endNumberComboBox, false);
        }

        private void startNumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDetailedAddressId(startStreetComboBox, startNumberComboBox, true);
        }

        private void endNumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDetailedAddressId(endStreetComboBox, endNumberComboBox, false);
        }
    }
}
