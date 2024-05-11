using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class GroupDriveViewModel : INotifyPropertyChanged
    {
        private User Tourist;
        public Location SelectedLocation { get; set; }
        private int DetailedStartAddressId { get; set; }
        private int DetailedEndAddressId { get; set; }
        public int AddressId { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string LanguageName { get; set; }

        public LocationRepository _locationRepository { get; set; }
        public AddressRepository _addressRepository { get; set; }
        public GroupDriveService _groupDriveService { get; set; }

        private List<string> _countries;
        public List<string> Countries
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

        private List<string> _cities;
        public List<string> Cities
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

        private List<string> _languages;
        public List<string> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                OnPropertyChanged(nameof(Languages));
            }
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
                InputCities();
            }
        }

        private int _numberOfPeople;
        public int NumberOfPeople
        {
            get { return _numberOfPeople; }
            set
            {
                _numberOfPeople = value;
                OnPropertyChanged(nameof(NumberOfPeople));
            }
        }

        public GroupDriveViewModel(User user)
        {
            Tourist = user;
            InputCountries();
            InputLanguages();
            _locationRepository = new LocationRepository();
            _addressRepository = new AddressRepository();
            _groupDriveService = new GroupDriveService();
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
            Countries = new List<string>(countries);
        }

        private void InputLanguages()
        {
            var languages = GetDistinctLanguages();
            Languages = new List<string>(languages);
        }

        private List<string> GetDistinctCountries()
        {
            return new LocationRepository().GetAll()
                                            .Select(loc => loc.Country)
                                            .Distinct()
                                            .OrderBy(c => c)
                                            .ToList();
        }

        private List<string> GetDistinctLanguages()
        {
            return new LanguageRepository().GetAll()
                                            .Select(lng => lng.Name)
                                            .Distinct()
                                            .OrderBy(c => c)
                                            .ToList();
        }

        private void InputCities()
        {
            string selectedCountry = SelectedCountry;
            CountryName = selectedCountry;

            if (string.IsNullOrEmpty(selectedCountry))
            {
                Cities = new List<string>();
                return;
            }

            var cities = _groupDriveService.GetCitiesByCountry(selectedCountry);

            if (cities.Count > 0)
            {
                UpdateCityComboBox(cities);
            }
            else
            {
                Cities = new List<string>();
            }
        }

        private void UpdateCityComboBox(List<KeyValuePair<int, string>> items)
        {
            Cities = new List<string>(items.Select(city => city.Value).ToList());
        }

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

        public string Reservation()
        {
            DateTime departure = _groupDriveService.CreateDateTimeFromSelections(DepartureDate, DepartureHour, SelectedMinute);

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

            GroupDrive groupDrive = new GroupDrive(DetailedStartAddressId, DetailedEndAddressId, departure, DateTime.Now, Tourist, 2, 0, 0, SelectedLanguage, NumberOfPeople);
            _groupDriveService.Create(groupDrive);
            return "Rezervacija uspješna";
        }


        private int AddNewAddress(string address)
        {
            return _groupDriveService.AddNewAddress(address, SelectedCity, SelectedCountry);
        }
    }
}
