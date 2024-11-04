using BookingApp.Model;
using BookingApp.Service;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourRequestViewModel : INotifyPropertyChanged
    {
        private User Tourist;
        public ObservableCollection<TourRequestSegmentViewModel> TourSegments { get; set; }

        public ObservableCollection<KeyValuePair<int, string>> Countries { get; private set; }
        public ObservableCollection<KeyValuePair<int, string>> Languages { get; private set; }
        public ICommand CancelCommand { get; set; }

        private TourRequestService _tourRequestService;
        private TourRequestSegmentService _tourRequestSegmentService;
        private TourRequestGuestService _tourRequestGuestService;

        private LocationService _locationService;
        private LanguageService _languageService;

        public event PropertyChangedEventHandler PropertyChanged;

        public TourRequestViewModel(User tourist, LocationService locationService, LanguageService languageService, TourRequestService tourRequestService, TourRequestSegmentService tourRequestSegmentService, TourRequestGuestService tourRequestGuestService)
        {
            Tourist = tourist;
            TourSegments = new ObservableCollection<TourRequestSegmentViewModel>();
            TourSegments.CollectionChanged += TourSegments_CollectionChanged;
            Countries = new ObservableCollection<KeyValuePair<int, string>>();
            Languages = new ObservableCollection<KeyValuePair<int, string>>();
            _locationService = locationService;
            _languageService = languageService;
            _tourRequestService = tourRequestService;
            _tourRequestSegmentService = tourRequestSegmentService;
            _tourRequestGuestService = tourRequestGuestService;
            CancelCommand = new RelayCommand<TourRequestViewModel>(ExecuteCancelCommand);
            FillCountries();
            FillLanguages();
            AddSegment();
        }

        private void ExecuteCancelCommand(TourRequestViewModel tourRequestViewModel)
        {
            foreach (var segment in TourSegments)
            {
                segment.TourDescription = string.Empty;
                segment.NumberOfPeople = 0;
                segment.NumberOfPeopleText = string.Empty;
                segment.StartDate = DateTime.Now;
                segment.EndDate = DateTime.Now;
                segment.SelectedCountry = new KeyValuePair<int, string>();
                segment.SelectedCity = new KeyValuePair<int, string>();
                segment.SelectedLanguage = new KeyValuePair<int, string>();
                segment.TourGuestInputs.Clear();
            }
            FillCountries();
            FillLanguages();
        }

        private void FillCountries()
        {
            var countries = _locationService.GetAllCountries();
            Countries.Clear();
            foreach (var country in countries)
            {
                Countries.Add(country);
            }
        }

        private void FillLanguages()
        {
            var languages = _languageService.GetAll();
            Languages.Clear();
            foreach (var language in languages)
            {
                Languages.Add(new KeyValuePair<int, string>(language.Id, language.Name));
            }
        }

        public void AddSegment()
        {
            var newSegment = new TourRequestSegmentViewModel(_locationService, Countries, Languages);
            newSegment.PropertyChanged += Segment_PropertyChanged;
            TourSegments.Add(newSegment);
        }

        public void CreateTourRequest()
        {
            bool isComplexRequest = TourSegments.Count != 1;

            var request = new TourRequest(Tourist.Id, isComplexRequest);
            _tourRequestService.Create(request);

            foreach (var segment in TourSegments)
            {
                var tourSegment = new TourRequestSegment(
                    request.Id,
                    segment.TourDescription,
                    segment.SelectedCity.Key,
                    segment.SelectedLanguage.Key,
                    segment.NumberOfPeople,
                    segment.StartDate,
                    segment.EndDate
                );

                _tourRequestSegmentService.Create(tourSegment);

                foreach (var guest in segment.TourGuestInputs)
                {
                    var guestName = $"{guest.FirstName} {guest.LastName}";

                    var tourRequestGuest = new TourRequestGuest(
                        guestName,
                        guest.Age,
                        Tourist.Id,
                        tourSegment.Id
                    );

                    _tourRequestGuestService.Create(tourRequestGuest);
                }
            }
        }

        private void TourSegments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CanConfirm));
        }

        private void Segment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TourRequestSegmentViewModel.IsValid))
            {
                OnPropertyChanged(nameof(CanConfirm));
            }
        }

        public bool CanConfirm
        {
            get
            {
                return TourSegments.All(segment => segment.IsValid);
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
