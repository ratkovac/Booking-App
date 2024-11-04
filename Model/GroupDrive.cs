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
        public int DriveStatusId { get; set; }
        public double Delay { get; set; }
        public int DriverId { get; set; }
        public int LanguageId { get; set; }
        public int NumberOfPeople { get; set; }

        public GroupDrive()
        {
        }

        public GroupDrive(int startAddressId, int endAddressId, DateTime date, DateTime timeOfReservation, int guestId, int driveStatusId, double delay, int driverId, int languageId, int numberOfPeople)
        {
            StartAddressId = startAddressId;
            EndAddressId = endAddressId;
            Date = date;
            TimeOfReservation = timeOfReservation;
            GuestId = guestId;
            DriveStatusId = driveStatusId;
            Delay = delay;
            DriverId = driverId;
            LanguageId = languageId;
            NumberOfPeople = numberOfPeople;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                StartAddressId.ToString(),
                EndAddressId.ToString(),
                Date.ToString(),
                TimeOfReservation.ToString(),
                GuestId.ToString(),
                DriveStatusId.ToString(),
                Delay.ToString(),
                DriverId.ToString(),
                LanguageId.ToString(),
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
            DriverId = Convert.ToInt32(values[8]);
            LanguageId = Convert.ToInt32(values[9]);
            NumberOfPeople = Convert.ToInt32(values[10]);
        }
    }
}
