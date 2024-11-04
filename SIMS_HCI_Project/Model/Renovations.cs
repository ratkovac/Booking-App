using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
namespace BookingApp.Model
{
    public class Renovations : ISerializable
    {
        public int Id { get; set; }
        public Accommodation Accommodation { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Description { get; set; }

        public Renovations(Accommodation accommodation, DateOnly startDate, DateOnly endDate, string description)
        {
            Accommodation = accommodation;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }
        public Renovations(int id, Accommodation accommodation, DateOnly startDate, DateOnly endDate, string description)
        {
            id = Id;
            Accommodation = accommodation;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
        }
        public Renovations() { }

        public string[] ToCSV()
        {
            string accommodation = Accommodation.Id.ToString();
            string[] values =
            {
                Id.ToString(),
                accommodation,
                StartDate.ToString("yyyy-MM-dd"),
                EndDate.ToString("yyyy-MM-dd"),
                Description
            };
            return values;
        }

        public void FromCSV(string[] values)
        {

            DateTime startRenovationDate;
            DateTime endRenovationDate;

            Id = Convert.ToInt32(values[0]);
            int accommodationId = Convert.ToInt32(values[1]);
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            Accommodation = accommodationRepository.GetByID(accommodationId);
            startRenovationDate = DateTime.Parse(values[2], CultureInfo.InvariantCulture);
            endRenovationDate = DateTime.Parse(values[3], CultureInfo.InvariantCulture);
            ApplyParsedDates(startRenovationDate, endRenovationDate);
            Description = values[4];
        }
        private void ApplyParsedDates(DateTime startDateDateTime, DateTime endDateDateTime)
        {
            this.StartDate = DateOnly.FromDateTime(startDateDateTime);
            this.EndDate = DateOnly.FromDateTime(endDateDateTime);
        }
    }
}
