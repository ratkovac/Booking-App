using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Image : ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int AccomodationId { get; set; }
        public int TourId { get; set; }

        public Image() { }
        public Image(string path, int accomodationId, int tourId)
        {
            Path = path;
            AccomodationId = accomodationId;
            TourId = tourId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Path = values[1];
            AccomodationId = Convert.ToInt32(values[2]);
            TourId = Convert.ToInt32(values[3]);
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                Path,
                AccomodationId.ToString(),
                TourId.ToString()
            };
            return values;
        }
    }
}
