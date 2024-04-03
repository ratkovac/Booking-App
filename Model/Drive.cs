using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Drive: ISerializable
    {
        public int Id { get; set; } 
        public int DriverId { get; set; }
        public User Driver { get; set; }
        public int StartAddressId { get; set; }
        public Address StartAddress { get; set; }
        public int EndAddressId { get; set; }
        public Address EndAddress { get; set; }
        public DateTime Date { get; set; }
        public int GuestId { get; set; }
        public User Guest { get; set; }
        public int DriveStatusId { get; set; }
        public double Delay { get; set; }

        public Drive()
        {
        }

        public Drive(int id, int driverId, User driver, int startAddressId,int endAddressId, DateTime date, int guestId)
        {
            Id = id;
            DriverId = driverId;
            Driver = driver;
            StartAddressId = startAddressId;
            EndAddressId = endAddressId;
            Date = date;
            GuestId = guestId;
        }

        public Drive(Address startAddress, Address endAddress, DateTime date, User driver, User guest, int driveStatusId, double delay)
        {
            StartAddress = startAddress;
            EndAddress = endAddress;
            Date = date;
            Driver = driver;
            Guest = guest;
            DriveStatusId = driveStatusId;
            Delay = delay;
        }

        public Drive(int startAddressId, int endAddressId, DateTime date, int guestId, int driveStatusId, double delay)
        {
            StartAddressId = startAddressId;
            EndAddressId = endAddressId;
            Date = date;
            GuestId = guestId;
            DriveStatusId = driveStatusId;
            Delay = delay;
        }

        public Drive(int id, User driver, Address startAddress, Address endAddress, DateTime date, User guest)
        {
            Id = id;
            DriverId = driver.Id;
            Driver = driver;
            StartAddressId = startAddress.Id;
            StartAddress = startAddress;
            EndAddressId = endAddress.Id;
            EndAddress = endAddress;
            Date = date;
            GuestId = guest.Id;
            Guest = guest;
        }

        public string[] ToCSV()
        {
            string driverId = Driver.Id.ToString();
            string startAddressId = StartAddress.Id.ToString();
            string endAddressId = EndAddress.Id.ToString();
            string guestId = Guest.Id.ToString();  
            string[] csvValues = {
                Id.ToString(),
                driverId,
                startAddressId,
                endAddressId,
                Date.ToString(),
                guestId,
                DriveStatusId.ToString(),
                Delay.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 8)
            {
                throw new ArgumentException("Neispravan format CSV podataka.");
            }
            Id = Convert.ToInt32(values[0]);
            DriverId = Convert.ToInt32(values[1]);
            UserRepository userRepository = new UserRepository();
            Driver = userRepository.GetByID(DriverId);

            StartAddressId = Convert.ToInt32(values[2]);
            AddressRepository addressRepository = new AddressRepository();
            StartAddress = addressRepository.GetAddressById(StartAddressId);

            EndAddressId = Convert.ToInt32(values[3]);
            EndAddress = addressRepository.GetAddressById(EndAddressId);


            Date = Convert.ToDateTime(values[4]);
            GuestId = Convert.ToInt32(values[5]);
            DriveStatusId = Convert.ToInt32(values[6]);
            Delay = Convert.ToDouble(values[7]);
            Guest = userRepository.GetByID(GuestId);

        }
    }
}
