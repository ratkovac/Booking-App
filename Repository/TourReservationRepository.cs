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

        public TourReservation Save(TourReservation tourReservation)
        {
            tourReservation.Id = NextId();
            _tourReservations.Add(tourReservation);
            _serializer.ToCSV(FilePath, _tourReservations);
            return tourReservation;
        }

        public int NextId()
        {
            if (_tourReservations.Count < 1)
            {
                return 1;
            }
            return _tourReservations.Max(t => t.Id) + 1;
        }

        public void Create(TourReservation tourReservation)
        {
            tourReservation.Id = NextId();
            _tourReservations.Add(tourReservation);
            _serializer.ToCSV(FilePath, _tourReservations);
        }

        public void Delete(TourReservation tourReservation)
        {
            TourReservation found = _tourReservations.Find(c => c.Id == tourReservation.Id);
            if (found != null)
            {
                _tourReservations.Remove(found);
            }
            _serializer.ToCSV(FilePath, _tourReservations);
        }

        public TourReservation Update(TourReservation tourReservation)
        {
            int index = _tourReservations.FindIndex(t => tourReservation.Id == t.Id);
            if (index != -1)
            {
                _tourReservations[index] = tourReservation;
                _serializer.ToCSV(FilePath, _tourReservations);
            }
            return tourReservation;
        }

        public TourReservation GetByID(int id)
        {
            return _tourReservations.Find(r => r.Id == id);
        }

        public List<TourReservation> GetAllByUserId(int userId)
        {
            return _serializer.FromCSV(FilePath).Where(tr => tr.UserId == userId).ToList();
        }

        public List<TourReservation> GetAllByTourInstanceId(int tourInstanceId)
        {
            return _serializer.FromCSV(FilePath).Where(tr => tr.TourInstanceId == tourInstanceId).ToList();
        }
    }

}