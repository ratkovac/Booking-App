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
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequests.csv";

        private readonly Serializer<TourRequest> _serializer;
        private List<IObserver> observers;
        private List<TourRequest> _tourRequests;


        public TourRequestRepository()
        {
            _serializer = new Serializer<TourRequest>();
            _tourRequests = _serializer.FromCSV(FilePath);
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            _tourRequests.Add(tourRequest);
            _serializer.ToCSV(FilePath, _tourRequests);
            return tourRequest;
        }

        public int NextId()
        {
            if (_tourRequests.Count == 0)
            {
                return 0;
            }
            return _tourRequests.Max(tr => tr.Id) + 1;
        }

        public void Create(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            _tourRequests.Add(tourRequest);
            _serializer.ToCSV(FilePath, _tourRequests);
        }

        public void Delete(TourRequest tourRequest)
        {
            TourRequest found = _tourRequests.Find(tr => tr.Id == tourRequest.Id);
            _tourRequests.Remove(found);
            _serializer.ToCSV(FilePath, _tourRequests);
        }

        public void Update(TourRequest tourRequest)
        {
            int index = _tourRequests.FindIndex(tr => tourRequest.Id == tr.Id);
            if (index != -1)
            {
                _tourRequests[index] = tourRequest;
                _serializer.ToCSV(FilePath, _tourRequests);
            }
        }

        public List<TourRequest> GetAll()
        {
            return _tourRequests;
        }

        public TourRequest GetById(int id)
        {
            return _tourRequests.Find(tr => tr.Id == id);
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
