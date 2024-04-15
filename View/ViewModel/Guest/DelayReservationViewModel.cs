using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.Service;
using System.ComponentModel;
using System.Windows;
using CLI.Observer;

namespace BookingApp.View.ViewModel.Guest
{
    public class DelayReservationViewModel : IObserver
    {
        public ObservableCollection<AccommodationReservationDTO> SuggestedReservations { get; set; }
        private SuggestReservationService suggestReservationsService;
        private AccommodationReservationDTO AccommodationReservation { get; set; }

        private int reservationDays;
        public int ReservationDays
        {
            get { return reservationDays; }
            set
            {
                if (value != reservationDays)
                {
                    reservationDays = value;
                    OnPropertyChanged("ReservationDays");
                }
            }
        }

        private DateOnly startDate;
        public DateOnly StartDate
        {
            get { return startDate; }
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
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }

        public DelayReservationViewModel(AccommodationReservationDTO accommodationReservation)
        {
            suggestReservationsService = new SuggestReservationService();
            suggestReservationsService.Subscribe(this);
            SuggestedReservations = new ObservableCollection<AccommodationReservationDTO>();
            AccommodationReservation = accommodationReservation;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FindAllFreeReservation()
        {
            SuggestedReservations = suggestReservationsService.SuggestReservation(reservationDays,
                SuggestedReservations, StartDate, EndDate);

            foreach (var accommodationReservation in SuggestedReservations)
            {
                accommodationReservation.AccommodationName = AccommodationReservation.AccommodationName;
            }
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
