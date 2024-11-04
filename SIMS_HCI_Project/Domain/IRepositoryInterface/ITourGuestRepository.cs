using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterface
{
    public interface ITourGuestRepository : IGenericRepository<TourGuest, int>
    {
        public int GetTouristNumberByTour(int tourReservationId);

    }
}
