using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.View.Owner;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Repository.RepositoryInterface;

namespace BookingApp.Repository
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {

        private const string FilePath = "../../../Resources/Data/reservation.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;

        public Subject AccommodationReservationSubject;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservationSubject = new Subject();
        }

        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservations;
        }
        public AccommodationReservation Save(AccommodationReservation AccommodationReservation)
        {
            AccommodationReservation.Id = NextId();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(AccommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return AccommodationReservation;
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }

        public void Create(AccommodationReservation entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(AccommodationReservation AccommodationReservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation founded = _accommodationReservations.Find(c => c.Id == AccommodationReservation.Id);
            if (founded != null)
            {
                _accommodationReservations.Remove(founded);
            }
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }


        public void Update(AccommodationReservation AccommodationReservation)
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            AccommodationReservation current = _accommodationReservations.Find(c => c.Id == AccommodationReservation.Id);
            int index = _accommodationReservations.IndexOf(current);
            _accommodationReservations.Remove(current);
            _accommodationReservations.Insert(index, AccommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public List<AccommodationReservation> GetAllByID(int accommodationId)
        {
            List<AccommodationReservation> reservationsByAccommodation = new List<AccommodationReservation>();

            foreach (var reservation in _accommodationReservations)
            {
                if (reservation.Accommodation.Id == accommodationId)
                {
                    reservationsByAccommodation.Add(reservation);
                }
            }

            return reservationsByAccommodation;

        }
        public AccommodationReservation GetByID(int key)
        {
            return _accommodationReservations.Find(c => c.Id == key);
        }
        public AccommodationReservation GetById(int key)
        {
            return _accommodationReservations.Find(c => c.Id == key);
        }

        public List<AccommodationReservation> GetAllByUser(int userId)
        {
            List<AccommodationReservation> reservationsByUser = new List<AccommodationReservation>();

            foreach (var reservation in _accommodationReservations)
            {
                if (reservation.User.Id == userId)
                {
                    reservationsByUser.Add(reservation);
                }
            }

            return reservationsByUser;
        }
    }
}