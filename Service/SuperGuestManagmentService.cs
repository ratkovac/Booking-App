using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;

namespace BookingApp.Service
{
    public class SuperGuestManagmentService
    {
        private AccommodationReservationService accommodationReservationService;
        private SuperGuestService superGuestService;

        public SuperGuestManagmentService()
        {
            accommodationReservationService = new AccommodationReservationService();
            superGuestService = new SuperGuestService();
        }

        private List<AccommodationReservation> AddExpiredReservations(int userId)
        {
            List<AccommodationReservation> reservations = accommodationReservationService.GetAll();

            return reservations.Where(reservation => IsReservationExpiredLastYear(reservation, userId)).ToList();
        }

        private bool IsReservationExpiredLastYear(AccommodationReservation reservation, int userId)
        {
            return reservation.User.Id == userId &&
                   ((reservation.EndDate.AddYears(1).ToDateTime(new TimeOnly(0, 0)) > DateOnly.FromDateTime(DateTime.Today).ToDateTime(new TimeOnly(0, 0))));
        }


        public bool AddSuperGuest(int userId)
        {
            List<AccommodationReservation> expiredReservations = AddExpiredReservations(userId);

            DeleteSuperGuestIfYearPassed(userId);

            bool isAlreadySuperGuest = superGuestService.GetAll().Any(superguest => superguest.UserId == userId);
            if (isAlreadySuperGuest)
            {
                return true;
            }

            if (expiredReservations.Count >= 10)
            {
                DateOnly todayDateOnly = DateOnly.FromDateTime(DateTime.Today);
                SuperGuest superGuest = new SuperGuest(userId, todayDateOnly, todayDateOnly.AddDays(365), 5);
                superGuestService.Add(superGuest);
                return true;
            }

            return false;
        }

        private void DeleteSuperGuestIfYearPassed(int userId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var superGuestsToDelete = superGuestService.GetAll()
                .Where(sg => sg.UserId == userId && sg.StartDate.AddDays(365) < today)
                .ToList();

            foreach (var superGuest in superGuestsToDelete)
            {
                superGuestService.Delete(superGuest);
            }
        }

        public void SubtractBonusPoints(int userId)
        {
            var superGuests = superGuestService.GetAll()
                .Where(sg => sg.UserId == userId && sg.BonusPoens > 0)
                .ToList();

            foreach (var superGuest in superGuests)
            {
                superGuest.BonusPoens--;
                superGuestService.Update(superGuest);
            }
        }
    }
}
