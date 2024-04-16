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
        public List<DelayReservation> GetAll()
        {
            return _repository.GetAll();
        }
        public void Subscribe(IObserver observer)
        {
            _repository.Subscribe(observer);
        }

        public List<DelayReservation> GetAll()
        {
            return _repository.GetAll();
        public DelayReservation? GetByID(int delayReservationId)
        {
            return _repository.GetById(delayReservationId);
        }
        public void Update(DelayReservation delayReservation)
        {
            _repository.Update(delayReservation);
        }
    }
}
