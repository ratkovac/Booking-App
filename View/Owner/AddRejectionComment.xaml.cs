using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.ViewModel.Guest;
using BookingApp.View.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.View.Owner
{
    public partial class AddRejectionComment : Window
    {
        private AddRejectionCommentViewModel addRejectionCommentViewModel;
        private DelayReservationService delayReservationService;
        private DelayReservationDTO selectedRequest;
        public AddRejectionComment(AddRejectionCommentViewModel addRejectionCommentViewModel, DelayReservationDTO delayReservationDTO)
        {
            InitializeComponent();
            this.DataContext = addRejectionCommentViewModel;
            this.addRejectionCommentViewModel = addRejectionCommentViewModel;
            selectedRequest = delayReservationDTO;
            delayReservationService = new DelayReservationService();
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string comment = Comment.Text;
            DelayReservation dr = delayReservationService.GetByID(selectedRequest.Id);

            addRejectionCommentViewModel.NewComment(dr, comment);
            this.Close();
        }
    }
}
