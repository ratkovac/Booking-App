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
        public List<int> AllYears(int accommodationId)
        {
            HashSet<int> uniqueYears = new HashSet<int>();
            foreach (var eventt in GetByAccommodationId(accommodationId))
            {
                uniqueYears.Add(eventt.EventDate.Year);
            }
            List<int> years = uniqueYears.ToList();
            return years;
        }
        public int getStatisticByYear(int year, EventEnum.EventType type, int accommodationId)
        {
            int counter = 0;
            foreach (var reserved in GetAll())
            {
                if (reserved.Accommodation.Id == accommodationId)
                {
                    if (reserved.EventDate.Year == year && reserved.EventType.Equals(type))
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }
        public void Subscribe(IObserver observer)
        {
            _repository.Subscribe(observer);
        }
    }
}
