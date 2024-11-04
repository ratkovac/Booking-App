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
        private ITourRepository tourRepository;

        public TourInstanceService()
        {
            tourInstanceRepository = Injector.CreateInstance<ITourInstanceRepository>();
            tourRepository = Injector.CreateInstance<ITourRepository>();
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
                if (tourInstance.State == TourInstanceState.Finished)
                {
                    toursFinished.Add(tourInstance);
                }
            }

            return toursFinished;
        }
        public List<TourInstance> GetInactiveToursByUser(int userId)
        {
            return tourInstanceRepository.GetInactiveToursByUser(userId);
        }

        public List<TourInstance> GetFinishedTourInstances()
        {
            return tourInstanceRepository.GetFinishedTourInstances();
        }

        public TourInstance GetById(int id)
        {
            return tourInstanceRepository.GetById(id);
        }

        public List<TourInstance> GetByUserId(int userId)
        {
            return tourInstanceRepository.GetByUserId(userId);
        }

        public List<TourInstance> GetFinishedByUserId(int userId)
        {
            return tourInstanceRepository.GetFinishedByUserId(userId);
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
