using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.View.Owner
{
    public partial class DelayRequests : Window
    {
        public DelayReservationDTO selectedRequest { get; set; }
        private DelayReservationService delayReservationService;
        private AccommodationReservationService accommodationReservationService;
        public DelayRequests(DelayRequestsViewModel delayRequestsViewModel)
        {
            InitializeComponent();
            delayReservationService = new DelayReservationService();
            accommodationReservationService = new AccommodationReservationService();
            this.DataContext = delayRequestsViewModel;
            DelayRequestsGrid.ItemsSource = delayRequestsViewModel.allRequests();
            Decline.IsEnabled = false;
            Accept.IsEnabled = false;
        }

        
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Decline_Click(object sender, RoutedEventArgs e)
        {
            DelayReservation oldDelayReservation = delayReservationService.GetByID(selectedRequest.Id);
            oldDelayReservation.Status = DelayReservationStatusEnum.Declined;
            delayReservationService.Update(oldDelayReservation);
            MessageBox.Show("Declined request!");
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            AccommodationReservation accommodationReservation = accommodationReservationService.GetById(selectedRequest.ReservationId);
            accommodationReservation.StartDate = selectedRequest.NewStartDate;
            accommodationReservation.EndDate = selectedRequest.NewEndDate;
            DelayReservation oldDelayReservation = delayReservationService.GetByID(selectedRequest.Id);
            oldDelayReservation.Status = DelayReservationStatusEnum.Approved;
            accommodationReservationService.Update(accommodationReservation);
            delayReservationService.Update(oldDelayReservation);
            MessageBox.Show("Accepted request!");
        }

        private void DelayRequestsGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Decline.IsEnabled = true;
            Accept.IsEnabled = true;
            selectedRequest = (DelayReservationDTO)DelayRequestsGrid.SelectedItem;
        }
    }
}
