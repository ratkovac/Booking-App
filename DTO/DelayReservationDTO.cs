using BookingApp.Model;
using BookingApp.View.Owner;
using System;
using System.ComponentModel;
using BookingApp.View;

namespace BookingApp.DTO
{
    public class DelayReservationDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public int Id { get; set; }

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

        private AccommodationReservation reservation;

        public AccommodationReservation Reservation
        {
            get
            {
                return reservation;
            }
            set
            {
                if (value != reservation)
                {
                    reservation = value;
                    OnPropertyChanged("Reservation");
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
        private DateOnly oldStartDate;
        public DateOnly OldStartDate
        {
            get
            {
                return oldStartDate;
            }
            set
            {
                if (value != oldStartDate)
                {
                    oldStartDate = value;
                    OnPropertyChanged("OldStartDate");
                }
            }
        }
        private DateOnly oldEndDate;
        public DateOnly OldEndDate
        {
            get
            {
                return oldEndDate;
            }
            set
            {
                if (value != oldEndDate)
                {
                    oldEndDate = value;
                    OnPropertyChanged("OldEndDate");
                }
            }
        }
        private DateOnly newStartDate;
        public DateOnly NewStartDate
        {
            get
            {
                return newStartDate;
            }
            set
            {
                if (value != newStartDate)
                {
                    newStartDate = value;
                    OnPropertyChanged("NewStartDate");
                }
            }
        }
        private DateOnly newEndDate;
        public DateOnly NewEndDate
        {
            get
            {
                return newEndDate;
            }
            set
            {
                if (value != newEndDate)
                {
                    newEndDate = value;
                    OnPropertyChanged("NewEndDate");
                }
            }
        }
        private int reservationId;
        public int ReservationId
        {
            get
            {
                return reservationId;
            }
            set
            {
                if (value != reservationId)
                {
                    reservationId = value;
                    OnPropertyChanged("Reservation");
                }
            }
        }

        private bool read;
        public bool Read
        {
            get
            {
                return read;
            }
            set
            {
                if (value != read)
                {
                    read = value;
                    OnPropertyChanged("Read");
                }
            }
        }

        private DelayReservationStatusEnum delayReservationStatus;

        public DelayReservationStatusEnum DelayReservationStatus
        {
            get
            {
                return delayReservationStatus;
            }
            set
            {
                if (value != delayReservationStatus)
                {
                    delayReservationStatus = value;
                    OnPropertyChanged("DelayReservationStatus");
                }
            }
        }
        public DelayReservationDTO()
        {
        }
        public DelayReservationDTO(DelayReservation delayReservation)
        {
            Id = delayReservation.Id;
            accommodationName = delayReservation.Reservation.Accommodation.Name;
            reservation = delayReservation.Reservation;
            oldStartDate = delayReservation.Reservation.StartDate;
            oldEndDate = delayReservation.Reservation.EndDate;
            newStartDate = delayReservation.NewStartDate; 
            newEndDate = delayReservation.NewEndDate;
            reservationId = delayReservation.Reservation.Id;
        }

        public DelayReservation ToDelayReservation()
        {
            DelayReservation delayReservation = new DelayReservation(reservation, newStartDate, newEndDate);
            return delayReservation;
        }
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}