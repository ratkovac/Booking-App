using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;

namespace BookingApp.Service
{
    public class AccommodationService
    {
        IAccommodationRepository _repository;
        public AccommodationService()
        {
            _repository = Injector.CreateInstance<IAccommodationRepository>();
        }

        public Accommodation GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Subscribe(IObserver observer)
        {
            _repository.Subscribe(observer);
        }
    }
}
