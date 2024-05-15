using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface IAddressRepository : IGenericRepository<Address, int>
    {

        public int? GetLocationIdByAddressId(int addressId);
        public Address GetByAddress(string street, string number);
        public void Subscribe(IObserver observer)
        {

        }
    }
}
