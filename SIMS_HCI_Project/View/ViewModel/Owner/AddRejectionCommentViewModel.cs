using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.Owner;
using CLI.Observer;

namespace BookingApp.View.ViewModel.Owner
{
    public class AddRejectionCommentViewModel : IObserver
    {
        private RequestRejectionCommentService requestRejectionCommentService;
        public AddRejectionCommentViewModel()
        {
            requestRejectionCommentService = new RequestRejectionCommentService();
        }
        public void NewComment(DelayReservation delayReservation, string comment)
        {
            RequestRejectionComment requestRejectionComment = new RequestRejectionComment(delayReservation, comment);
            requestRejectionCommentService.Create(requestRejectionComment);
        }
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
