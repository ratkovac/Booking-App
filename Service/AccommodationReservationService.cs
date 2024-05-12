using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;

namespace BookingApp.Service
{
    public class AccommodationReservationService
    {
        private IAccommodationReservationRepository _repository;
        public AccommodationReservationService()
        {
            _repository = Injector.CreateInstance<IAccommodationReservationRepository>();
        }

        public List<AccommodationReservation> GetAllByUser(int userId)
        {
            return _repository.GetAllByUser(userId);
        }
        public List<AccommodationReservation> GetAll()
        {
            return _repository.GetAll();
        }

        public AccommodationReservation GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            _repository.Delete(accommodationReservation);
        }
        public void Update(AccommodationReservation AccommodationReservation)
        {
            _repository.Update(AccommodationReservation);
        }

        public void Add(AccommodationReservation entity)
        {
            _repository.Create(entity);
        }

        public void Subscribe(IObserver observer)
        {
            _repository.Subscribe(observer);
        }
    }
}
