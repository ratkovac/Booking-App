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
    public class FastDriveSearchService
    {
        private User Tourist;
        private int SelectedLocationId { get; set; }
        private int AddressId { get; set; }
        private int DetailedStartAddressId { get; set; }
        private int DetailedEndAddressId { get; set; }

        public FastDriveSearchService(User user)
        {
            Tourist = user;
        }

        public List<string> GetDistinctCountries()
        {
            return new LocationRepository().GetAll()
                                            .Select(loc => loc.Country)
                                            .Distinct()
                                            .OrderBy(c => c)
                                            .ToList();
        }

        public List<KeyValuePair<int, string>> GetCitiesByCountry(string country)
        {
            return new LocationRepository().GetAll()
                                            .Where(location => location.Country == country)
                                            .Select(location => new KeyValuePair<int, string>(location.Id, location.City))
                                            .Distinct()
                                            .OrderBy(pair => pair.Value)
                                            .ToList();
        }

        public void InputAddressForCity(ComboBox cityComboBox, TextBox streetTextBox)
        {
            InputAddress(cityComboBox, streetTextBox);
            if (cityComboBox.SelectedItem is KeyValuePair<int, string> selectedCity)
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

        public void SetDetailedAddressId(TextBox streetTextBox, bool isStartAddress)
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

        public DateTime CreateDateTimeFromSelections(DateTime? selectedDate, string hourText, ComboBoxItem selectedMinuteItem)
        {
            if (selectedDate.HasValue &&
                !string.IsNullOrEmpty(hourText) &&
                int.TryParse(hourText, out int hour) &&
                hour >= 0 && hour < 24 &&
                selectedMinuteItem != null)
            {
                int minute = int.Parse(selectedMinuteItem.Content.ToString());

                return new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, selectedDate.Value.Day, hour, minute, 0);
            }
            else
            {
                throw new ArgumentException("Molimo unesite validan datum i sat.");
            }
        }

        public void Reservation(TextBox startStreetTextBox, TextBox endStreetTextBox, DateTime departure)
        {
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

    }
}
