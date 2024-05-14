using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterface
{
    public interface ITourInstanceRepository : IGenericRepository<TourInstance, int>
    {
        public List<TourInstance> GetAllTourInstancesByTour(Tour tour);

        public void Subscribe(IObserver observer)
        {

        }
    }
}
