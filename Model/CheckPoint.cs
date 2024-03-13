using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class CheckPoint : ISerializable
    {
        public int Id { get; set; }
        public string PointText { get; set; }
        public int TourId { get; set; }

        public static int nextId = 1;

        public CheckPoint() { }
        public CheckPoint(string pointText, int tourId)
        {
            Id = nextId++;
            //TourId = Tour.nextId;
            PointText = pointText;
            TourId = tourId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), PointText, TourId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            PointText = values[1];
            TourId = Convert.ToInt32(values[2]);
        }
    }   

}
