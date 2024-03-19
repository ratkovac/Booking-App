using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourInstance : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int AvailableSlots { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsFinished { get; set; }

        public TourInstance()
        {

        }

        public TourInstance(int tourId, int availableSlots, DateTime startTime)
        {
            TourId = tourId;
            AvailableSlots = availableSlots;
            StartTime = startTime;
            IsFinished = false;
        }

        public string[] ToCSV()
        {
            return new string[] {
            Id.ToString(),
            TourId.ToString(),
            AvailableSlots.ToString(),
            StartTime.ToString(),
            IsFinished.ToString()
            };
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            AvailableSlots = Convert.ToInt32(values[2]);
            StartTime = DateTime.Parse(values[3]);
            IsFinished = bool.Parse(values[4]);
        }
    }
}
