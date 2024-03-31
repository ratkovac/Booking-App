using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
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
    public partial class FastDriveSearch : Page
    {
        private FastDriveSearchService fastDriveSearchService;
        public User Tourist;
        private int SelectedLocationId { get; set; }

        public FastDriveSearch(User user)
        {
            InitializeComponent();
            Tourist = user;
            fastDriveSearchService = new FastDriveSearchService(Tourist);
            InputCountries();
        }

        private void InputCountries()
        {
            var countries = fastDriveSearchService.GetDistinctCountries();
            UpdateComboBoxItems(CountryComboBox, countries);
        }

        private void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputCities();
        }

        private void InputCities()
        {
            string selectedCountry = CountryComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedCountry))
            {
                ClearComboBox(CityComboBox);
                return;
            }

            var cities = fastDriveSearchService.GetCitiesByCountry(selectedCountry);

            if (cities.Count > 0)
            {
                UpdateCityComboBox(CityComboBox, cities);
            }
            else
            {
                ClearComboBox(CityComboBox);
            }
        }

        private void UpdateComboBoxItems(ComboBox comboBox, List<string> items)
        {
            comboBox.ItemsSource = items;
        }

        private void UpdateCityComboBox(ComboBox cityComboBox, List<KeyValuePair<int, string>> items)
        {
            cityComboBox.ItemsSource = items;
            cityComboBox.DisplayMemberPath = "Value";
            cityComboBox.SelectedValuePath = "Key";

            cityComboBox.SelectionChanged += (sender, e) =>
            {
                if (cityComboBox.SelectedItem is KeyValuePair<int, string> selectedCity)
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
            fastDriveSearchService.InputAddressForCity(cityComboBox, streetTextBox);
        }

        private DateTime CreateDateTimeFromSelections()
        {
            return fastDriveSearchService.CreateDateTimeFromSelections(dateDp.SelectedDate, hourTextBox.Text, minuteComboBox.SelectedItem as ComboBoxItem);
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            DateTime departure = CreateDateTimeFromSelections();
            fastDriveSearchService.Reservation(startStreetTextBox, endStreetTextBox, departure);
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InputAddressForCity(CityComboBox, startStreetTextBox);
        }
    }
}
