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
    public class DelayReservationService
    {
        private IDelayReservationRepository _repository;
        public DelayReservationService()
        {
            _repository = Injector.CreateInstance<IDelayReservationRepository>();
        }

        public void Create(DelayReservation delayReservation)
        {
            _repository.Create(delayReservation);
        }

        public void Subscribe(IObserver observer)
        {
            _repository.Subscribe(observer);
        }
    }
}
