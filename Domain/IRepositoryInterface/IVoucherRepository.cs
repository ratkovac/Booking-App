using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.RepositoryInterface
{
    public interface IVoucherRepository : IGenericRepository<Voucher, int>
    {
        public List<Voucher> GetWithIds(List<int> ids);

        public void Subscribe(IObserver observer)
        {

        }
    }
}
