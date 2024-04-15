using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourGuestService
    {
        private ITourReservationRepository tourReservationRepository;
        private ITouristRepository touristRepository;
        private ITourInstanceRepository tourInstanceRepository;
        private ITourGuestRepository tourGuestRepository;

        public TourGuestService()
        {
            tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            touristRepository = Injector.CreateInstance<ITouristRepository>();
            tourInstanceRepository = Injector.CreateInstance<ITourInstanceRepository>();
            tourGuestRepository = Injector.CreateInstance<ITourGuestRepository>();
        }

        public List<TourGuest> GetAll()
        {
            return tourGuestRepository.GetAll();
        }

        public TourGuest Save(TourGuest tourGuest)
        {
            return tourGuestRepository.Save(tourGuest);
        }

        public void SaveMultiple(List<TourGuest> tourGuests)
        {
            tourGuestRepository.SaveMultiple(tourGuests);
        }

        public void Delete(TourGuest tourGuest)
        {
            tourGuestRepository.Delete(tourGuest);
        }

        public void Update(TourGuest tourGuest)
        {
            tourGuestRepository.Update(tourGuest);
        }

        public TourGuest GetById(int id)
        {
            return tourGuestRepository.GetById(id);
        }

        public List<TourGuest> GetByTouristAndReservationId(int tourReservationId, int touristId)
        {
            return tourGuestRepository.GetByTouristAndReservationId(tourReservationId, touristId);
        }
        public List<TourGuest> GetAllByTouristId(int touristId)
        {
            return tourGuestRepository.GetAllByTouristId(touristId);
        }

        public List<TourGuest> GetAllPresentByTourReservationId(int tourReservationId)
        {
            return tourGuestRepository.GetAllPresentByTourReservationId(tourReservationId);
        }

        public List<TourGuest> GetAllByTourInstanceId(int tourInstanceId)
        {
            return tourGuestRepository.GetAllByTourInstanceId(tourInstanceId);
        }

        public List<TourGuest> GetAllByTourReservationId(int tourReservationId)
        {
            return tourGuestRepository.GetAllByTourReservationId(tourReservationId);
        }

        public int GetTouristNumberByTourReservationId(int tourReservationId)
        {
            return tourGuestRepository.GetTouristNumberByTourReservationId(tourReservationId);
        }


    }
}
