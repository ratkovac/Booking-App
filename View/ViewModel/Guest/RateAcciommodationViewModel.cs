using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.NGuest;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Wpf;

namespace BookingApp.View.ViewModel.Guest
{
    public class RateAcciommodationViewModel : IObserver
    {
        public ObservableCollection<AccommodationReservationDTO> RateAccommodations { get; private set; }
        private AccommodationReservation accommodationReservation { get; set; }
        public AccommodationReservationDTO? SelectedReservation { get; set; }
        private AccommodationReservationService accommodationReservationService;
        private User user { get; set; }

        public RateAcciommodationViewModel(User user)
        {
            accommodationReservationService = new AccommodationReservationService();
            accommodationReservationService.Subscribe(this);
            RateAccommodations = new ObservableCollection<AccommodationReservationDTO>();
            SelectedReservation = new AccommodationReservationDTO();
            this.user = user;
            Update();
            
        }
        public void Update()
        {
            RateAccommodations.Clear();
            DateTime today = DateTime.Today;

            foreach (var accommodationReservation in accommodationReservationService.GetAllByUser(user.Id))
            {
                DateTime reservationEndDate = new DateTime(accommodationReservation.EndDate.Year, accommodationReservation.EndDate.Month, accommodationReservation.EndDate.Day);
                if ((today - reservationEndDate).Days < 5 && (today - reservationEndDate).Days > 0)
                {
                    RateAccommodations.Add(new AccommodationReservationDTO(accommodationReservation));
                }
            }
        }

        public void OnClickRate()
        {
            RateViewModel rateViewModel = new RateViewModel(SelectedReservation.ToAccommodationReservation());
            Rate rate = new Rate(rateViewModel);
            rate.Show();
        }
    }
}
