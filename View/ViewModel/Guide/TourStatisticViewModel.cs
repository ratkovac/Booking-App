using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Service;
using BookingApp.View.Tourist.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.View.ViewModel.Guide
{
    internal class TourStatisticViewModel : INotifyPropertyChanged
    {
        private User user;
        private TourGuestService tourGuestService;
        private TourInstanceService tourInstanceService;
        private TourReservationService tourReservationService;
        private TourRepository tourRepository;

        private List<TourInstance> tourInstances;
        private List<BookingApp.Model.TourReservation> tourReservations;
        private TourInstance bestTourInstance;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _mostTourists;
        public int MostTourists
        {
            get { return _mostTourists; }
            set
            {
                _mostTourists = value;
                OnPropertyChanged(nameof(BestTour));
            }
        }

        private Tour _bestTour;
        public Tour BestTour
        {
            get { return _bestTour; }
            set
            {
                _bestTour = value;
                OnPropertyChanged(nameof(BestTour));
            }
        }

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

        private AgeGroup _ageGroups;

        public AgeGroup AgeGroups
        {
            get { return _ageGroups; }
            set
            {
                _ageGroups = value;
                OnPropertyChanged(nameof(AgeGroups));
            }
        }

        public struct AgeGroup
        {
            public int Under18 { get; set; }
            public int Between18And50 { get; set; }
            public int Above50 { get; set; }

            public static AgeGroup operator +(AgeGroup a, AgeGroup b)
            {
                return new AgeGroup
                {
                    Under18 = a.Under18 + b.Under18,
                    Between18And50 = a.Between18And50 + b.Between18And50,
                    Above50 = a.Above50 + b.Above50
                };
            }
        }


        public TourStatisticViewModel(User user)
        {
            this.user = user;
            tourGuestService = new TourGuestService();
            tourInstanceService = new TourInstanceService();
            tourReservationService = new TourReservationService();
            tourRepository = new TourRepository();
            MostVistedTour();
            BestTour = tourRepository.GetById(bestTourInstance.TourId);
            TourInstances = new ObservableCollection<TourInstance>(tourInstanceService.GetFinishedTourInstances());
        }


        public void GetAgeStatistics(int tourInstanceId)
        {
            TourInstance tourInstance = tourInstanceService.GetById(tourInstanceId);
            tourReservations = tourReservationService.GetAllByTourInstanceId(tourInstanceId);
            AgeGroups = CalculateAgeStatistics(tourReservations);
            OnPropertyChanged(nameof(AgeGroups));
        }

        private AgeGroup GetAgeGroupForReservation(int reservationId)
        {
            AgeGroup ageGroup = new AgeGroup();
            List<TourGuest> tourGuests = tourGuestService.GetAllByTourReservationId(reservationId);

            foreach (TourGuest tourGuest in tourGuests)
            {
                int age = Convert.ToInt32(tourGuest.Age);
                if (age < 18)
                {
                    ageGroup.Under18++;
                }
                else if (age <= 50)
                {
                    ageGroup.Between18And50++;
                }
                else
                {
                    ageGroup.Above50++;
                }
            }

            return ageGroup;
        }

        private AgeGroup CalculateAgeStatistics(List<BookingApp.Model.TourReservation> reservations)
        {
            AgeGroup totalAgeGroup = new AgeGroup();
            foreach (BookingApp.Model.TourReservation reservation in reservations)
            {
                AgeGroup ageGroup = GetAgeGroupForReservation(reservation.Id);
                totalAgeGroup += ageGroup;
            }
            return totalAgeGroup;
        }

        public void MostVistedTour()
        {
            List<TourInstance> userTours = GetUserTours(tourInstanceService.GetFinishedTourInstances());
            if (userTours.Count == 0)
            {
                InitializeBestTourInstance();
                return;
            }

            int mostTourists = CalculateMostTourists(userTours);
            SetMostTourists(mostTourists);
        }

        private void InitializeBestTourInstance()
        {
            bestTourInstance = new TourInstance();
            OnPropertyChanged(nameof(BestTour));
        }

        private int CalculateMostTourists(List<TourInstance> userTours)
        {
            int max = 0;
            foreach (TourInstance tourInstance in userTours)
            {
                int current = CalculateTotalTourists(tourInstance);
                if (max <= current)
                {
                    max = current;
                    bestTourInstance = tourInstance;
                }
            }
            return max;
        }

        private int CalculateTotalTourists(TourInstance tourInstance)
        {
            int totalTourists = 0;
            tourReservations = tourReservationService.GetAllByTourInstanceId(tourInstance.Id);
            foreach (BookingApp.Model.TourReservation tourReservation in tourReservations)
            {
                totalTourists += tourGuestService.GetTouristNumberByTourReservationId(tourReservation.Id);
            }
            return totalTourists;
        }

        private void SetMostTourists(int mostTourists)
        {
            MostTourists = mostTourists;
            OnPropertyChanged(nameof(MostTourists));
            OnPropertyChanged(nameof(BestTour));
        }


        private List<TourInstance> GetUserTours(List<TourInstance> tourInstances)
        {
            List<TourInstance> userTours = new List<TourInstance>();
            foreach (TourInstance tourInstance in tourInstances)
            {
                if (tourInstance.GuideId == user.Id)
                {
                    userTours.Add(tourInstance);
                }
            }
            return userTours;
        }

        public void MostVistedTourByYear(int year)
        {
            List<TourInstance> userTours = GetUserTours(tourInstanceService.GetFinishedTourInstances());
            List<TourInstance> filteredTours = FilterToursByYear(userTours, year);

            bestTourInstance = FindMostVisitedTour(filteredTours);

            if (bestTourInstance == null)
            {
                bestTourInstance = new TourInstance();
            }

            MostTourists = GetTotalVisitorsForTourInstance(bestTourInstance);
            OnPropertyChanged(nameof(MostTourists));
            OnPropertyChanged(nameof(BestTour));
        }

        private TourInstance FindMostVisitedTour(List<TourInstance> tourInstances)
        {
            int maxVisitors = 0;
            TourInstance mostVisitedTour = null;
            foreach (TourInstance tourInstance in tourInstances)
            {
                int visitors = GetTotalVisitorsForTourInstance(tourInstance);
                if (visitors > maxVisitors)
                {
                    maxVisitors = visitors;
                    mostVisitedTour = tourInstance;
                }
            }
            return mostVisitedTour;
        }

        private List<TourInstance> FilterToursByYear(List<TourInstance> tourInstances, int year)
        {
            return tourInstances.Where(tourInstance => tourInstance.StartTime.Year == year).ToList();
        }

        private int GetTotalVisitorsForTourInstance(TourInstance tourInstance)
        {
            int totalVisitors = 0;
            tourReservations = tourReservationService.GetAllByTourInstanceId(tourInstance.Id);
            foreach (BookingApp.Model.TourReservation tourReservation in tourReservations)
            {
                totalVisitors += tourGuestService.GetTouristNumberByTourReservationId(tourReservation.Id);
            }
            return totalVisitors;
        }



    }
}
