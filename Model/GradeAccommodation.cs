using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Documents;

namespace BookingApp.Model
{
    public class GradeAccommodation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        public int Cleanliness { get; set; }
        public int Correctness { get; set; }
        public string Comment { get; set; }

        public List<Image> Images { get; set; }

        public GradeAccommodation()
        {

        }
        public GradeAccommodation(AccommodationReservation accommodationReservation, int cleanliness, int ownerCorrectness, string comment)
        {
            AccommodationReservation = accommodationReservation;
            Cleanliness = cleanliness;
            Correctness = ownerCorrectness;
            Comment = comment;
        }
        public GradeAccommodation(AccommodationReservation accommodationReservation, int cleanliness, int ownerCorrectness, string comment, List<Image> images)
        {
            AccommodationReservation = accommodationReservation;
            Cleanliness = cleanliness;
            Correctness = ownerCorrectness;
            Comment = comment;
            Images = images;
        }

        public string[] ToCSV()
        {
            string imagesString = string.Join("|", Images.Select(image => image.Id.ToString()));
            string accommodationReservation = AccommodationReservation.Id.ToString();
            string[] values =
            {
                Id.ToString(),
                accommodationReservation,
                Cleanliness.ToString(),
                Correctness.ToString(),
                Comment,
                imagesString
            };

            return values;
        }

        public void FromCSV(string[] values)
        {
            try
            {
                Id = Convert.ToInt32(values[0]);
                int reservationId = Convert.ToInt32(values[1]);
                AccommodationReservationRepository accommodationReservationRepository = new AccommodationReservationRepository();
                AccommodationReservation = accommodationReservationRepository.GetByID(reservationId);
                Cleanliness = Convert.ToInt32(values[2]);
                Correctness = Convert.ToInt32(values[3]);
                Comment = values[4];
                for (int i = 5; i < values.Length; i++)
                {
                    ImageRepository imageRepository = new ImageRepository();
                    Image image = imageRepository.GetById(Convert.ToInt32(values[i]));
                    Images = new List<Image>();
                    Images.Add(image);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("No data for reading: " + ex.Message);
            }
        }

    }
}
