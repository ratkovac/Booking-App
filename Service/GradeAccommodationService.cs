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
    public class GradeAccommodationService
    {
        private IGradeAccommodationRepository _repository;
        public GradeAccommodationService()
        {
            _repository = Injector.CreateInstance<IGradeAccommodationRepository>();
        }

        public List<GradeAccommodation> GetAll()
        {
            return _repository.GetAll();
        }
        public List<GradeAccommodation> GetAllByUser(int userId)
        {
            return _repository.GetAllByUser(userId);
        }
        public void Subscribe(IObserver observer)
        {
            _repository.Subscribe(observer);
        }
    }
}
