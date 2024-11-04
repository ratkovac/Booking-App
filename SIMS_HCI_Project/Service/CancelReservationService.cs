using BookingApp.Repository.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DependencyInjection;
using BookingApp.Model;
using CLI.Observer;

namespace BookingApp.Service
{
    public class CancelReservationService
    {
        private ICanceledReservationRepository _repository;

        public CancelReservationService()
        {
            _repository = Injector.CreateInstance<ICanceledReservationRepository>();
        }

        public List<CanceledReservation> GetAll()
        {
            return _repository.GetAll();
        }

        public void Create(CanceledReservation canceledReservation)
        {
            _repository.Create(canceledReservation);
        }

        public void Subscribe(IObserver observer)
        {
            _repository.Subscribe(observer);
        }
    }
}
