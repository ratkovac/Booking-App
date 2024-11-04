using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface ITourRequestSegmentRepository : IGenericRepository<TourRequestSegment, int>
    {
        public List<TourRequestSegment> GetAllComplexSegmentsByTourRequestId(int tourRequestId);
        public List<TourRequestSegment> GetAllNonComplexRequests();
        public List<TourRequestSegment> GetAllComplexRequests();
        public void Subscribe(IObserver observer)
        {

        }
    }
}
