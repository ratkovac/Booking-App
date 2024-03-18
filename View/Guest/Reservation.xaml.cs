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

        public Reservation(AccommodationDTO selectedAccommodation, User user)
        {
            InitializeComponent();
            DataContext = this;

            SelectedAccommodation = selectedAccommodation;

            AccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
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

        private ObservableCollection<AccommodationReservationDTO> FindAllAvailableAccommodationPeriods()
        {
            ObservableCollection<AccommodationReservationDTO> availableAccommodationPeriods =
                new ObservableCollection<AccommodationReservationDTO>();

            ObservableCollection<AccommodationReservationDTO> sortedAccommodationReservations = SortAccommodationReservations();

            for (int i = 0; i < sortedAccommodationReservations.Count - 1; i++)
            {

                var currentReservation = sortedAccommodationReservations[i];
                var nextReservation = sortedAccommodationReservations[i + 1];

                DateTime currentReservationEndDateTime = currentReservation.EndDate.ToDateTime(new TimeOnly(0, 0));
                DateTime nextReservationStartDateTime = nextReservation.StartDate.ToDateTime(new TimeOnly(0, 0));

                //MessageBox.Show($"Start Date: {currentReservationEndDateTime:yyyy-MM-dd}, End Date: {nextReservationStartDateTime:yyyy-MM-dd}");

                TimeSpan gap = nextReservationStartDateTime - currentReservationEndDateTime;
                int gapDays = gap.Days;
                MessageBox.Show($"Gap days: {gapDays}");

                if (gapDays >= reservationDays)
                {
                    AccommodationReservationDTO newReservationDTO = new AccommodationReservationDTO(sortedAccommodationReservations[i].ToAccommodationReservation());

                    newReservationDTO.StartDate = currentReservation.EndDate.AddDays(1);
                    newReservationDTO.EndDate = currentReservation.EndDate.AddDays(gapDays);
                    newReservationDTO.ReservationDays = gapDays;

                    availableAccommodationPeriods.Add(newReservationDTO);
                    MessageBox.Show(newReservationDTO.ToString());
                }
            }
            if (sortedAccommodationReservations.Any())
            {
                var lastReservation = sortedAccommodationReservations.Last();
                if (endDate.DayNumber - lastReservation.EndDate.AddDays(1).DayNumber >= reservationDays)
                {
                    AccommodationReservationDTO newReservationDTO = new AccommodationReservationDTO(lastReservation.ToAccommodationReservation());

                    newReservationDTO.StartDate = lastReservation.EndDate.AddDays(1);
                    newReservationDTO.EndDate = lastReservation.EndDate.AddDays(reservationDays);
                    newReservationDTO.ReservationDays = reservationDays;

                    availableAccommodationPeriods.Add(newReservationDTO);
                }
            }
            if (sortedAccommodationReservations.Count == 0)
            {
                AccommodationReservationDTO newReservationDTO = new AccommodationReservationDTO();

                newReservationDTO.StartDate = startDate;
                newReservationDTO.EndDate = startDate.AddDays(reservationDays);
                newReservationDTO.ReservationDays = reservationDays;

                availableAccommodationPeriods.Add(newReservationDTO);
            }

            return availableAccommodationPeriods;
        }

        private ObservableCollection<AccommodationReservationDTO> SortAccommodationReservations()
        {

            var sorted = AccommodationReservations.OrderBy(reservation => reservation.StartDate).ToList();

            AccommodationReservations.Clear();

            foreach (var reservation in sorted)
            {
                AccommodationReservations.Add(reservation);
            }

            return AccommodationReservations;

        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<AccommodationReservationDTO> sortedAccommodationReservations = SortAccommodationReservations();

            for (int i = 0; i < sortedAccommodationReservations.Count - 1; i++)
            {

                var currentReservation = sortedAccommodationReservations[i];
                var nextReservation = sortedAccommodationReservations[i + 1];

                DateTime currentReservationEndDateTime = currentReservation.EndDate.ToDateTime(new TimeOnly(0, 0));
                DateTime nextReservationStartDateTime = nextReservation.StartDate.ToDateTime(new TimeOnly(0, 0));


                TimeSpan gap = nextReservationStartDateTime - currentReservationEndDateTime;
                int gapDays = gap.Days;

                while (gapDays - reservationDays >= 0)
                {
                    if (gapDays >= reservationDays)
                    {
                        AccommodationReservationDTO newReservationDTO = new AccommodationReservationDTO(sortedAccommodationReservations[i].ToAccommodationReservation());

                        newReservationDTO.StartDate = currentReservation.EndDate.AddDays(1);
                        newReservationDTO.EndDate = currentReservation.EndDate.AddDays(reservationDays);
                        newReservationDTO.ReservationDays = reservationDays;

                        currentReservation.EndDate = currentReservation.EndDate.AddDays(reservationDays);

                        AvailableAccommodationPeriods.Add(newReservationDTO);
                    }
                    gapDays -= reservationDays;
                }
            }
            if (sortedAccommodationReservations.Any())
            {
                var lastReservation = sortedAccommodationReservations.Last();
                if (endDate.DayNumber - lastReservation.EndDate.AddDays(1).DayNumber >= reservationDays)
                {
                    AccommodationReservationDTO newReservationDTO = new AccommodationReservationDTO(lastReservation.ToAccommodationReservation());

                    newReservationDTO.StartDate = lastReservation.EndDate.AddDays(1);
                    newReservationDTO.EndDate = lastReservation.EndDate.AddDays(reservationDays);
                    newReservationDTO.ReservationDays = reservationDays;

                    AvailableAccommodationPeriods.Add(newReservationDTO);
                }
            }
            if (sortedAccommodationReservations.Count == 0)
            {
                AccommodationReservationDTO newReservationDTO = new AccommodationReservationDTO();

                newReservationDTO.StartDate = startDate;
                newReservationDTO.EndDate = startDate.AddDays(reservationDays);
                newReservationDTO.ReservationDays = reservationDays;
                newReservationDTO.AccommodationName = SelectedAccommodation.Name;
                newReservationDTO.UserName = SelectedAccommodation.User.Username;

                AvailableAccommodationPeriods.Add(newReservationDTO);
            }

        }
    }
}