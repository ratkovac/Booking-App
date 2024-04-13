using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;

namespace BookingApp.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();
        public Accommodation Save(Accommodation accommodation);
        public void Delete(Accommodation accommodation);
        public Accommodation Update(Accommodation accommodation);
        public Accommodation? GetByID(int accommodationId);
    }
}
