using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Domain.RepositoryInterface;
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
            InitializeGuest();
            InitializeTourInstance();
            InitializeReservation();
        }

        private void InitializeGuest()
        {
            foreach (var item in tourReservationRepository.GetAll())
            {
                item.Tourist = touristRepository.GetById(item.TouristId);
            }
        }

        private void InitializeReservation()
        {
            foreach (var item in tourGuestRepository.GetAll())
            {
                item.TourReservation = tourReservationRepository.GetById(item.TourReservationId);
            }
        }

        private void InitializeTourInstance()
        {
            foreach (var item in tourReservationRepository.GetAll())
            {
                item.TourInstance = tourInstanceRepository.GetById(item.TourInstanceId);
            }
        }

        public List<TourGuest> GetAll()
        {
            return tourGuestRepository.GetAll();
        }

    }
}
