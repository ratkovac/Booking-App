using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Service;
using BookingApp.View.Tourist.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.View.ViewModel.Guide
{
    internal class TourStatisticViewModel : INotifyPropertyChanged
    {
        private User user;
        private TourGuestService tourGuestService = new TourGuestService();
        private TourInstanceService tourInstanceService = new TourInstanceService();
        private TourReservationService tourReservationService = new TourReservationService();
        private TourRepository tourRepository = new TourRepository();

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

            public AgeGroup()
            {
                Under18 = 0;
                Between18And50 = 0;
                Above50 = 0;
            }
        }

        public TourStatisticViewModel(User user)
        {
            this.user = user;
            MostVistedTour();
            BestTour = tourRepository.GetById(bestTourInstance.TourId);
            TourInstances = new ObservableCollection<TourInstance>(tourInstanceService.GetFinishedTourInstances());
        }

        public void GetAgeStatistics(int tourInstanceId)
        {
            TourInstance tourInstance = tourInstanceService.GetById(tourInstanceId);

            tourReservations = tourReservationService.GetAllByTourInstanceId(tourInstanceId);
            AgeGroup ageGroup = new AgeGroup();

            foreach (BookingApp.Model.TourReservation tourReservation in tourReservations)
            {
                List<TourGuest> tourGuests = tourGuestService.GetAllByTourReservationId(tourReservation.Id);
                foreach (TourGuest tourGuest in tourGuests)
                {
                    if (Convert.ToInt32(tourGuest.Age) < 18)
                    {
                        ageGroup.Under18++;
                    }
                    else if (Convert.ToInt32(tourGuest.Age) <= 50)
                    {
                        ageGroup.Between18And50++;
                    }
                    else
                    {
                        ageGroup.Above50++;
                    }
                }
            }
            int i = ageGroup.Between18And50;
            AgeGroups = ageGroup;
            OnPropertyChanged(nameof(AgeGroups));


        }

        public void MostVistedTour()
        {
            int max = 0;
            int current = 0;
            List<TourInstance> userTours = GetUserTours(tourInstanceService.GetFinishedTourInstances());
            bestTourInstance = userTours.FirstOrDefault();

            if (userTours.Count == 0)
            {
                bestTourInstance = new TourInstance();
                return;
            }
            foreach (TourInstance tourInstance in userTours)
            {

                tourReservations = tourReservationService.GetAllByTourInstanceId(tourInstance.Id);
                foreach (BookingApp.Model.TourReservation tourReservation in tourReservations)
                {
                    current += tourGuestService.GetTouristNumberByTourReservationId(tourReservation.Id);
                }

                if (max <= current)
                {
                    max = current;
                    bestTourInstance = tourInstance;
                }
                current = 0;

            }
            MostTourists = max;
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
            int max = 0;
            int current = 0;
            List<TourInstance> userTours = GetUserTours(tourInstanceService.GetFinishedTourInstances());
            bestTourInstance = userTours.FirstOrDefault();

            if (userTours.Count == 0)
            {
                bestTourInstance = new TourInstance();
                return;
            }

            foreach (TourInstance tourInstance in userTours)
            {
                if (tourInstance.StartTime.Year == year)
                {
                    tourReservations = tourReservationService.GetAllByTourInstanceId(tourInstance.Id);
                    foreach (BookingApp.Model.TourReservation tourReservation in tourReservations)
                    {
                        current += tourGuestService.GetTouristNumberByTourReservationId(tourReservation.Id);
                    }

                    if (max <= current)
                    {
                        max = current;
                        bestTourInstance = tourInstance;
                    }
                    current = 0;
                }
            }

            MostTourists = max;
            OnPropertyChanged(nameof(MostTourists));
            OnPropertyChanged(nameof(BestTour));

        }

    }
}
