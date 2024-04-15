using BookingApp.Repository;
using BookingApp.Serializer;
using System;

namespace BookingApp.Model
{
    public class GradeAccommodation : ISerializable
    {
        public int Id { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        public int Cleanliness { get; set; }
        public int Correctness { get; set; }
        public string Comment { get; set; }

        public GradeAccommodation()
        {

        }

        public GradeAccommodation(int id, AccommodationReservation accommodationReservation, int cleanliness, int correctness, string comment)
        {
            Id = id;
            AccommodationReservation = accommodationReservation;
            Cleanliness = cleanliness;
            Correctness = correctness;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string accommodationReservation = AccommodationReservation.Id.ToString();
            string[] values =
            {
                Id.ToString(),
                accommodationReservation,
                Cleanliness.ToString(),
                Correctness.ToString(),
                Comment
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("No data for reading: " + ex.Message);
            }

        }
    }
}
