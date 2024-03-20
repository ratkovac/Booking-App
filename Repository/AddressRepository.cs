using BookingApp.Model;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class AddressRepository
    {
        private const string FilePath = "../../../Resources/Data/addresses.csv";

        private readonly Serializer<Address> _serializer;

        private List<Address> _addresses;

        public Subject AddressSubject;

        public AddressRepository()
        {
            _serializer = new Serializer<Address>();
            _addresses = _serializer.FromCSV(FilePath);
            AddressSubject = new Subject();
        }

        public List<Address> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Address Save(Address address)
        {
            address.Id = NextId();
            _addresses = _serializer.FromCSV(FilePath);
            _addresses.Add(address);
            AddressSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _addresses);
            return address;
        }

        public int NextId()
        {
            _addresses = _serializer.FromCSV(FilePath);
            if (_addresses.Count < 1)
            {
                return 1;
            }
            return _addresses.Max(c => c.Id) + 1;
        }

        public void Delete(Address address)
        {
            _addresses = _serializer.FromCSV(FilePath);
            Address founded = _addresses.Find(c => c.Id == address.Id);
            if (founded != null)
            {
                _addresses.Remove(founded);
            }
            AddressSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _addresses);
        }

        public Address Update(Address address)
        {
            _addresses = _serializer.FromCSV(FilePath);
            Address current = _addresses.Find(c => c.Id == address.Id);
            int index = _addresses.IndexOf(current);
            _addresses.Remove(current);
            _addresses.Insert(index, address);
            _serializer.ToCSV(FilePath, _addresses);
            AddressSubject.NotifyObservers();
            return address;
        }


        public Address GetAddressById(int id)
        {
            return _addresses.FirstOrDefault(address => address.Id == id);
        }

        public Address GetByAddress(string street)
        {
            _addresses = _serializer.FromCSV(FilePath);
            return _addresses.FirstOrDefault(loc => loc.Street.Equals(street, StringComparison.OrdinalIgnoreCase));
        }

        public void Subscribe(IObserver observer)
        {

        }
    }
}
