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
using System.Windows.Media.Imaging;

namespace BookingApp.View.Owner
{
    public partial class DelayRequests : Window
    {
        public DelayReservationDTO selectedRequest { get; set; }
        private DelayReservationService delayReservationService;
        private AccommodationReservationService accommodationReservationService;
        public User LoggedInUser;


        public DelayRequests(DelayRequestsViewModel delayRequestsViewModel)
        {
            InitializeComponent();
            this.DataContext = delayRequestsViewModel;
        }
   

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}