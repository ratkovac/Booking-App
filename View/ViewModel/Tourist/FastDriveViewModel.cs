using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using BookingApp.Service;
using System.Collections.ObjectModel;

namespace BookingApp.View.ViewModel.Tourist
{
    public class FastDriveViewModel : INotifyPropertyChanged
    {
        private User Tourist;
        public Location SelectedLocation { get; set; }
        private int DetailedStartAddressId { get; set; }
        private int DetailedEndAddressId { get; set; }
        public int AddressId { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }

        public LocationRepository _locationRepository { get; set; }
        public AddressRepository _addressRepository { get; set; }
        public FastDriveRepository _fastDriveRepository { get; set; }

        private ObservableCollection<string> _countries;
        public ObservableCollection<string> Countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
                OnPropertyChanged(nameof(Countries));
            }
        }

        private string _selectedCountry;
        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
                InputCities();
            }
        }

        private ObservableCollection<string> _cities;
        public ObservableCollection<string> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                _selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        private string _startStreet;
        public string StartStreet
        {
            get { return _startStreet; }
            set
            {
                _startStreet = value;
                OnPropertyChanged(nameof(StartStreet));
            }
        }

        private string _endStreet;
        public string EndStreet
        {
            get { return _endStreet; }
            set
            {
                _endStreet = value;
                OnPropertyChanged(nameof(EndStreet));
            }
        }

        private DateTime _departureDate;
        public DateTime DepartureDate
        {
            get { return _departureDate; }
            set
            {
                _departureDate = value;
                OnPropertyChanged(nameof(DepartureDate));
            }
        }

        private string _departureHour;
        public string DepartureHour
        {
            get { return _departureHour; }
            set
            {
                _departureHour = value;
                OnPropertyChanged(nameof(DepartureHour));
            }
        }

        private string _selectedMinute;
        public string SelectedMinute
        {
            get { return _selectedMinute; }
            set
            {
                _selectedMinute = value;
                OnPropertyChanged(nameof(SelectedMinute));
            }
        }

        public FastDriveViewModel(User user)
        {
            Tourist = user;
            InputCountries();
            _locationRepository = new LocationRepository();
            _addressRepository = new AddressRepository();
            _fastDriveRepository = new FastDriveRepository();
            DepartureDate = DateTime.Today;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InputCountries()
        {
            var countries = GetDistinctCountries();
            Countries = new ObservableCollection<string>(countries);
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

        private void InputCities()
        {
            string selectedCountry = SelectedCountry;
            CountryName = selectedCountry;

            if (string.IsNullOrEmpty(selectedCountry))
            {
                Cities = new ObservableCollection<string>();
                return;
            }

            var cities = GetCitiesByCountry(selectedCountry);

            if (cities.Count > 0)
            {
                UpdateCityComboBox(cities);
            }
            else
            {
                Cities = new ObservableCollection<string>();
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
            Cities = new ObservableCollection<string>(items.Select(city => city.Value).ToList());
        }

        private void InputAddress()
        {
            if (SelectedCity != null)
            {
                string input = StartStreet.Trim();
                int cityId = _locationRepository.GetCityIdByName(SelectedCity);

                if (string.IsNullOrEmpty(input))
                {
                    StartStreet = "";
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

                var streets = _addressRepository.GetAll()
                    .Where(address => address.LocationId == cityId && address.Street.Equals(streetName, StringComparison.OrdinalIgnoreCase) && address.Number.Equals(streetNumber, StringComparison.OrdinalIgnoreCase))
                    .Select(address => address.Street)
                    .Distinct()
                    .OrderBy(street => street)
                    .ToList();

                StartStreet = string.Join(Environment.NewLine, streets);
            }
            else
            {
                StartStreet = ""; // Ako nije odabran grad, postavljamo StartStreet na prazan string
            }
        }


        /*private void InputAddressForCity()
        {
            InputAddress();
            if (!string.IsNullOrEmpty(SelectedCity))
            {
                SelectedLocation = _locationRepository.GetLocationByCityAndCountry(SelectedCity, SelectedCountry);
            }
        }*/

        private void SetDetailedAddressId(string address, bool isStartAddress)
        {
            string input = address.Trim();

            string[] parts = input.Split(',');

            string streetName = parts[0].Trim();
            string streetNumber = parts[1].Trim();

            AddressRepository addressRepository = new AddressRepository();
            var addressObj = addressRepository.GetByAddress(streetName, streetNumber);
            if (addressObj != null)
            {
                if (isStartAddress) DetailedStartAddressId = addressObj.Id;
                else DetailedEndAddressId = addressObj.Id;
            }
        }

        private DateTime CreateDateTimeFromSelections(DateTime DepartureDate, string DepartureHour, string SelectedMinute)
        {
            // Parsiranje stringa sata u integer vrednost
            int hour = int.Parse(DepartureHour);

            // Parsiranje stringa minuta u integer vrednost
            int minute = int.Parse(SelectedMinute);

            // Provera da li su sati i minuti u validnom opsegu
            if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
            {
                // Ako nisu u validnom opsegu, možete vratiti neku podrazumevanu vrednost ili podići izuzetak
                throw new ArgumentException("Nevažeći sati ili minuti.");
            }

            // Kreiranje DateTime objekta koristeći odabrani datum, sat i minutu
            DateTime selectedDateTime = new DateTime(DepartureDate.Year, DepartureDate.Month, DepartureDate.Day, hour, minute, 0);

            return selectedDateTime;
        }





        public string Reservation()
        {
            DateTime departure = CreateDateTimeFromSelections(DepartureDate, DepartureHour, SelectedMinute);

            SetDetailedAddressId(StartStreet, true);
            SetDetailedAddressId(EndStreet, false);
            if (DetailedStartAddressId == 0)
            {
                DetailedStartAddressId = AddNewAddress(StartStreet);
            }

            if (DetailedEndAddressId == 0)
            {
                DetailedEndAddressId = AddNewAddress(EndStreet);
            }

            FastDrive fastDrive = new FastDrive(DetailedStartAddressId, DetailedEndAddressId, departure, DateTime.Now, Tourist, 2, 0);
            _fastDriveRepository.Save(fastDrive);
            return "Rezervacija uspješna";
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
                Location = SelectedLocation,
                Street = streetName,
                Number = streetNumber
            };

            addressRepository.Save(newAddress);

            return newAddress.Id;
        }
    }
}
