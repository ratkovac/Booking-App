using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;

namespace BookingApp.Service
{
    public class SuperGuestService
    {
        private ISuperGuestRepository _superGuestRepository;

        public SuperGuestService()
        {
            _superGuestRepository = Injector.CreateInstance<ISuperGuestRepository>();
        }

        public List<SuperGuest> GetAll()
        {
            return _superGuestRepository.GetAll();
        }

        public SuperGuest GetById(int id)
        {
            return _superGuestRepository.GetById(id);
        }

        public void Add(SuperGuest superGuest)
        {
            _superGuestRepository.Create(superGuest);
        }

        public void Delete(SuperGuest superGuest)
        {
            _superGuestRepository.Delete(superGuest);
        }
        public void Update(SuperGuest superGuest)
        {
            _superGuestRepository.Update(superGuest);
        }

        public void Subscribe(IObserver observer)
        {
            _superGuestRepository.Subscribe(observer);
        }
    }
}
