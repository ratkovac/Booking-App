using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BookingApp.DTO;
using BookingApp.GUI_Elements;
using BookingApp.Model;
using BookingApp.Service;

namespace BookingApp.View.ViewModel.Guest
{
    public class SuggestedReservationsViewModel
    {
        public ObservableCollection<AccommodationReservationDTO> Reservations { get; set; }
        public AccommodationReservationDTO? SelectedReservation { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }

        private int capacity;
        private User user;

        private AccommodationReservationService accommodationReservationService;
        private SuperGuestManagmentService superGuestManagmentService;
        public SuggestedReservationsViewModel(AccommodationDTO selectedAccommodation, ObservableCollection<AccommodationReservationDTO> reservations, int capacity, User user)
        {
            Reservations = new ObservableCollection<AccommodationReservationDTO>();
            Reservations = reservations;
            SelectedReservation = new AccommodationReservationDTO();
            SelectedAccommodation = selectedAccommodation;
            accommodationReservationService = new AccommodationReservationService();
            superGuestManagmentService = new SuperGuestManagmentService();
            this.capacity = capacity;
            this.user = user;
        }

        public void ReserveReservation(object sender, MouseButtonEventArgs e)
        {
            var contentControl = sender as ContentControl;
            if (contentControl != null)
            {
                var reservation = contentControl.DataContext as AccommodationReservationDTO;
                if (reservation != null)
                {
                    ItemsControlExtensions.SetSelectedItem(contentControl, reservation);
                    reservation.Capacity = capacity;
                    reservation.User = user;
                    accommodationReservationService.Add(reservation.ToAccommodationReservation());
                    superGuestManagmentService.SubtractBonusPoints(reservation.User.Id);
                    Reservations.Remove(reservation);
                }
            }
        }
    }
}
