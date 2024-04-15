using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.NGuest;
using CLI.Observer;

namespace BookingApp.View.ViewModel.Guest
{
    public class MyReservationViewModel : IObserver
    {
        public ObservableCollection<AccommodationReservationDTO> MyReservations { get; private set; }
        private AccommodationReservation accommodationReservation { get; set; }
        private User user { get; set; }
        public AccommodationReservationDTO? SelectedReservation { get; set; }
        private AccommodationReservationService accommodationReservationService;

        public MyReservationViewModel(User user)
        {
            this.user = user;
            accommodationReservationService = new AccommodationReservationService();
            accommodationReservationService.Subscribe(this);
            MyReservations = new ObservableCollection<AccommodationReservationDTO>();
            SelectedReservation = new AccommodationReservationDTO();
            Update();
        }
        public void Update()
        {
            MyReservations.Clear();
            foreach (var accommodationReservation in accommodationReservationService.GetAllByUser(user.Id))
            {
                MyReservations.Add(new AccommodationReservationDTO(accommodationReservation));
            }
        }
        public void OnClickDelay()
        {
            DelayReservationViewModel delayReservationViewModel = new DelayReservationViewModel(SelectedReservation);
            DelayReservations delayReservations = new DelayReservations(delayReservationViewModel);
            delayReservations.Show();
        }
    }
}
