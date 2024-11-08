﻿using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        public ICommand ReservationCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public LocationService _locationService { get; set; }
        public AddressService _addressService { get; set; }
        public GroupDriveService _groupDriveService { get; set; }

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

        private ObservableCollection<string> _languages;
        public ObservableCollection<string> Languages
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
                CheckReservationEligibility();
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

        public GroupDriveViewModel(User user)
        {
            Tourist = user;
            InputCountries();
            InputLanguages();
            _locationService = new LocationService();
            _addressService = new AddressService();
            _groupDriveService = new GroupDriveService();
            ReservationCommand = new RelayCommand<GroupDriveViewModel>(ExecuteReservationCommand);
            CancelCommand = new RelayCommand<GroupDriveViewModel>(ExecuteCancelCommand);
            DepartureDate = DateTime.Today;
            SelectedMinute = "00";
            NumberOfPeople = 0;
            CanReserve = false;
        }

        private void CheckReservationEligibility()
        {
            CanReserve = !string.IsNullOrEmpty(SelectedCountry) &&
                         !string.IsNullOrEmpty(SelectedCity) &&
                         !string.IsNullOrEmpty(StartStreet) &&
                         !string.IsNullOrEmpty(EndStreet) &&
                         !string.IsNullOrEmpty(DepartureHour) &&
                         !string.IsNullOrEmpty(SelectedMinute) &&
                         !string.IsNullOrEmpty(SelectedLanguage) &&
                         !string.IsNullOrEmpty(NumberOfPeople.ToString());
        }

        private void ExecuteReservationCommand(GroupDriveViewModel groupDriveViewModel)
        {
            MessageBox.Show(Reservation());
        }

        private void ExecuteCancelCommand(GroupDriveViewModel groupDriveViewModel)
        {
            SelectedCountry = null;
            SelectedCity = null;
            StartStreet = string.Empty;
            EndStreet = string.Empty;
            DepartureDate = DateTime.Today;
            DepartureHour = string.Empty;
            SelectedMinute = "00";
            SelectedLanguage = null;
            NumberOfPeople = 1;

            InputCountries();
            InputLanguages();
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

        private void InputLanguages()
        {
            var languages = GetDistinctLanguages();
            Languages = new ObservableCollection<string>(languages);
        }

        private List<string> GetDistinctCountries()
        {
            return new LocationService().GetAll()
                                            .Select(loc => loc.Country)
                                            .Distinct()
                                            .OrderBy(c => c)
                                            .ToList();
        }

        private List<string> GetDistinctLanguages()
        {
            return new LanguageService().GetAll()
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
                Cities = new ObservableCollection<string>();
                return;
            }

            var cities = _locationService.GetCitiesByCountry(selectedCountry);

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

            AddressService addressService = new AddressService();
            var addressObj = addressService.GetByAddress(streetName, streetNumber);
            if (addressObj != null)
            {
                if (isStartAddress) DetailedStartAddressId = addressObj.Id;
                else DetailedEndAddressId = addressObj.Id;
            }
        }

        public string Reservation()
        {
            DateTime departure = _groupDriveService.CreateDateTimeFromSelections(DepartureDate, DepartureHour, SelectedMinute);

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

            Language language = _groupDriveService.FindLanguageByName(SelectedLanguage);
            GroupDrive groupDrive = new GroupDrive(DetailedStartAddressId, DetailedEndAddressId, departure, DateTime.Now, Tourist.Id, 2, 0, 0, language.Id, NumberOfPeople);
            _groupDriveService.Create(groupDrive);

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
            return _groupDriveService.AddNewAddress(address, SelectedCity, SelectedCountry);
        }
    }
}
