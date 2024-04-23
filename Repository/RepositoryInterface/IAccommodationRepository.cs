using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface IAccommodationRepository : IGenericRepository<Accommodation, int>
    {
        public void Subscribe(IObserver observer)
        {

        }
    }
}
