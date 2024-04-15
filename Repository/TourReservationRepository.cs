using BookingApp.Model;
using BookingApp.Domain.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourReservationRepository : ITourReservationRepository
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

        public void Update(TourReservation tourReservation)
        {
            int index = _tourReservations.FindIndex(t => tourReservation.Id == t.Id);
            if (index != -1)
            {
                _tourReservations[index] = tourReservation;
                _serializer.ToCSV(FilePath, _tourReservations);
            }
        }

        public TourReservation GetById(int id)
        {
            return _tourReservations.Find(r => r.Id == id);
        }

        public List<TourReservation> GetAllByUserId(int userId)
        {
            return _serializer.FromCSV(FilePath).Where(tr => tr.TouristId == userId).ToList();
        }

        public List<TourReservation> GetAllByTourInstanceId(int tourInstanceId)
        {
            return _serializer.FromCSV(FilePath).Where(tr => tr.TourInstanceId == tourInstanceId).ToList();
        }

        public List<TourReservation> GetReservationsByTourInstance(TourInstance tourInstance)
        {
            return _tourReservations.Where(r => r.TourInstanceId == tourInstance.Id).ToList();
        }
        public List<TourReservation> GetReservationsByTourInstanceAndState(TourInstance tourInstance, TouristState state)
        {
            return _tourReservations.Where(r => r.State == state && r.TourInstanceId == tourInstance.Id).ToList();
        }
        public List<int> GetTouristIdsByTourInstanceAndState(TourInstance tourInstance, TouristState state)
        {
            List<TourReservation> wantedReservations = _tourReservations.Where(r => r.State == state && r.TourInstanceId == tourInstance.Id).ToList();
            List<int> guestIds = wantedReservations.Select(r => r.TouristId).ToList();
            return guestIds;
        }
        public TourReservation GetTourInstanceIdWhereTouristIsWaiting(Tourist tourist)
        {
            TourReservation reservation = _tourReservations.Find(r => r.TouristId == tourist.Id && r.State == TouristState.ActiveTour);
            return reservation;
        }
        public TourReservation GetReservationByTouristAndTourInstance(TourInstance tourInstance, Tourist tourist)
        {
            TourReservation tourReservation = _tourReservations.Find(r => r.TouristId == tourist.Id && r.TourInstanceId == tourInstance.Id);
            return tourReservation;
        }
        public List<int> FindTourInstanceIdsWhereTouristPresent(int touristId)
        {
            List<int> tourIds = new List<int>();
            foreach (TourReservation reservation in GetAll())
            {
                if (reservation.TouristId == touristId && reservation.State == TouristState.Present && reservation.RatedTour != true)

                {
                    tourIds.Add(reservation.TourInstanceId);
                }
            }
            return tourIds;
        }
        public List<TourReservation> GetToursWhichFinished()
        {
            List<TourReservation> toursFinished = new List<TourReservation>();
            List<TourReservation> allTourInstances = GetAll();

            foreach (TourReservation tourReservation in allTourInstances)
            {
                if (tourReservation.TourInstance.State == TourInstanceState.Finished && tourReservation.RatedTour == false)
                {
                    toursFinished.Add(tourReservation);
                }
            }

            return toursFinished;
        }
    }

}