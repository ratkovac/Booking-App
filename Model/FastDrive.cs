using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class FastDrive : ISerializable
    {
        public int Id { get; set; }
        public DateTime TimeOfReservation { get; set; }
        public int StartAddressId { get; set; }
        public Address StartAddress { get; set; }
        public int EndAddressId { get; set; }
        public Address EndAddress { get; set; }
        public DateTime Date { get; set; }
        public int GuestId { get; set; }
        public User Guest { get; set; }
        public int DriveStatusId { get; set; }
        public double Delay { get; set; }
        public User Driver { get; set; }
        public int DriverId { get; set; }


        public FastDrive()
        {
        }

        public FastDrive(int startAddressId, int endAddressId, DateTime date, DateTime timeOfReservation, User guest, int driveStatusId, double delay, int driverId)
        {
            StartAddressId = startAddressId;
            EndAddressId = endAddressId;
            Date = date;
            TimeOfReservation = timeOfReservation;
            Guest = guest;
            GuestId = guest.Id;
            DriveStatusId = driveStatusId;
            Delay = delay;
            DriverId = driverId;
        }

        public string[] ToCSV()
        {
            string guestId = Guest.Id.ToString();
            string[] csvValues = {
                Id.ToString(),
                StartAddressId.ToString(),
                EndAddressId.ToString(),
                Date.ToString(),
                TimeOfReservation.ToString(),
                guestId,
                DriveStatusId.ToString(),
                Delay.ToString(),
                DriverId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 9)
            {
                throw new ArgumentException("Neispravan format CSV podataka.");
            }
            Id = Convert.ToInt32(values[0]);

            StartAddressId = Convert.ToInt32(values[1]);
            AddressRepository addressRepository = new AddressRepository();
            StartAddress = addressRepository.GetAddressById(StartAddressId);

            EndAddressId = Convert.ToInt32(values[2]);
            EndAddress = addressRepository.GetAddressById(EndAddressId);


            Date = Convert.ToDateTime(values[3]);
            TimeOfReservation = Convert.ToDateTime(values[4]);
            GuestId = Convert.ToInt32(values[5]);
            DriveStatusId = Convert.ToInt32(values[6]);
            Delay = Convert.ToDouble(values[7]);
            UserRepository userRepository = new UserRepository();
            Guest = userRepository.GetByID(GuestId);
            DriverId = Convert.ToInt32(values[8]);
            Driver = userRepository.GetByID(DriverId);
        }
    }
}
