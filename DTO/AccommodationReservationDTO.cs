using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class AccommodationReservationDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id { get; set; }

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
                    OnPropertyChanged("AccommodationId");
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
                    OnPropertyChanged("accommodationName");
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

        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (value != userName)
                {
                    userName = value;
                    OnPropertyChanged("userName");
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

        private DateOnly endDate;
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
        private double userGrade;
        public double UserGrade
        {
            get
            {
                return userGrade;
            }
            set
            {
                if (value != userGrade)
                {
                    userGrade = value;
                    OnPropertyChanged("UserGrade");
                }
            }
        }
        private double accommodationGrade;
        public double AccommodationGrade
        {
            get
            {
                return accommodationGrade;
            }
            set
            {
                if (value != accommodationGrade)
                {
                    accommodationGrade = value;
                    OnPropertyChanged("AccommoationGrade");
                }
            }
        }
        private int daysToRating;

        public int DaysToRating
        {
            get
            {
                return daysToRating;
            }
            set
            {
                if (value != daysToRating)
                {
                    daysToRating = value;
                    OnPropertyChanged("daysToRating");
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
                    OnPropertyChanged("Capacity");
                }
            }
        }


        public AccommodationReservationDTO()
        {
        }
        public AccommodationReservationDTO(AccommodationReservation accommodationReservation)
        {
            Id = accommodationReservation.Id;
            accommodation = accommodationReservation.Accommodation;
            user = accommodationReservation.User;
            startDate = accommodationReservation.StartDate;
            endDate = accommodationReservation.EndDate;
            reservationDays = accommodationReservation.ReservationDays;
            userGrade = accommodationReservation.UserGrade;
            accommodationGrade = accommodationReservation.AccommodationGrade;
            userName = accommodationReservation.User.Username;
            accommodationName = accommodationReservation.Accommodation.Name;
            capacity = accommodationReservation.Capacity;

        }
        public AccommodationReservation ToAccommodationReservation()
        {
            AccommodationReservation accommodationReservation = new AccommodationReservation(Id, accommodation, user, startDate, endDate, reservationDays, userGrade, accommodationGrade, capacity);
            return accommodationReservation;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
