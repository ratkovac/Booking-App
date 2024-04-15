using BookingApp.Model;
using BookingApp.View;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface ITourReservationRepository : IGenericRepository<TourReservation, int>
    {
        public List<TourReservation> GetReservationsByTourInstance(TourInstance tourInstance);
        public List<TourReservation> GetReservationsByTourInstanceAndState(TourInstance tourInstance, TouristState state);
        public TourReservation GetReservationByTouristAndTourInstance(TourInstance tourInstance, Tourist guest);
        public TourReservation GetTourInstanceIdWhereTouristIsWaiting(Tourist tourist);
        public List<TourReservation> GetAllByTourInstanceId(int tourInstanceId);
        public void Subscribe(IObserver observer)
        {

        }
    }
}
