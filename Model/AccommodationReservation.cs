using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId { get; set; }
        public int UserId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public AccommodationReservation(int id, int accommodationId, int userId, DateOnly startDate, DateOnly endDate)
        {
            Id = id;
            AccommodationId = accommodationId;
            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public AccommodationReservation()
        {
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                UserId.ToString(),
                StartDate.ToString(),
                EndDate.ToString()
            };
            return values;
        }

        public void FromCSV(string[] values)
        {
            DateOnly StartDate;
            DateOnly EndDate;

            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            UserId = Convert.ToInt32(values[2]);
            bool startDateSuccess = DateOnly.TryParse(values[3], out StartDate);
            bool endDateSuccess = DateOnly.TryParse(values[4], out EndDate);
        }
    }
}
