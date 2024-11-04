using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using CLI.Observer;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface IAccommodationReservationRepository : IGenericRepository<AccommodationReservation, int>
    {
        public List<AccommodationReservation> GetAllByUser(int userId);
        public List<AccommodationReservation> GetAllByID(int accommodationId);
        public void Subscribe(IObserver observer)
        {

        }
    }
}