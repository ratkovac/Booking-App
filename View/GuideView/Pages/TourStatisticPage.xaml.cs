using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.GuideView.Pages
{
    /// <summary>
    /// Interaction logic for TourStatisticPage.xaml
    /// </summary>
    public partial class TourStatisticPage : Page, INotifyPropertyChanged
    {
        private User user;
        private TourGuestRepository tourGuestRepository = new TourGuestRepository();
        private TourInstanceRepository tourInstanceRepository = new TourInstanceRepository();
        private TourReservationRepository tourReservationRepository = new TourReservationRepository();
        private TourRepository tourRepository = new TourRepository();

        private List<TourInstance> tourInstances;
        private List<TourReservation> tourReservations;
        private TourInstance bestTourInstance;

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


        public TourStatisticPage(User user)
        {
            this.user = user;
            InitializeComponent();
            MostVistedTour();
            BestTour = tourRepository.GetById(bestTourInstance.TourId);
            DataContext = this;
            TourInstances = TourInstances = new ObservableCollection<TourInstance>(tourInstanceRepository.GetFinishedTourInstances());
        }

        private void GetAgeStatistics(int tourInstanceId)
        {
            TourInstance tourInstance = tourInstanceRepository.GetById(tourInstanceId);

            tourReservations = tourReservationRepository.GetAllByTourInstanceId(tourInstanceId);
            AgeGroup ageGroup = new AgeGroup();

            foreach (TourReservation tourReservation in tourReservations)
            {
                List<TourGuest> tourGuests = tourGuestRepository.GetAllByTourReservationId(tourReservation.Id);
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
            AgeGroups = ageGroup;
            
        }
        private void MostVistedTour()
        {
            int max = 0;
            int current = 0;           
            List<TourInstance> userTours = GetUserTours(tourInstanceRepository.GetFinishedTourInstances());
            bestTourInstance = userTours.FirstOrDefault();
            
            if(userTours.Count == 0)
            {
                bestTourInstance = new TourInstance();
                return;
            }

            foreach (TourInstance tourInstance in userTours)
            {

                tourReservations = tourReservationRepository.GetAllByTourInstanceId(tourInstance.Id);
                foreach (TourReservation tourReservation in tourReservations)
                {
                    current += tourGuestRepository.GetTouristNumberByTourReservationId(tourReservation.Id);
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

        private void MostVistedTourByYear(int year)
        {
            int max = 0;
            int current = 0;
            List<TourInstance> userTours = GetUserTours(tourInstanceRepository.GetFinishedTourInstances());
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
                    tourReservations = tourReservationRepository.GetAllByTourInstanceId(tourInstance.Id);
                    foreach (TourReservation tourReservation in tourReservations)
                    {
                        current += tourGuestRepository.GetTouristNumberByTourReservationId(tourReservation.Id);
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


        private void cbOverall_Checked(object sender, RoutedEventArgs e)
        {
         //   cbSpecificYear.IsChecked = false;
            UpdateTourStatistic();         
        }


        private void cbSpecificYear_Checked(object sender, RoutedEventArgs e)
        {
            //cbOverall.IsChecked = false;
            UpdateTourStatistic();           
        }

        private void txtYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTourStatistic();
        }

        private void UpdateTourStatistic()
        {
            if (cbOverall.IsChecked == true)
            {
                MostVistedTour();
            }
            else if (cbSpecificYear.IsChecked == true)
            {
                if (int.TryParse(txtYear.Text, out int year))
                {
                    MostVistedTourByYear(year);
                }
                else
                {
                    MessageBox.Show("Please enter a valid year.");
                }
            }
        }
        private void cbTour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetAgeStatistics(SelectedTourInstance.Id);
            MessageBox.Show(AgeGroups.Under18.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
