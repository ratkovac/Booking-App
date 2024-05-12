using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class AccommodationEventService
    {
        IAccommodationEventRepository _repository;
        public AccommodationEventService()
        {
            _repository = Injector.CreateInstance<IAccommodationEventRepository>();
        }
        public List<AccommodationEvent> GetAll()
        {
            return _repository.GetAll();
        }
        public void Create(AccommodationEvent accommodationEvent)
        {
            _repository.Create(accommodationEvent);
        }
        public List<AccommodationEvent> GetByAccommodationId(int accommodationId)
        {
            return _repository.GetByAccommodationId(accommodationId);
        }
        public void Subscribe(IObserver observer)
        {
            _repository.Subscribe(observer);
        }
    }
}
