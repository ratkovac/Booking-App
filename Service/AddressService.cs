using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class AddressService
    {
        private IAddressRepository addressRepository;

        public AddressService()
        {
            addressRepository = Injector.CreateInstance<IAddressRepository>();
        }
        public int NextId()
        {
            return addressRepository.NextId();
        }
        public List<Address> GetAll()
        {
            return addressRepository.GetAll();
        }
        public Address GetById(int id)
        {
            return addressRepository.GetById(id);
        }
        public void Create(Address address)
        {
            addressRepository.Create(address);
        }
        public void Delete(Address address)
        {
            addressRepository.Delete(address);
        }
        public void Update(Address address)
        {
            addressRepository.Update(address);
        }
        public void Subscribe(IObserver observer)
        {
            addressRepository.Subscribe(observer);
        }
        public Address GetByAddress(string street, string number)
        {
            return addressRepository.GetByAddress(street, number);
        }
        public int? GetLocationIdByAddressId(int addressId)
        {
            return addressRepository.GetLocationIdByAddressId(addressId);
        }
    }
}
