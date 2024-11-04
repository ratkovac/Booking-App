using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.GuideView.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static BookingApp.View.ViewModel.Guide.TourStatisticViewModel;

namespace BookingApp.View.ViewModel.Guide
{
    internal class TourReviewsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private TourInstanceService tourInstanceService;
        private TourReservationService tourReservationService;
        private TourGuestService tourGuestService;
        private GradeTourService gradeTourService;
        private CheckPointService checkPointService;
        private TourService tourService;

        List<GradeTour> gradeTours = new List<GradeTour>();


        private ObservableCollection<TourInstance> _tourInstances;
        public ObservableCollection<TourInstance> TourInstances
        {
            get { return _tourInstances; }
            set
            {
                _tourInstances = value;
                OnPropertyChanged(nameof(TourInstances));
            }
        }

        private TourInstance _selectedTourInstance;
        public TourInstance SelectedTourInstance
        {
            get { return _selectedTourInstance; }
            set
            {
                _selectedTourInstance = value;
                OnPropertyChanged(nameof(SelectedTourInstance));
            }
        }

        private Tours _selectedTour;
        public Tours SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));
            }
        }

        private List<Tours> tours = new List<Tours>();
        public struct Tours : INotifyPropertyChanged
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set
                {
                    if (_name != value)
                    {
                        _name = value;
                        OnPropertyChanged(nameof(Name));
                    }
                }
            }

            private int _tourInstanceId;
            public int TourInstanceId
            {
                get { return _tourInstanceId; }
                set
                {
                    if (_tourInstanceId != value)
                    {
                        _tourInstanceId = value;
                        OnPropertyChanged(nameof(TourInstanceId));
                    }
                }
            }
            
            private int _tourId;
            public int TourId
            {
                get { return _tourId; }
                set
                {
                    if (_tourId != value)
                    {
                        _tourInstanceId = value;
                        OnPropertyChanged(nameof(TourId));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public Tours(string name, int tourInstanceId, int tourId)
            {
                _name = name;
                _tourInstanceId = tourInstanceId;
                _tourId = tourId;
                PropertyChanged = null;
            }
        }

        private ObservableCollection<Tours> _toursBind;
        public ObservableCollection<Tours> ToursBind
        {
            get { return _toursBind; }
            set
            {
                _toursBind = value;
                OnPropertyChanged(nameof(ToursBind));
            }
        }

        private List<TourReview> tourReviews = new List<TourReview>();
        public struct TourReview : INotifyPropertyChanged
        {

            private string _touristName;
            public string TouristName
            {
                get { return _touristName; }
                set
                {
                    if (_touristName != value)
                    {
                        _touristName = value;
                        OnPropertyChanged(nameof(TouristName));
                    }
                }
            }

            private int _gradeTourId;
            public int GradeTourId
            {
                get { return _gradeTourId; }
                set
                {
                    if (_gradeTourId != value)
                    {
                        _gradeTourId = value;
                        OnPropertyChanged(nameof(GradeTourId));
                    }
                }
            }
            private string _pointText;
            public string PointText
            {
                get { return _pointText; }
                set
                {
                    if (_pointText != value)
                    {
                        _pointText = value;
                        OnPropertyChanged(nameof(PointText));
                    }
                }
            }

            private string _comment;
            public string Comment
            {
                get { return _comment; }
                set
                {
                    if (_comment != value)
                    {
                        _comment = value;
                        OnPropertyChanged(nameof(Comment));
                    }
                }
            }

            private int _grade;
            public int Grade
            {
                get { return _grade; }
                set
                {
                    if (_grade != value)
                    {
                        _grade = value;
                        OnPropertyChanged(nameof(Grade));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public TourReview(string pointText, string comment, int grade, int gradeTourId, string touristName)
            {
                _pointText = pointText;
                _comment = comment;
                _grade = grade;
                _gradeTourId = gradeTourId;
                _touristName = touristName;
                PropertyChanged = null; 
            }
        }


        private ObservableCollection<TourReview> _tourReviews;
        public ObservableCollection<TourReview> TourReviews
        {
            get { return _tourReviews; }
            set
            {
                _tourReviews = value;
                OnPropertyChanged(nameof(TourReviews));
            }
        }

        public void Reviews(int tourInstanceId)
        {

            List<TourReservation> tourReservations = tourReservationService.GetAllByTourInstanceId(tourInstanceId);
            gradeTours.Clear();
            tourReviews.Clear();

            foreach (TourReservation tourReservation in tourReservations)
            {
                GetTourGrades(tourReservation);
            }

            GetCheckPoint(gradeTours);

            TourReviews = new ObservableCollection<TourReview>(tourReviews);
            //OnPropertyChanged(nameof(TourReviews));


        }

        private void GetTourGrades(TourReservation tourReservation)
        {
            List<GradeTour> tempGradeTours = gradeTourService.GetAllRatingsByTour(tourReservation);
            foreach (GradeTour gradeTour in tempGradeTours)
            {
                gradeTours.Add(gradeTour);
            }
        }

        public void GetCheckPoint(List<GradeTour> tempGradeTours)
        {
            foreach (var gradeTour in tempGradeTours)
            {
                ProcessGradeTour(gradeTour);
            }
        }

        private void ProcessGradeTour(GradeTour gradeTour)
        {
            var tourGuests = tourGuestService.GetByTouristAndReservationId(gradeTour.TourReservationId, gradeTour.TouristId);
            foreach (var tourGuest in tourGuests)
            {

                var checkPoint = checkPointService.GetById(tourGuest.CheckpointId);            
                var tourReview = new TourReview(checkPoint.PointText, gradeTour.Comment, gradeTour.Grade, gradeTour.Id, gradeTour.Tourist.Name);
                tourReviews.Add(tourReview);

            }
        }


        public void LoadReviews(int selectedTourInstanceId)
        {
            if (selectedTourInstanceId != null)
            {
                Reviews(selectedTourInstanceId);
            }
            else
            {
                ToursBind = null;
            }
        }

        public void Report(int gradeTourId)
        {
            GradeTour gradeTour = gradeTourService.GetById(gradeTourId);
            gradeTour.IsValid = false;
            gradeTourService.Update(gradeTour);
        }

        private void GetNamesForTourInstances(List<TourInstance> tourInstances)
        {
            foreach (TourInstance tourInstance in tourInstances)
            {
                Tour tour = tourService.GetById(tourInstance.TourId);
                Tours tourBind = new Tours(tour.Name + " " + tourInstance.StartTime.ToString(), tourInstance.Id, tour.Id);
                tours.Add(tourBind);

            }
        }
        public TourReviewsViewModel(User user)
        {
            tourGuestService = new TourGuestService();
            gradeTourService = new GradeTourService();
            checkPointService = new CheckPointService();
            tourGuestService = new TourGuestService();
            tourReservationService = new TourReservationService();
            tourInstanceService = new TourInstanceService();
            tourService = new TourService();
            GetNamesForTourInstances(tourInstanceService.GetFinishedByUserId(user.Id));
            ToursBind = new ObservableCollection<Tours>(tours);
            OnPropertyChanged(nameof(ToursBind));
        }
    }
}
