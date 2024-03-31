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

namespace BookingApp.Service
{
    internal class FastDriveSearchService
    {
        private User Tourist;
        private int AddressId { get; set; }
        private int DetailedStartAddressId { get; set; }
        private int DetailedEndAddressId { get; set; }
        private int SelectedLocationId { get; set; }
        private ComboBox CountryComboBox;
        private ComboBox CityComboBox;
        private TextBox startStreetTextBox;
        private TextBox endStreetTextBox;
        private DatePicker dateDp;
        private TextBox hourTextBox;
        private ComboBox minuteComboBox;

        public FastDriveSearchService(User user, ComboBox countryComboBox, ComboBox cityComboBox, TextBox startStreetTextBox, TextBox endStreetTextBox, DatePicker dateDp, TextBox hourTextBox, ComboBox minuteComboBox)
        {
            Tourist = user;
            CountryComboBox = countryComboBox;
            CityComboBox = cityComboBox;
            this.startStreetTextBox = startStreetTextBox;
            this.endStreetTextBox = endStreetTextBox;
            this.dateDp = dateDp;
            this.hourTextBox = hourTextBox;
            this.minuteComboBox = minuteComboBox;

            InputCountries(CountryComboBox);
            HandleReservationClick();
        }

        private void InputCountries(ComboBox comboBox)
        {
            var countries = GetDistinctCountries();
            UpdateComboBoxItems(CountryComboBox, countries);
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
            string selectedCountry = CountryComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedCountry))
            {
                ClearComboBox(CityComboBox);
                return;
            }

            var cities = GetCitiesByCountry(selectedCountry);

            if (cities.Count > 0)
            {
                UpdateCityComboBox(cities);
            }
            else
            {
                ClearComboBox(CityComboBox);
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

        private void UpdateCityComboBox(List<KeyValuePair<int, string>> items)
        {
            CityComboBox.ItemsSource = items;
            CityComboBox.DisplayMemberPath = "Value";
            CityComboBox.SelectedValuePath = "Key";

            CityComboBox.SelectionChanged += (sender, e) =>
            {
                if (CityComboBox.SelectedItem is KeyValuePair<int, string> selectedCity)
                {
                    SelectedLocationId = selectedCity.Key;
                }
            };
        }

        private void ClearComboBox(ComboBox comboBox)
        {
            comboBox.ItemsSource = null;
        }

        private void InputAddressForCity(ComboBox cityComboBox, TextBox streetTextBox)
        {
            InputAddress(CityComboBox, startStreetTextBox);
            if (CityComboBox.SelectedItem is KeyValuePair<int, string> selectedCity)
            {
                AddressId = selectedCity.Key;
            }
        }

        private void InputAddress(ComboBox cityComboBox, TextBox streetTextBox)
        {
            if (cityComboBox.SelectedItem != null)
            {
                var selectedCity = (KeyValuePair<int, string>)cityComboBox.SelectedItem;
                AddressRepository addressRepository = new AddressRepository();

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

                return new DateTime(dateDp.SelectedDate.Value.Year, dateDp.SelectedDate.Value.Month, dateDp.SelectedDate.Value.Day, hour, minute, 0);
            }
            else
            {
                MessageBox.Show("Molimo unesite validan datum i sat.");
                return DateTime.MinValue;
            }
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

        private int AddNewAddress(string address)
        {
            string[] parts = address.Split(',');

            if (parts.Length != 2)
            {
                MessageBox.Show("Neispravan format unosa. Molimo unesite ulicu i broj odvojene zarezom.");
                return 0;
            }

            string streetName = parts[0].Trim();
            string streetNumber = parts[1].Trim();

            AddressRepository addressRepository = new AddressRepository();

            Address newAddress = new Address
            {
                Id = addressRepository.NextId(),
                LocationId = SelectedLocationId,
                Street = streetName,
                Number = streetNumber
            };

            addressRepository.Save(newAddress);

            return newAddress.Id;
        }

        public void HandleReservationClick()
        {
            if (!AreAllCriteriaMet())
            {
                return;
            }

            DateTime departure = CreateDateTimeFromSelections();

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

            Drive drive = new Drive(DetailedStartAddressId, DetailedEndAddressId, departure, Tourist.Id, 2, 0);
            DriveRepository driveRepository = new DriveRepository();
            driveRepository.Save(drive);
            MessageBox.Show("Rezervacija uspješna");
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
