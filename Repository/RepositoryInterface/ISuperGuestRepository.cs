using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using CLI.Observer;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface ISuperGuestRepository : IGenericRepository<SuperGuest, int>
    {
        public void Subscribe(IObserver observer)
        {

        }
    }
}
