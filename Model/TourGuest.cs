using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourGuest : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TourReservationId { get; set; }
        public int TouristId { get; set; }

        public int CheckpointId { get; set; }
        public TourGuest() { }

        public TourGuest(string name, int tourInstanceId, int touristId, int checkpointId)
        {
            Name = name;
            TourReservationId = tourInstanceId;
            TouristId = touristId;
            CheckpointId = checkpointId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            TourReservationId = int.Parse(values[2]);
            TouristId = int.Parse(values[3]);
            CheckpointId = int.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                Name,
                TourReservationId.ToString(),
                TouristId.ToString(),
                CheckpointId.ToString()
            };
        }
    }
}
