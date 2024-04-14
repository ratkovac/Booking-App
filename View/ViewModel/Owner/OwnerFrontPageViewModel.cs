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

namespace BookingApp.View.ViewModel.Owner
{
    public class OwnerFrontPageViewModel : IObserver
    {
        private User LoggedInUser;
        private AccommodationReservationService accommodationReservationService;
        private List<Double> accommoationGrades = new List<Double>();
        public bool SuperOwner(User user)
        {
            foreach (AccommodationReservation ar in accommodationReservationService.GetAll())
            {
                if (ar.AccommodationGrade != 0 && ar.Accommodation.User.Id == user.Id)
                {
                    accommoationGrades.Add(ar.AccommodationGrade);
                }
            }
            double avarageAccommodationGrade = AvarageAccommoationGrade();
            if (accommoationGrades.Count >= 50 && avarageAccommodationGrade >= 4.5)
                return true;
            else
                return false;
        }

        private double AvarageAccommoationGrade()
        {
            double sum = 0;
            int len = 0;
            double avg = 0;
            foreach (Double aag in accommoationGrades)
            {
                sum += aag;
            }
            len = accommoationGrades.Count;
            avg = sum / len;
            return avg;
        }
        public OwnerFrontPageViewModel(User user)
        {
            accommodationReservationService = new AccommodationReservationService();
            LoggedInUser = user;
            
        }
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
