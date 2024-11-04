using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class YearAccommodationStatisticDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public int Id { get; set; }

        private int year;
        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                if (value != year)
                {
                    year = value;
                    OnPropertyChanged("Year");
                }
            }
        }
        private int numberOfReservations;
        public int NumberOfReservations
        {
            get
            {
                return numberOfReservations;
            }
            set
            {
                if (value != numberOfReservations)
                {
                    numberOfReservations = value;
                    OnPropertyChanged("NumberOfReservations");
                }
            }
        }

        private int numberOfCancelledReservations;
        public int NumberOfCancelledReservations
        {
            get
            {
                return numberOfCancelledReservations;
            }
            set
            {
                if (value != numberOfCancelledReservations)
                {
                    numberOfCancelledReservations = value;
                    OnPropertyChanged("NumberOfCancelledReservations");
                }
            }
        }
        private int numberOfMovedReservations;
        public int NumberOfMovedReservations
        {
            get
            {
                return numberOfMovedReservations;
            }
            set
            {
                if (value != numberOfMovedReservations)
                {
                    numberOfMovedReservations = value;
                    OnPropertyChanged("NumberOfMovedReservations");
                }
            }
        }
        private int numberOfRenovationProposal;
        public int NumberOfRenovationProposal
        {
            get
            {
                return numberOfRenovationProposal;
            }
            set
            {
                if (value != numberOfRenovationProposal)
                {
                    numberOfRenovationProposal = value;
                    OnPropertyChanged("NumberOfRenovationProposal");
                }
            }
        }
        public YearAccommodationStatisticDTO()
        {
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
