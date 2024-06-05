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
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace BookingApp.WPF.ViewModel.Tourist
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
        public ICommand ReservationCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public LocationRepository _locationRepository { get; set; }
        public AddressRepository _addressRepository { get; set; }
        public FastDriveService _fastDriveService { get; set; }

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
                CheckReservationEligibility();
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
                CheckReservationEligibility();
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
                CheckReservationEligibility();
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
                CheckReservationEligibility();
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
                CheckReservationEligibility();
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
                CheckReservationEligibility();
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
                CheckReservationEligibility();
            }
        }

        private bool _canReserve;
        public bool CanReserve
        {
            get { return _canReserve; }
            set
            {
                if (_canReserve != value)
                {
                    _canReserve = value;
                    OnPropertyChanged(nameof(CanReserve));
                }
            }
        }

        public FastDriveViewModel(User user)
        {
            Tourist = user;
            InputCountries();
            _locationRepository = new LocationRepository();
            _addressRepository = new AddressRepository();
            _fastDriveService = new FastDriveService();
            ReservationCommand = new RelayCommand<FastDriveViewModel>(ExecuteReservationCommand);
            CancelCommand = new RelayCommand<FastDriveViewModel>(ExecuteCancelCommand);
            SelectedMinute = "00";
            DepartureDate = DateTime.Today;
            CanReserve = false;
        }

        private void CheckReservationEligibility()
        {
            CanReserve = !string.IsNullOrEmpty(SelectedCountry) &&
                         !string.IsNullOrEmpty(SelectedCity) &&
                         !string.IsNullOrEmpty(StartStreet) &&
                         !string.IsNullOrEmpty(EndStreet) &&
                         !string.IsNullOrEmpty(DepartureHour) &&
                         !string.IsNullOrEmpty(SelectedMinute);
        }

        private void ExecuteReservationCommand(FastDriveViewModel fastDriveViewModel)
        {
            MessageBox.Show(Reservation());
        }

        private void ExecuteCancelCommand(FastDriveViewModel fastDriveViewModel)
        {
            SelectedCountry = null;
            SelectedCity = null;
            StartStreet = string.Empty;
            EndStreet = string.Empty;
            DepartureDate = DateTime.Today;
            DepartureHour = string.Empty;
            SelectedMinute = "00";

            InputCountries();
            Cities = new ObservableCollection<string>();
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

        private void InputCities()
        {
            string selectedCountry = SelectedCountry;
            CountryName = selectedCountry;

            if (string.IsNullOrEmpty(selectedCountry))
            {
                Cities = new ObservableCollection<string>();
                return;
            }

            var cities = _fastDriveService.GetCitiesByCountry(selectedCountry);

            if (cities.Count > 0)
            {
                UpdateCityComboBox(cities);
            }
            else
            {
                Cities = new ObservableCollection<string>();
            }
        }

        private void UpdateCityComboBox(List<KeyValuePair<int, string>> items)
        {
            Cities = new ObservableCollection<string>(items.Select(city => city.Value).ToList());
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
            DateTime departure = _fastDriveService.CreateDateTimeFromSelections(DepartureDate, DepartureHour, SelectedMinute);

            if (!IsValidStreetFormat(StartStreet) || !IsValidStreetFormat(EndStreet))
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
                if (!IsValidStreetFormat(StartStreet) && !IsValidStreetFormat(EndStreet))
                {
                    StartStreet = string.Empty;
                    EndStreet = string.Empty;
                }
                else if (!IsValidStreetFormat(StartStreet))
                {
                    StartStreet = string.Empty;
                }
                else if (!IsValidStreetFormat(EndStreet))
                {
                    EndStreet = string.Empty;
                }
                return errorMessage;
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

                DepartureHour = string.Empty;
                return errorMessage;
            }

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

            FastDrive fastDrive = new FastDrive(DetailedStartAddressId, DetailedEndAddressId, departure, DateTime.Now, Tourist, 2, 0, 0);
            _fastDriveService.Create(fastDrive);
            if (App.CurrentLanguage == "en-US")
            {
                return "Reservation successful!";
            }
            else
            {
                return "Rezervacija uspješna!";
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
            return _fastDriveService.AddNewAddress(address, SelectedCity, SelectedCountry);
        }
    }
}
