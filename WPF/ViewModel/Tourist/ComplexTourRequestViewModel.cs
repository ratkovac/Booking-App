using BookingApp.Model;
using BookingApp.Service;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ComplexTourRequestViewModel
    {
        private User Tourist;
        public ObservableCollection<TourRequestSegmentViewModel> TourSegments { get; set; }

        public ObservableCollection<KeyValuePair<int, string>> Countries { get; private set; }
        public ObservableCollection<KeyValuePair<int, string>> Languages { get; private set; }
        public ICommand AddSegmentCommand { get; set; }
        public ICommand RemoveSegmentCommand { get; set; }
        public ICommand SubmitCommand { get; set; }

        private TourRequestService _tourRequestService;
        private TourRequestSegmentService _tourRequestSegmentService;
        private TourRequestGuestService _tourRequestGuestService;

        private LocationService _locationService;
        private LanguageService _languageService;

        public ComplexTourRequestViewModel(User tourist, LocationService locationService, LanguageService languageService, TourRequestService tourRequestService, TourRequestSegmentService tourRequestSegmentService, TourRequestGuestService tourRequestGuestService)
        {
            Tourist = tourist;
            TourSegments = new ObservableCollection<TourRequestSegmentViewModel>();
            Countries = new ObservableCollection<KeyValuePair<int, string>>();
            Languages = new ObservableCollection<KeyValuePair<int, string>>();
            _locationService = locationService;
            _languageService = languageService;
            _tourRequestService = tourRequestService;
            _tourRequestSegmentService = tourRequestSegmentService;
            _tourRequestGuestService = tourRequestGuestService;
            AddSegmentCommand = new RelayCommand<ComplexTourRequestViewModel>(ExecuteAddSegmentCommand);
            RemoveSegmentCommand = new RelayCommand<object>(ExecuteRemoveSegmentCommand);
            SubmitCommand = new RelayCommand<ComplexTourRequestViewModel>(ExecuteSubmitCommand);
            FillCountries();
            FillLanguages();
            AddSegment();
        }

        private void ExecuteAddSegmentCommand(ComplexTourRequestViewModel complexTourRequestViewModel)
        {
            AddSegment();
        }

        private void ExecuteRemoveSegmentCommand(object sender)
        {
            var button = sender as Button;
            if (button != null && button.DataContext is TourRequestSegmentViewModel segment)
            {
                RemoveSegment(segment);
            }
        }

        private void ExecuteSubmitCommand(ComplexTourRequestViewModel complexTourRequestViewModel)
        {
            CreateTourRequest();
            MessageBox.Show("Tour request added successfully!");
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
            foreach (var segment in TourSegments)
            {
                segment.IsExpanded = false;
            }

            var newSegment = new TourRequestSegmentViewModel(_locationService, Countries, Languages);
            newSegment.IsExpanded = true;
            TourSegments.Add(newSegment);
        }


        public void RemoveSegment(TourRequestSegmentViewModel segment)
        {
            if (TourSegments.Count > 1)
            {
                TourSegments.Remove(segment);
            }
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

    }
}
