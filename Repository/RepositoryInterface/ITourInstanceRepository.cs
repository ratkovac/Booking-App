using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface ITourInstanceRepository : IGenericRepository<TourInstance, int>
    {
        public List<TourInstance> GetAllTourInstancesByTour(Tour tour);
        public List<TourInstance> GetFinishedTourInstances();
        public List<TourInstance> GetByUserId(int userId);
        public List<TourInstance> GetFinishedByUserId(int userId);
        public List<TourInstance> GetInactiveToursByUser(int userId);

        public void Subscribe(IObserver observer)
        {

        }
    }
}
