using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Repository;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourInstanceService
    {
        private ITourInstanceRepository tourInstanceRepository;
        private TourRepository tourRepository;

        public TourInstanceService()
        {
            tourInstanceRepository = Injector.CreateInstance<ITourInstanceRepository>();
            tourRepository = new TourRepository();
            InitializeTour();
        }
        private void InitializeTour()
        {
            foreach (var item in tourInstanceRepository.GetAll())
            {
                item.Tour = tourRepository.GetById(item.TourId);
            }
        }
        public List<TourInstance> GetToursWhichFinished()
        {
            List<TourInstance> toursFinished = new List<TourInstance>();
            List<TourInstance> allTourInstances = GetAll();

            foreach (TourInstance tourInstance in allTourInstances)
            {
                if (tourInstance.IsFinished)
                {
                    toursFinished.Add(tourInstance);
                }
            }

            return toursFinished;
        }
        public List<TourInstance> GetAll()
        {
            return tourInstanceRepository.GetAll();
        }
        public void Create(TourInstance tourInstances)
        {
            tourInstanceRepository.Create(tourInstances);
        }
        public void Delete(TourInstance tourInstances)
        {
            tourInstanceRepository.Delete(tourInstances);
        }
        public void Update(TourInstance tourInstances)
        {
            tourInstanceRepository.Update(tourInstances);
        }
        public void Subscribe(IObserver observer)
        {
            tourInstanceRepository.Subscribe(observer);
        }
    }
}
