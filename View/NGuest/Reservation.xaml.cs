using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.NGuest;
using BookingApp.View.ViewModel.Guest;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Reservation.xaml
    /// </summary>
    public partial class Reservation : Page, INotifyPropertyChanged
    {
        public AccommodationDTO SelectedAccommodation { get; set; }
        public AccommodationReservationRepository AccommodationReservationRepository { get; set; }
        public ObservableCollection<AccommodationReservationDTO> AccommodationReservations { get; set; }

        public ObservableCollection<AccommodationReservationDTO> AvailableAccommodationPeriods { get; set; }

        public ObservableCollection<AccommodationReservationDTO> SortedAccommodationReservations { get; set; }

        public ObservableCollection<string> Images { get; set; }

        public event Action BeginWindowDrag;
        private int currentIndex;

        public Reservation(AccommodationDTO selectedAccommodation, User user)
        {
            InitializeComponent();
            DataContext = this;

            SelectedAccommodation = selectedAccommodation;

            AccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            SortedAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            AccommodationReservationRepository = new AccommodationReservationRepository();

            AvailableAccommodationPeriods = new ObservableCollection<AccommodationReservationDTO>();

            this.User = user;

            Images = new ObservableCollection<string>
            {
                "../../Images/Accommodation.jpg",
                "../../Images/apartment1.jpg",
                "../../Images/apartment2.jpg"
            };

            currentIndex = 1;
            UpdateImageDisplay();
            Update();
        }

        private void Update()
        {
            AccommodationReservations.Clear();
            foreach (AccommodationReservation accommodationReservation in AccommodationReservationRepository.GetAll())
            {
                if (accommodationReservation.Accommodation.Id == SelectedAccommodation.Id)
                {
                    AccommodationReservations.Add(new AccommodationReservationDTO(accommodationReservation));
                }
            }

        }

        private Accommodation accommodation;

        public Accommodation Accommodation
        {
            get
            {
                return accommodation;
            }
            set
            {
                if (value != accommodation)
                {
                    accommodation = value;
                    OnPropertyChanged("Accommodation");
                }
            }
        }

        private int capacity;

        public int Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged(nameof(Capacity));
                }
            }
        }

        private string accommodationName;

        public string AccommodationName
        {
            get
            {
                return accommodationName;
            }
            set
            {
                if (value != accommodationName)
                {
                    accommodationName = value;
                    OnPropertyChanged("AccommodationName");
                }
            }
        }

        private User user;
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                if (value != user)
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }

        private DateOnly startDate;

        public DateOnly StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }

        public DateOnly endDate;

        public DateOnly EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }

        private int reservationDays;

        public int ReservationDays
        {
            get
            {
                return reservationDays;
            }
            set
            {
                if (value != reservationDays)
                {
                    reservationDays = value;
                    OnPropertyChanged("ReservationDays");
                }
            }
        }

        private AccommodationReservationDTO selectedReservation;
        public AccommodationReservationDTO SelectedReservation
        {
            get
            {
                return selectedReservation;
            }
            set
            {
                if (value != selectedReservation)
                {
                    selectedReservation = value;
                    OnPropertyChanged(nameof(SelectedReservation));
                }

            }
        }

        private ObservableCollection<AccommodationReservationDTO> SortAccommodationReservations(DateOnly startDate, DateOnly endDate)
        {

            var sorted = AccommodationReservations.OrderBy(reservation => reservation.StartDate).ToList();

            SortedAccommodationReservations.Clear();
            foreach (var reservation in sorted)
            {
                if (reservation.StartDate >= startDate && reservation.EndDate <= endDate)
                {
                    SortedAccommodationReservations.Add(reservation);
                }
            }

            return SortedAccommodationReservations;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            AvailableAccommodationPeriods.Clear();

            SuggestReservation(startDate, endDate);
            if (AvailableAccommodationPeriods.Count == 0)
                SuggestReservation(startDate.AddDays(-5), endDate.AddDays(5));
            Update();

            if (SelectedAccommodation.Capacity >= capacity &&
                SelectedAccommodation.MinReservationDays <= reservationDays)
            {
                SuggestedReservationsViewModel viewModel = new SuggestedReservationsViewModel(SelectedAccommodation, AvailableAccommodationPeriods, capacity, user);
                SuggestedReservations suggestedReservationsPage = new SuggestedReservations(viewModel);
                this.NavigationService.Navigate(suggestedReservationsPage);
            }
            else
            {
                MessageBox.Show("Kapacitet ili Minimalan broj dana nisu validni");
            }
        }

        private void SuggestReservation(DateOnly startDate, DateOnly endDate)
        {
            ObservableCollection<AccommodationReservationDTO> sortedAccommodationReservations = SortAccommodationReservations(startDate, endDate);

            FillGapsBetweenReservations(sortedAccommodationReservations);
            FillGapBeforeFirstReservation(sortedAccommodationReservations, startDate);
            FillGapAfterLastReservation(sortedAccommodationReservations, endDate);
            FillEntirePeriodIfNoReservations(sortedAccommodationReservations, startDate, endDate);

            var sortedAvailableAccommodationPeriods = new ObservableCollection<AccommodationReservationDTO>(
                AvailableAccommodationPeriods.OrderBy(reservation => reservation.StartDate));

            AvailableAccommodationPeriods.Clear();
            foreach (var reservation in sortedAvailableAccommodationPeriods)
            {
                AvailableAccommodationPeriods.Add(reservation);
            }

        }

        private void FillGapsBetweenReservations(ObservableCollection<AccommodationReservationDTO> reservations)
        {
            for (int i = 0; i < reservations.Count - 1; i++)
            {
                var gapDays = CalculateGapDays(reservations[i].EndDate, reservations[i + 1].StartDate);

                DateOnly nextStartDate = reservations[i].EndDate.AddDays(1);
                FillGap(nextStartDate, gapDays, i);
            }
        }

        private void FillGapBeforeFirstReservation(ObservableCollection<AccommodationReservationDTO> reservations, DateOnly periodStart)
        {
            if (reservations.Any())
            {
                var firstReservationStartDate = reservations.First().StartDate;
                var gapDays = CalculateGapDays(periodStart, firstReservationStartDate, adjustForInclusiveEndDate: true);

                FillGap(periodStart, gapDays, -1, insertAtBeginning: true);
            }
        }

        private void FillGapAfterLastReservation(ObservableCollection<AccommodationReservationDTO> reservations, DateOnly periodEnd)
        {
            if (reservations.Any())
            {
                var lastReservationEndDate = reservations.Last().EndDate;
                var gapDays = CalculateGapDays(lastReservationEndDate, periodEnd, adjustForInclusiveEndDate: false);

                FillGap(lastReservationEndDate.AddDays(1), gapDays, reservations.Count - 1);
            }
        }

        private void FillEntirePeriodIfNoReservations(ObservableCollection<AccommodationReservationDTO> reservations, DateOnly periodStart, DateOnly periodEnd)
        {
            if (!reservations.Any())
            {
                var gapDays = CalculateGapDays(periodStart, periodEnd, adjustForInclusiveEndDate: true);
                FillGap(periodStart, gapDays, -1);
            }
        }

        private int CalculateGapDays(DateOnly start, DateOnly end, bool adjustForInclusiveEndDate = false)
        {
            return (end.ToDateTime(new TimeOnly(0, 0)) - start.ToDateTime(new TimeOnly(0, 0))).Days + (adjustForInclusiveEndDate ? 1 : 0);
        }

        private void FillGap(DateOnly start, int gapDays, int referenceIndex, bool insertAtBeginning = false)
        {
            while (gapDays >= reservationDays)
            {
                var newReservationDTO = new AccommodationReservationDTO
                {
                    StartDate = start,
                    EndDate = start.AddDays(reservationDays - 1),
                    ReservationDays = reservationDays,
                    Accommodation = SelectedAccommodation.ToAccommodation(),
                    User = user
                };

                if (insertAtBeginning)
                {
                    AvailableAccommodationPeriods.Insert(0, newReservationDTO);
                }
                else
                {
                    AvailableAccommodationPeriods.Add(newReservationDTO);
                }

                start = start.AddDays(reservationDays);
                gapDays -= reservationDays;
            }
        }
        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BeginWindowDrag?.Invoke();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex < Images.Count - 1)
            {
                currentIndex++;
                UpdateImageDisplay();
            }
        }

        private void UpdateImageDisplay()
        {
            ImageList.ItemsSource = new ObservableCollection<string> { Images[currentIndex] };
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateImageDisplay();
            }
        }

        private void OnClick_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    } 
}