using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterface
{
    public interface ITouristRepository : IGenericRepository<Tourist, int>
    {
        public Tourist GetByUserId(int userId);

        public void Subscribe(IObserver observer)
        {

        }
    }
}
