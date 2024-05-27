using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class AccommodationEventRepository : IAccommodationEventRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationEvents.csv";

        private readonly Serializer<AccommodationEvent> _serializer;

        private List<AccommodationEvent> _accommodationEvents;
        public Subject AccommodationEventSubject;
        public void Create(AccommodationEvent accommodationEvent)
        {
            accommodationEvent.Id = NextId();
            _accommodationEvents = _serializer.FromCSV(FilePath);
            _accommodationEvents.Add(accommodationEvent);
            AccommodationEventSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _accommodationEvents);
        }

        public AccommodationEventRepository()
        {
            _serializer = new Serializer<AccommodationEvent>();
            _accommodationEvents = _serializer.FromCSV(FilePath);
            AccommodationEventSubject = new Subject();
        }

        public void Delete(AccommodationEvent entity)
        {
            throw new NotImplementedException();
        }

        public List<AccommodationEvent> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public AccommodationEvent GetById(int key)
        {
            throw new NotImplementedException();
        }
        public List<AccommodationEvent> GetByAccommodationId(int accommodationId)
        {
            List<AccommodationEvent> eventsByAccommodation = new List<AccommodationEvent>();

            foreach (var ev in _accommodationEvents)
            {
                if (ev.Accommodation.Id == accommodationId)
                {
                    eventsByAccommodation.Add(ev);
                }
            }
            return eventsByAccommodation;
        }

        public int NextId()
        {
            _accommodationEvents = _serializer.FromCSV(FilePath);
            if (_accommodationEvents.Count < 1)
            {
                return 1;
            }
            return _accommodationEvents.Max(c => c.Id) + 1;
        }

        public void Update(AccommodationEvent entity)
        {
            throw new NotImplementedException();
        }
    }
}
