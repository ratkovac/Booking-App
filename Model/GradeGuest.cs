using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class GradeGuest : ISerializable
    {

        public int Id { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        public int Cleanliness { get; set; }
        public int RulesFollowing { get; set; }
        public string Comment { get; set; }

        public GradeGuest()
        {

        }

        public GradeGuest(int id, AccommodationReservation accommodationReservation, int cleanliness, int rulesfollowing, string comment)
        {
            Id = id;
            AccommodationReservation = accommodationReservation;
            Cleanliness = cleanliness;
            RulesFollowing = rulesfollowing;
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
                RulesFollowing.ToString(),
                Comment
            };
            return values;
        }


        public void FromCSV(string[] values)
        {

            try {
                Id = Convert.ToInt32(values[0]);
                int reservationId = Convert.ToInt32(values[1]);
                AccommodationReservationRepository accommodationReservationRepository = new AccommodationReservationRepository();
                AccommodationReservation = accommodationReservationRepository.GetByID(reservationId);
                Cleanliness = Convert.ToInt32(values[2]);
                RulesFollowing = Convert.ToInt32(values[3]);
                Comment = values[4];
            }
            catch (Exception ex)
            {
                Console.WriteLine("No data for reading: " + ex.Message);
            }

        }

    }
}
