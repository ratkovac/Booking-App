using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int TourInstanceId { get; set; }
        public int UserId { get; set; }

        public TourReservation() { }

        public TourReservation(int tourInstanceId, int userId)
        {
            TourInstanceId = tourInstanceId;
            UserId = userId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourInstanceId = Convert.ToInt32(values[1]);
            UserId = Convert.ToInt32(values[2]);
        }

        public string[] ToCSV()
        {
            string[] csvvalues = { Id.ToString(), TourInstanceId.ToString(), UserId.ToString()};
            return csvvalues;
        }
    }
}
