using BookingApp.Repository;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourRequestSegmentViewModel : INotifyPropertyChanged
    {
        private LocationService _locationService;

        private string _tourDescription;
        private int _numberOfPeople;
        private DateTime _startDate;
        private DateTime _endDate;
        private KeyValuePair<int, string> _selectedCountry;
        private KeyValuePair<int, string> _selectedCity;
        private KeyValuePair<int, string> _selectedLanguage;

        public ObservableCollection<TourGuestViewModel> TourGuestInputs { get; } = new ObservableCollection<TourGuestViewModel>();

        private ObservableCollection<int> _numberOfPeopleSelection = new ObservableCollection<int>();
        private ObservableCollection<KeyValuePair<int, string>> _languages = new ObservableCollection<KeyValuePair<int, string>>();
        private ObservableCollection<KeyValuePair<int, string>> _countries = new ObservableCollection<KeyValuePair<int, string>>();
        private ObservableCollection<KeyValuePair<int, string>> _cities = new ObservableCollection<KeyValuePair<int, string>>();

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public ObservableCollection<KeyValuePair<int, string>> Countries
        {
            get { return _countries; }
            set
            {
                if (_countries != value)
                {
                    _countries = value;
                    OnPropertyChanged(nameof(Countries));
                }
            }
        }

        public KeyValuePair<int, string> SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (!Equals(_selectedCountry, value))
                {
                    _selectedCountry = value;
                    FillCities();
                    OnPropertyChanged(nameof(SelectedCountry));
                }
            }
        }

        public ObservableCollection<KeyValuePair<int, string>> Cities
        {
            get { return _cities; }
            set
            {
                if (_cities != value)
                {
                    _cities = value;
                    OnPropertyChanged(nameof(Cities));
                    FillCities();
                }
            }
        }

        public KeyValuePair<int, string> SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (!Equals(_selectedCity, value))
                {
                    _selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }

        public ObservableCollection<int> NumberOfPeopleSelection
        {
            get => _numberOfPeopleSelection;
            private set
            {
                _numberOfPeopleSelection = value;
                OnPropertyChanged(nameof(NumberOfPeopleSelection));
            }
        }

        public ObservableCollection<KeyValuePair<int, string>> Languages
        {
            get => _languages;
            set
            {
                if (_languages != value)
                {
                    _languages = value;
                    OnPropertyChanged(nameof(Languages));
                }
            }
        }

        public KeyValuePair<int, string> SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (!Equals(_selectedLanguage, value))
                {
                    _selectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
                }
            }
        }

        public string TourDescription
        {
            get => _tourDescription;
            set
            {
                if (_tourDescription != value)
                {
                    _tourDescription = value;
                    OnPropertyChanged(nameof(TourDescription));
                }
            }
        }

        public int NumberOfPeople
        {
            get => _numberOfPeople;
            set
            {
                if (_numberOfPeople != value)
                {
                    _numberOfPeople = value;
                    GenerateGuests(value);
                    OnPropertyChanged(nameof(NumberOfPeople));
                }
            }
        }

        public TourRequestSegmentViewModel(LocationService locationService, ObservableCollection<KeyValuePair<int, string>> countries, ObservableCollection<KeyValuePair<int, string>> languages)
        {
            NumberOfPeopleOptions();
            _startDate = DateTime.Now;
            _endDate = DateTime.Now;
            Countries = countries;
            Languages = languages;
            _locationService = locationService;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NumberOfPeopleOptions()
        {
            NumberOfPeopleSelection.Clear();
            NumberOfPeopleSelection.Add(1);
            NumberOfPeopleSelection.Add(2);
            NumberOfPeopleSelection.Add(3);
            NumberOfPeopleSelection.Add(4);
            NumberOfPeopleSelection.Add(5);
        }

        private void GenerateGuests(int numberOfPeople)
        {
            TourGuestInputs.Clear();
            if (numberOfPeople > 0)
            {
                for (int i = 0; i < numberOfPeople; i++)
                {
                    TourGuestInputs.Add(new TourGuestViewModel());
                }
            }
        }

        public void FillCities()
        {
            var cities = _locationService.GetCitiesByCountry(SelectedCountry.Value).ToList();
            Cities.Clear();
            foreach (var city in cities)
            {
                Cities.Add(city);
            }
        }
    }
}
