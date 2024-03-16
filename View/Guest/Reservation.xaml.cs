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
using System.Windows.Shapes;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Reservation.xaml
    /// </summary>
    public partial class Reservation : Window
    {
        public AccommodationDTO SelectedAccommodation { get; set; }
        public AccommodationReservationRepository AccommodationReservationRepository { get; set; }
        public ObservableCollection<AccommodationReservationDTO> AccommodationReservations { get; set; }
        public Reservation(AccommodationDTO selectedAccommodation, User user)
        {
            InitializeComponent();
            DataContext = this;

            SelectedAccommodation = selectedAccommodation;

            AccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            AccommodationReservationRepository = new AccommodationReservationRepository();

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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}