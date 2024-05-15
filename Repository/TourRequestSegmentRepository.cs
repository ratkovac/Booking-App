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
    public class TourRequestSegmentRepository : ITourRequestSegmentRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequestSegments.csv";

        private readonly Serializer<TourRequestSegment> _serializer;
        private List<IObserver> observers;
        private List<TourRequestSegment> _tourRequestSegments;


        public TourRequestSegmentRepository()
        {
            _serializer = new Serializer<TourRequestSegment>();
            _tourRequestSegments = _serializer.FromCSV(FilePath);
        }

        public TourRequestSegment Save(TourRequestSegment tourRequestSegment)
        {
            tourRequestSegment.Id = NextId();
            _tourRequestSegments.Add(tourRequestSegment);
            _serializer.ToCSV(FilePath, _tourRequestSegments);
            return tourRequestSegment;
        }

        public int NextId()
        {
            if (_tourRequestSegments.Count == 0)
            {
                return 0;
            }
            return _tourRequestSegments.Max(trs => trs.Id) + 1;
        }

        public void Create(TourRequestSegment tourRequestSegment)
        {
            tourRequestSegment.Id = NextId();
            _tourRequestSegments.Add(tourRequestSegment);
            _serializer.ToCSV(FilePath, _tourRequestSegments);
        }

        public void Delete(TourRequestSegment tourRequestSegment)
        {
            TourRequestSegment found = _tourRequestSegments.Find(trs => trs.Id == tourRequestSegment.Id);
            _tourRequestSegments.Remove(found);
            _serializer.ToCSV(FilePath, _tourRequestSegments);
        }

        public void Update(TourRequestSegment tourRequestSegment)
        {
            int index = _tourRequestSegments.FindIndex(trs => tourRequestSegment.Id == trs.Id);
            if (index != -1)
            {
                _tourRequestSegments[index] = tourRequestSegment;
                _serializer.ToCSV(FilePath, _tourRequestSegments);
            }
        }

        public List<TourRequestSegment> GetAll()
        {
            return _tourRequestSegments;
        }

        public TourRequestSegment GetById(int id)
        {
            return _tourRequestSegments.Find(trs => trs.Id == id);
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
