using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DTO;

namespace BookingApp.View.ViewModel.Guest
{
    public class SuggestedReservationsViewModel
    {
        public ObservableCollection<AccommodationReservationDTO> Reservations { get; set; }
        public AccommodationReservationDTO? SelectedReservation { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }
        public SuggestedReservationsViewModel(AccommodationDTO selectedAccommodation, ObservableCollection<AccommodationReservationDTO> reservations)
        {
            Reservations = new ObservableCollection<AccommodationReservationDTO>();
            Reservations = reservations;
            SelectedReservation = new AccommodationReservationDTO();
            SelectedAccommodation = selectedAccommodation;
        }
    }
}
