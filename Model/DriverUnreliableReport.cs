using BookingApp.Serializer;
using BookingApp.View.NGuest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class DriverUnreliableReport : ISerializable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DriveId { get; set; }
        public int TouristId { get; set; }
        public int DriverId { get; set; }

        public DriverUnreliableReport() { }

        public DriverUnreliableReport(int touristId, int driverId, int driveId, DateTime date)
        {

            TouristId = touristId;
            DriverId = driverId;
            DriveId = driveId;
            Date = date;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TouristId = int.Parse(values[1]);
            DriverId = int.Parse(values[2]);
            DriveId = int.Parse(values[3]);
            Date = DateTime.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), TouristId.ToString(), DriverId.ToString(), DriveId.ToString(), Date.ToString() };
        }
    }
}
