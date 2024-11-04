using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;

namespace BookingApp.Service
{
    public class GradeGuestManagmentService
    {
        private GradeAccommodationService gradeAccommodationService;
        private GradeGuestService gradeGuestService;
        public GradeGuestManagmentService()
        {
            gradeAccommodationService = new GradeAccommodationService();
            gradeGuestService = new GradeGuestService();
        }

        public List<GradeGuest> GenerateGuestScores(int userId)
        {
            List<GradeGuest> ratedGuests = new List<GradeGuest>();

            foreach (var gradedGuest in gradeGuestService.GetAll())
            {
                foreach (var gradeAccommodation in gradeAccommodationService.GetAllByUser(userId))
                {
                    if (gradedGuest.AccommodationReservation.Id == gradeAccommodation.AccommodationReservation.Id)
                    {
                        ratedGuests.Add(gradedGuest);
                    }
                }
            }
            return ratedGuests;
        }
    }
}
