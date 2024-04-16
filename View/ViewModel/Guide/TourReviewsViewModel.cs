using BookingApp.Model;
using BookingApp.Service;
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

        private TourInstanceService tourInstanceService = new TourInstanceService();
        private TourReservationService tourReservationService = new TourReservationService();
        private TourGuestService tourGuestService = new TourGuestService();
        private GradeTourService gradeTourService = new GradeTourService();
        private CheckPointService checkPointService = new CheckPointService();

        List<CheckPoint> checkPoints = new List<CheckPoint>();
        //List<TourGuest> tourGuests = new List<TourGuest>();
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

        private ObservableCollection<GradeTour> _gradeTours;
        public ObservableCollection<GradeTour> GradeTours
        {
            get { return _gradeTours; }
            set
            {
                _gradeTours = value;
                OnPropertyChanged(nameof(GradeTours));
            }
        }

        private ObservableCollection<CheckPoint> _checkPoints;
        public ObservableCollection<CheckPoint> CheckPoints
        {
            get { return _checkPoints; }
            set
            {
                _checkPoints = value;
                OnPropertyChanged(nameof(CheckPoints));
            }
        }

        private CheckPoint _checkPoint;
        public CheckPoint CheckPoint
        {
            get { return _checkPoint; }
            set
            {
                _checkPoint = value;
                OnPropertyChanged(nameof(CheckPoint));
            }
        }

        private List<TourReview> tourReviews = new List<TourReview>();
        public struct TourReview : INotifyPropertyChanged
        {
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

            public TourReview(string pointText, string comment, int grade, int gradeTourId)
            {
                _pointText = pointText;
                _comment = comment;
                _grade = grade;
                _gradeTourId = gradeTourId;
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
            List<GradeTour> tempGradeTours = new List<GradeTour>();
            gradeTours.Clear();
            checkPoints.Clear();
            tourReviews.Clear();
            foreach (TourReservation tourReservation in tourReservations)
            {
                GetTourGrades(tourReservation);
            }

            GradeTours = new ObservableCollection<GradeTour>(gradeTours);
            GetCheckPoint(gradeTours);

            TourReviews = new ObservableCollection<TourReview>(tourReviews);
            OnPropertyChanged(nameof(TourReviews));


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
            foreach (GradeTour gradeTour in tempGradeTours)
            {
                TourReservation tourReservation = tourReservationService.GetById(gradeTour.TourReservationId);
                List<TourGuest> tourGuests = tourGuestService.GetByTouristAndReservationId(gradeTour.TourReservationId, gradeTour.TouristId);
                foreach (TourGuest tourGuest in tourGuests)
                {
                    if(tourGuest.TouristId ==  gradeTour.TouristId)
                    {
                        CheckPoint checkPoint = checkPointService.GetById(tourGuest.CheckpointId);
                        checkPoints.Add(checkPoint);
                        TourReview tourReview = new TourReview(checkPoint.PointText, gradeTour.Comment, gradeTour.Grade, gradeTour.Id);
                        tourReviews.Add(tourReview);
                    }                   
                }
            }
        }

        public void LoadReviews(int selectedTourInstanceId)
        {
            if (SelectedTourInstance != null)
            {
                Reviews(selectedTourInstanceId);
            }
            else
            {
                TourReviews = null;
            }
        }

        public void Report(int gradeTourId)
        {
            GradeTour gradeTour = gradeTourService.GetById(gradeTourId);
            gradeTour.IsValid = false;
            gradeTourService.Update(gradeTour);
        }

        public TourReviewsViewModel(User user)
        {
            TourInstances = new ObservableCollection<TourInstance>(tourInstanceService.GetFinishedByUserId(user.Id));
        }
    }
}
