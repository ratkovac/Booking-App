using BookingApp.Domain.RepositoryInterface;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface IRenovationRepository : IGenericRepository<Renovations,int>
    {
    }
}
