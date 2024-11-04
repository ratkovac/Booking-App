using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;
using static BookingApp.Model.EventEnum;

namespace BookingApp.Model
{
    public class AccommodationEvent : ISerializable
    {
        public int Id { get; set; }
        public Accommodation Accommodation { get; set; }
        public DateOnly EventDate { get; set; }
        public EventType EventType { get; set; }
        public AccommodationEvent(){}
        public AccommodationEvent(Accommodation accommodation, DateOnly eventDate, EventType eventType)
        {
            Accommodation = accommodation;
            EventDate = eventDate;
            EventType = eventType;
        }
        public string[] ToCSV()
        {
            string accommodation = Accommodation.Id.ToString();
            string[] values =
            {
                Id.ToString(),
                accommodation,
                EventDate.ToString("yyyy-MM-dd"),
                EventType.ToString()
            };
            return values;
        }


        public void FromCSV(string[] values)
        {
            DateTime date;

            Id = Convert.ToInt32(values[0]);
            int accommodationId = Convert.ToInt32(values[1]);
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            Accommodation = accommodationRepository.GetByID(accommodationId);
            date = DateTime.Parse(values[2], CultureInfo.InvariantCulture);
            EventDate = DateOnly.FromDateTime(date);
            bool success = Enum.TryParse(values[3], out EventType parsedType);
            EventType = parsedType;
            
        }

    }
}
