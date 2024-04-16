using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace BookingApp.Model
{
    public class GradeTour : ISerializable
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public Tourist Tourist { get; set; }
        public int TourReservationId { get; set; }
        public TourReservation TourReservation { get; set; }
        public int TourGuestId { get; set; }
        public TourGuest TourGuest { get; set; }
        public int Grade { get; set; }
        public string Comment { get; set; }
        public List<string> Images { get; set; }
        public bool IsValid { get; set; }

        public GradeTour()
        {
        }

        public GradeTour(int tourId, int touristId, int tourGuestId, int grade, string comment, bool isValid)
        {
            TourReservationId = tourId;
            TouristId = touristId;
            TourGuestId = tourGuestId;
            Grade = grade;
            Comment = comment;
            IsValid = isValid;
        }

        public GradeTour(int touristId, Tourist tourist, int tourReservationId, int grade, string comment, List<string> images)
        {
            TouristId = touristId;
            Tourist = tourist;
            TourReservationId = tourReservationId;
            Grade = grade;
            Comment = comment;
            Images = images;
            IsValid = true;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourReservationId = int.Parse(values[1]);
            TouristId = int.Parse(values[2]);
            TourGuestId = int.Parse(values[3]);
            Grade = int.Parse(values[4]);
            Comment = values[5];
            IsValid = bool.Parse(values[6]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                TourReservationId.ToString(),
                TouristId.ToString(),
                TourGuestId.ToString(),
                Grade.ToString(),
                Comment,
                IsValid.ToString()
            };
        }
    }
}