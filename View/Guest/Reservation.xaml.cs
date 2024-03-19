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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Reservation.xaml
    /// </summary>
    public partial class Reservation : Window, INotifyPropertyChanged
    {
        public AccommodationDTO SelectedAccommodation { get; set; }
        public AccommodationReservationRepository AccommodationReservationRepository { get; set; }
        public ObservableCollection<AccommodationReservationDTO> AccommodationReservations { get; set; }

        public ObservableCollection<AccommodationReservationDTO> AvailableAccommodationPeriods { get; set; }

        public ObservableCollection<AccommodationReservationDTO> SortedAccommodationReservations { get; set; }

        public Reservation(AccommodationDTO selectedAccommodation, User user)
        {
            InitializeComponent();
            DataContext = this;

            SelectedAccommodation = selectedAccommodation;

            AccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            SortedAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            AccommodationReservationRepository = new AccommodationReservationRepository();

            AvailableAccommodationPeriods = new ObservableCollection<AccommodationReservationDTO>();

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

            AvailableAccommodationPeriods.Clear();
            AvailableAccommodationPeriods =
            FindAllAvailableAccommodationPeriods(StartDate, EndDate, ReservationDays);

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
            SuggestReservation(startDate, endDate);
            if(AvailableAccommodationPeriods.Count == 0)
                SuggestReservation(startDate.AddDays(-5), endDate.AddDays(5));
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
                var gapDays = CalculateGapDays(lastReservationEndDate, periodEnd, adjustForInclusiveEndDate: true);

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
                    User = SelectedAccommodation.User
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
            if (selectedReservation.Accommodation.Capacity >= capacity &&
                selectedReservation.Accommodation.MinReservationDays <= reservationDays)
            {
                selectedReservation.Capacity = capacity;
                AccommodationReservationRepository.Save(selectedReservation.ToAccommodationReservation());
                MessageBox.Show("Uspjesno ste izvrsili rezervaciju");
            }
            else
            {
                MessageBox.Show("Niste zadovoljili sve potrebne parametre");
            }
        }
    }
}