using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface ITourGuestRepository : IGenericRepository<TourGuest, int>
    {
        public int GetTouristNumberByTourReservationId(int tourReservationId);
        void SaveMultiple(List<TourGuest> tourGuests);
        List<TourGuest> GetAllByTourReservationId(int tourReservationId);
        List<TourGuest> GetAllByTourInstanceId(int tourInstanceId);
        List<TourGuest> GetAllPresentByTourReservationId(int tourReservationId);
        List<TourGuest> GetAllByTouristId(int touristId);
        List<TourGuest> GetByTouristAndReservationId(int tourReservationId, int touristId);

        TourGuest Save(TourGuest tourGuest);

    }
}
