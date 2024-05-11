using BookingApp.Model;
using System;
using System.ComponentModel;
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
                    OnPropertyChanged("accommodationName");
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
        private bool busy;

        public bool Busy
        {
            get { return busy; }
            set
            {
                if (value != busy)
                {
                    busy = value;
                    OnPropertyChanged(nameof(Busy));
                }
            }
        }
        private string frontImagePath;
        public string FrontImagePath
        {
            get
            {
                return frontImagePath;
            }
            set
            {
                if (frontImagePath != value)
                {
                    frontImagePath = value;
                    OnPropertyChanged("FrontImagePath");
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
            oldStartDate = delayReservation.Reservation.StartDate;
            oldEndDate = delayReservation.Reservation.EndDate;
            newStartDate = delayReservation.NewStartDate; 
            newEndDate = delayReservation.NewEndDate;
            reservationId = delayReservation.Reservation.Id;
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