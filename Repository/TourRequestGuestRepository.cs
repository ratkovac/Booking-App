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
    public class TourRequestGuestRepository : ITourRequestGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequestGuests.csv";

        private readonly Serializer<TourRequestGuest> _serializer;
        private List<IObserver> observers;
        private List<TourRequestGuest> _tourRequestGuests;


        public TourRequestGuestRepository()
        {
            _serializer = new Serializer<TourRequestGuest>();
            _tourRequestGuests = _serializer.FromCSV(FilePath);
        }

        public TourRequestGuest Save(TourRequestGuest tourRequestGuest)
        {
            tourRequestGuest.Id = NextId();
            _tourRequestGuests.Add(tourRequestGuest);
            _serializer.ToCSV(FilePath, _tourRequestGuests);
            return tourRequestGuest;
        }

        public int NextId()
        {
            if (_tourRequestGuests.Count == 0)
            {
                return 0;
            }
            return _tourRequestGuests.Max(trg => trg.Id) + 1;
        }

        public void Create(TourRequestGuest tourRequestGuest)
        {
            tourRequestGuest.Id = NextId();
            _tourRequestGuests.Add(tourRequestGuest);
            _serializer.ToCSV(FilePath, _tourRequestGuests);
        }

        public void Delete(TourRequestGuest tourRequestGuest)
        {
            TourRequestGuest found = _tourRequestGuests.Find(trg => trg.Id == tourRequestGuest.Id);
            _tourRequestGuests.Remove(found);
            _serializer.ToCSV(FilePath, _tourRequestGuests);
        }

        public void Update(TourRequestGuest tourRequestGuest)
        {
            int index = _tourRequestGuests.FindIndex(trg => tourRequestGuest.Id == trg.Id);
            if (index != -1)
            {
                _tourRequestGuests[index] = tourRequestGuest;
                _serializer.ToCSV(FilePath, _tourRequestGuests);
            }
        }

        public List<TourRequestGuest> GetAll()
        {
            return _tourRequestGuests;
        }

        public TourRequestGuest GetById(int id)
        {
            return _tourRequestGuests.Find(trg => trg.Id == id);
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }
    }
}
