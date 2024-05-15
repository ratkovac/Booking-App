using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourRequestGuest : ISerializable
    {
        public int Id { get; set; }
        public int TourSegmentId { get; set; }
        public int TouristId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public TourRequestGuest() { }

        public TourRequestGuest(string name, int age, int touristId, int tourSegmentId)
        {
            Name = name;
            Age = age;
            TouristId = touristId;
            TourSegmentId = tourSegmentId;
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), Name, Age.ToString(), TouristId.ToString(), TourSegmentId.ToString() };
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Age = Convert.ToInt32(values[2]);
            TouristId = Convert.ToInt32(values[3]);
            TourSegmentId = Convert.ToInt32(values[4]);
        }
    }
}
