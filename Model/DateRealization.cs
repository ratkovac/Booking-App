using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class DateRealization : ISerializable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TourId { get; set; }

        public static int NextId = 1;

        public DateRealization() { }

        public DateRealization(DateTime date, int tourId)
        {
            Id = NextId++;
            Date = date;
            TourId = tourId;
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), Date.ToString(), TourId.ToString() };
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Date = Convert.ToDateTime(values[1]);
            TourId = Convert.ToInt32(values[2]);
        }
    }
}
