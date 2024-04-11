using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;

namespace BookingApp.Service
{
    public class GradeTourService
    {
        private IGradeTourRepository gradeTourRepository;
        private UserRepository userRepository;
        private TourInstanceRepository tourInstanceRepository;

        public GradeTourService()
        {
            gradeTourRepository = Injector.CreateInstance<IGradeTourRepository>();
            userRepository = new UserRepository();
            tourInstanceRepository = new TourInstanceRepository();
            InitializeTourist();
            InitializeTour();
        }
        private void InitializeTourist()
        {
            foreach (var item in gradeTourRepository.GetAll())
            {
                item.Tourist = userRepository.GetByID(item.TouristId);
            }
        }
        private void InitializeTour()
        {
            foreach (var item in gradeTourRepository.GetAll())
            {
                item.TourInstance = tourInstanceRepository.GetById(item.Id);
            }
        }
        public List<GradeTour> GetAll()
        {
            return gradeTourRepository.GetAll();
        }
        public List<GradeTour> GetAllRatingsByTour(TourInstance tourInstance)
        {
            return gradeTourRepository.GetAllRatingsByTour(tourInstance);
        }
        public void Create(GradeTour gradeTours)
        {
            gradeTourRepository.Create(gradeTours);
        }
        public void ReportRating(GradeTour gradeTours)
        {
            gradeTours.IsValid = false;
            gradeTourRepository.Update(gradeTours);
        }
        public void Delete(GradeTour gradeTours)
        {
            gradeTourRepository.Delete(gradeTours);
        }
        public void Update(GradeTour gradeTours)
        {
            gradeTourRepository.Update(gradeTours);
        }
        public void Subscribe(IObserver observer)
        {
            gradeTourRepository.Subscribe(observer);
        }
    }
}
