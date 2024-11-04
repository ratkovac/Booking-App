using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.View.Owner;
using CLI.Observer;

namespace BookingApp.Repository
{
    public class CanceledReservationRepository : ICanceledReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/canceledReservation.csv";

        private readonly Serializer<CanceledReservation> _serializer;

        private List<CanceledReservation> _canceledReservations;

        public Subject AccommodationReservationSubject;

        public CanceledReservationRepository()
        {
            _serializer = new Serializer<CanceledReservation>();
            _canceledReservations = _serializer.FromCSV(FilePath);
            AccommodationReservationSubject = new Subject();
        }

        public List<CanceledReservation> GetAll()
        {
            return _canceledReservations;
        }
        
        public int NextId()
        {
            _canceledReservations = _serializer.FromCSV(FilePath);
            if (_canceledReservations.Count < 1)
            {
                return 1;
            }
            return _canceledReservations.Max(c => c.Id) + 1;
        }

        public void Create(CanceledReservation canceledReservation)
        {
            canceledReservation.Id = NextId();
            _canceledReservations = _serializer.FromCSV(FilePath);
            _canceledReservations.Add(canceledReservation);
            _serializer.ToCSV(FilePath, _canceledReservations);
        }

        void IGenericRepository<CanceledReservation, int>.Update(CanceledReservation entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(CanceledReservation canceledReservation)
        {
            _canceledReservations = _serializer.FromCSV(FilePath);
            CanceledReservation founded = _canceledReservations.Find(c => c.Id == canceledReservation.Id);
            if (founded != null)
            {
                _canceledReservations.Remove(founded);
            }
            _serializer.ToCSV(FilePath, _canceledReservations);
        }

        public CanceledReservation GetById(int key)
        {
            return _canceledReservations.Find(c => c.Id == key);
        }

        public CanceledReservation Update(CanceledReservation canceledReservation)
        {
            _canceledReservations = _serializer.FromCSV(FilePath);
            CanceledReservation current = _canceledReservations.Find(c => c.Id == canceledReservation.Id);
            int index = _canceledReservations.IndexOf(current);
            _canceledReservations.Remove(current);
            _canceledReservations.Insert(index, canceledReservation);
            _serializer.ToCSV(FilePath, _canceledReservations);
            return canceledReservation;
        }


    }
}
