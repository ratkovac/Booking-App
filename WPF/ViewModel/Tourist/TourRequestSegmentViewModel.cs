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
                    OnPropertyChanged(nameof(IsValid));
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
                    OnPropertyChanged(nameof(IsValid));
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
                    OnPropertyChanged(nameof(IsValid));
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
                    OnPropertyChanged(nameof(IsValid));
                }
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
                    OnPropertyChanged(nameof(IsValid));
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
                    OnPropertyChanged(nameof(IsValid));
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
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        private string _numberOfPeopleText;
        public string NumberOfPeopleText
        {
            get => _numberOfPeopleText;
            set
            {
                if (_numberOfPeopleText != value)
                {
                    _numberOfPeopleText = value;
                    OnPropertyChanged(nameof(NumberOfPeopleText));

                    if (int.TryParse(value, out int numberOfPeople))
                    {
                        NumberOfPeople = numberOfPeople;
                    }
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(TourDescription) &&
                       NumberOfPeople > 0 &&
                       !SelectedCountry.Equals(default(KeyValuePair<int, string>)) &&
                       !SelectedCity.Equals(default(KeyValuePair<int, string>)) &&
                       !SelectedLanguage.Equals(default(KeyValuePair<int, string>)) &&
                       TourGuestInputs.All(guest => guest.IsValid);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public TourRequestSegmentViewModel(LocationService locationService, ObservableCollection<KeyValuePair<int, string>> countries, ObservableCollection<KeyValuePair<int, string>> languages)
        {
            _locationService = locationService;
            Countries = countries;
            Languages = languages;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            TourGuestInputs.CollectionChanged += TourGuestInputs_CollectionChanged;
        }

        private void TourGuestInputs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (TourGuestViewModel guest in e.NewItems)
                {
                    guest.PropertyChanged += TourGuest_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (TourGuestViewModel guest in e.OldItems)
                {
                    guest.PropertyChanged -= TourGuest_PropertyChanged;
                }
            }

            OnPropertyChanged(nameof(IsValid));
        }

        private void TourGuest_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TourGuestViewModel.FirstName) ||
                e.PropertyName == nameof(TourGuestViewModel.LastName) ||
                e.PropertyName == nameof(TourGuestViewModel.Age))
            {
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private void FillCities()
        {
            var cities = _locationService.GetCitiesByCountry(SelectedCountry.Value);
            Cities.Clear();
            foreach (var city in cities)
            {
                Cities.Add(city);
            }
        }

        private void GenerateGuests(int numberOfGuests)
        {
            while (TourGuestInputs.Count < numberOfGuests)
            {
                TourGuestInputs.Add(new TourGuestViewModel());
            }
            while (TourGuestInputs.Count > numberOfGuests)
            {
                TourGuestInputs.RemoveAt(TourGuestInputs.Count - 1);
            }
        }
    }
}
