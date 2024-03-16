using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourReservationRepository
    {

        private const string FilePath = "../../../Resources/Data/tourReservations.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> _tourReservations;

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();
            _tourReservations = _serializer.FromCSV(FilePath);
        }

        public List<TourReservation> GetAll()
        {
            return _tourReservations;
        }

        public TourReservation Save(TourReservation TourReservation)
        {
            TourReservation.Id = NextId();
            _tourReservations.Add(TourReservation);
            _serializer.ToCSV(FilePath, _tourReservations);
            return TourReservation;
        }

        public int NextId()
        {
            if (_tourReservations.Count == 0)
            {
                return 0;
            }
            return _tourReservations.Max(t => t.Id) + 1;
        }

        public void Create(TourReservation TourReservation)
        {
            TourReservation.Id = NextId();
            _tourReservations.Add(TourReservation);
            _serializer.ToCSV(FilePath, _tourReservations);
        }

        public void Delete(TourReservation TourReservation)
        {
            TourReservation founded = _tourReservations.Find(c => c.Id == TourReservation.Id);
            if (founded != null)
            {
                _tourReservations.Remove(founded);
            }
            _serializer.ToCSV(FilePath, _tourReservations);
        }

        public TourReservation Update(TourReservation TourReservation)
        {
            int index = _tourReservations.FindIndex(t => TourReservation.Id == t.Id);
            if (index != -1)
            {
                _tourReservations[index] = TourReservation;
                _serializer.ToCSV(FilePath, _tourReservations);
            }
            return TourReservation;
        }

        public TourReservation GetById(int id)
        {
            return _tourReservations.Find(r => r.Id == id);
        }

        public List<TourReservation> GetReservationsByTour(Tour tour)
        {
            return _tourReservations.Where(r => r.TourId == tour.Id).ToList();
        }
    }

}