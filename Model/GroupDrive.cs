using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class GroupDrive : ISerializable
    {
        public int Id { get; set; }
        public DateTime TimeOfReservation { get; set; }
        public int StartAddressId { get; set; }
        public int EndAddressId { get; set; }
        public DateTime Date { get; set; }
        public int GuestId { get; set; }
        public User Guest { get; set; }
        public int DriveStatusId { get; set; }
        public double Delay { get; set; }
        public int DriverId { get; set; }
        public string LanguageName { get; set; }
        public int NumberOfPeople { get; set; }


        public GroupDrive()
        {
        }

        public GroupDrive(int startAddressId, int endAddressId, DateTime date, DateTime timeOfReservation, User guest, int driveStatusId, double delay, int driverId, string languageName, int numberOfPeople)
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
            LanguageName = languageName;
            NumberOfPeople = numberOfPeople;
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
                DriverId.ToString(),
                LanguageName,
                NumberOfPeople.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 11)
            {
                throw new ArgumentException("Neispravan format CSV podataka.");
            }
            Id = Convert.ToInt32(values[0]);

            StartAddressId = Convert.ToInt32(values[1]);
            EndAddressId = Convert.ToInt32(values[2]);

            Date = Convert.ToDateTime(values[3]);
            TimeOfReservation = Convert.ToDateTime(values[4]);
            GuestId = Convert.ToInt32(values[5]);
            DriveStatusId = Convert.ToInt32(values[6]);
            Delay = Convert.ToDouble(values[7]);
            UserRepository userRepository = new UserRepository();
            Guest = userRepository.GetByID(GuestId);
            DriverId = Convert.ToInt32(values[8]);

            LanguageName = values[9];
            NumberOfPeople = Convert.ToInt32(values[10]);
        }
    }
}
