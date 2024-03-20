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

        private void InputAddress(ComboBox cityComboBox, ComboBox streetComboBox)
        {
            if (cityComboBox.SelectedItem != null)
            {
                var selectedCity = (KeyValuePair<int, string>)cityComboBox.SelectedItem;
                AddressRepository addressRepository = new AddressRepository();

                var streets = addressRepository.GetAll()
                    .Where(address => address.LocationId == selectedCity.Key)
                    .Select(address => address.Street)
                    .Distinct()
                    .OrderBy(street => street)
                    .ToList();

                streetComboBox.ItemsSource = streets;
            }
            else
            {
                streetComboBox.ItemsSource = null;
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

        private void InputAddressForCity(ComboBox cityComboBox, ComboBox streetComboBox)
        {
            InputAddress(cityComboBox, streetComboBox);
            if (cityComboBox.SelectedItem is KeyValuePair<int, string> selectedCity)
            {
                if (cityComboBox == startCityComboBox) StartAddressId = selectedCity.Key;
                else if (cityComboBox == endCityComboBox) EndAddressId = selectedCity.Key;
            }
        }

        private void SetDetailedAddressId(ComboBox streetComboBox, bool isStartAddress)
        {
            if (streetComboBox.SelectedItem != null)
            {
                var selectedStreet = streetComboBox.SelectedItem.ToString();
                AddressRepository addressRepository = new AddressRepository();
                var address = addressRepository.GetByAddress(selectedStreet);
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
            if (!AreAllCriteriaMet())
            {
                MessageBox.Show("Molimo popunite sva polja.");
                return;
            }

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


        private bool AreAllCriteriaMet()
        {
            return dateDp.SelectedDate != null &&
                   startCountryComboBox.SelectedItem != null &&
                   startCityComboBox.SelectedItem != null &&
                   startStreetComboBox.SelectedItem != null &&
                   endCountryComboBox.SelectedItem != null &&
                   endCityComboBox.SelectedItem != null &&
                   endStreetComboBox.SelectedItem != null &&
                   hourComboBox.SelectedItem != null &&
                   minuteComboBox.SelectedItem != null;
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            DateTime departure = CreateDateTimeFromSelections();

            Drive drive = new(DetailedStartAddressId, DetailedEndAddressId, departure, SelectedDriverId, Tourist.Id, 2, 0);
            DriveRepository driveRepository = new DriveRepository();
            driveRepository.Save(drive);
            MessageBox.Show("Rezervacija uspješna");
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
            InputAddressForCity(startCityComboBox, startStreetComboBox);
        }

        private void endCityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputAddressForCity(endCityComboBox, endStreetComboBox);
        }

        private void startStreetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDetailedAddressId(startStreetComboBox, true);
        }

        private void endStreetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetDetailedAddressId(endStreetComboBox, false);
        }
    }
}
