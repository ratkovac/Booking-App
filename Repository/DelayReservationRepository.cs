using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.View.NGuest;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class DelayReservationRepository : IDelayReservationRepository 
    {
        private const string FilePath = "../../../Resources/Data/delayReservations.csv";

        private readonly Serializer<DelayReservation> _serializer;

        private List<DelayReservation> _delayReservations;
        public Subject DelayReservationSubject;

        public DelayReservationRepository()
        {
            _serializer = new Serializer<DelayReservation>();
            _delayReservations = _serializer.FromCSV(FilePath);
            DelayReservationSubject = new Subject();
        }

        public List<DelayReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }


        public int NextId()
        {
            _delayReservations = _serializer.FromCSV(FilePath);
            if (_delayReservations.Count < 1)
            {
                return 1;
            }
            return _delayReservations.Max(c => c.Id) + 1;
        }

        public void Create(DelayReservation entity)
        {
            entity.Id = NextId();
            _delayReservations = _serializer.FromCSV(FilePath);
            _delayReservations.Add(entity);
            DelayReservationSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _delayReservations);
        }

        void IGenericRepository<DelayReservation, int>.Update(DelayReservation entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(DelayReservation delayReservation)
        {
            _delayReservations = _serializer.FromCSV(FilePath);
            DelayReservation founded = _delayReservations.Find(c => c.Id == delayReservation.Id);
            if (founded != null)
            {
                _delayReservations.Remove(founded);
            }
            DelayReservationSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _delayReservations);
        }

        public DelayReservation GetById(int key)
        {
            throw new NotImplementedException();
        }

        public DelayReservation Update(DelayReservation delayReservation)
        {
            _delayReservations = _serializer.FromCSV(FilePath);
            DelayReservation current = _delayReservations.Find(c => c.Id == delayReservation.Id);
            int index = _delayReservations.IndexOf(current);
            _delayReservations.Remove(current);
            _delayReservations.Insert(index, delayReservation);
            _serializer.ToCSV(FilePath, _delayReservations);
            DelayReservationSubject.NotifyObservers();
            return delayReservation;
        }
        /*public List<DelayReservation> GetByUser(User user)
        {
            _delayReservations = _serializer.FromCSV(FilePath);
            return _delayReservations.FindAll(c => c.User.Id == user.Id);
        }*/
        public DelayReservation? GetByID(int delayReservationId)
        {
            return _delayReservations.Find(c => c.Id == delayReservationId);

        }
    }
}
