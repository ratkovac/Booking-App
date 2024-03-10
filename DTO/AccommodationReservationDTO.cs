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

        private int accommodationId;
        public int AccommodationId
        {
            get 
            { 
                return accommodationId;
            }
            set
            {
                if(value !=  accommodationId)
                {
                    accommodationId = value;
                    OnPropertyChanged("AccommodationId");
                }
            }
        }
        private int userId;
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                if(value != userId)
                {
                    userId = value;
                    OnPropertyChanged("UserId");
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
                if(value !=  startDate)
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
                if(value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }

        public AccommodationReservationDTO(AccommodationReservation accommodationReservation)
        {
            Id = accommodationReservation.Id;
            accommodationId = accommodationReservation.AccommodationId;
            userId = accommodationReservation.UserId;
            startDate = accommodationReservation.StartDate;
            endDate = accommodationReservation.EndDate;
        }
        public AccommodationReservation ToAccommodationReservation()
        {
            AccommodationReservation accommodationReservation = new AccommodationReservation(Id, accommodationId, userId, startDate, endDate);
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
