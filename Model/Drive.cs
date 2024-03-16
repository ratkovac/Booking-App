using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Drive
    {
        public int Id { get; set; } 

        public int DriverId { get; set; }

        public User Driver { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public DateTime Date { get; set; }

        public int GuestId { get; set; }

        public User Guest { get; set; }

        public Drive()
        {
        }

        public Drive(int id, int driverId, User driver, int addressId, DateTime date, int guestId)
        {
            Id = id;
            DriverId = driverId;
            Driver = driver;
            AddressId = addressId;
            Date = date;
            GuestId = guestId;
        }

        public string[] ToCSV()
        {
            string driverId = Driver.Id.ToString();
            string addressId = Address.Id.ToString();
            string guestId = Guest.Id.ToString();  
            string[] csvValues = { Id.ToString(), driverId, addressId, Date.ToString(), guestId };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 5)
            {
                throw new ArgumentException("Neispravan format CSV podataka.");
            }
            Id = Convert.ToInt32(values[0]);
            DriverId = Convert.ToInt32(values[1]);
            UserRepository userRepository = new UserRepository();
            Driver = userRepository.GetById(DriverId);

            AddressId = Convert.ToInt32(values[2]);
            AddressRepository addressRepository = new AddressRepository();
            Address = addressRepository.GetAddressById(AddressId);

            Date = Convert.ToDateTime(values[3]);
            GuestId = Convert.ToInt32(values[4]);
            Guest = userRepository.GetById(GuestId);

        }
    }
}
