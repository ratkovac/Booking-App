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
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public Accommodation Accommodation { get; set; }
        public User User { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public double UserGrade { get; set; }

        public AccommodationReservation(int id, Accommodation accommodation, User user, DateOnly startDate, DateOnly endDate, double userGrade)
        {
            Id = id;
            Accommodation = accommodation;
            User = user;
            StartDate = startDate;
            EndDate = endDate;
            UserGrade = userGrade;
        }

        public AccommodationReservation()
        {
        }

        public string[] ToCSV()
        {
            string accommodation = Accommodation.Id.ToString();
            string user = User.Id.ToString();
            string[] values =
            {
                Id.ToString(),
                accommodation,
                user,
                StartDate.ToString(),
                EndDate.ToString(),
                UserGrade.ToString()
            };
            return values;
        }

        public void FromCSV(string[] values)
        {
            DateOnly StartDate;
            DateOnly EndDate;

            Id = Convert.ToInt32(values[0]);
            int accommodationId = Convert.ToInt32(values[1]);
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            Accommodation = accommodationRepository.GetByID(accommodationId);
            int userId = Convert.ToInt32(values[2]);
            UserRepository userRepository = new UserRepository();
            User = userRepository.GetByID(userId);
            bool startDateSuccess = DateOnly.TryParse(values[3], out StartDate);
            bool endDateSuccess = DateOnly.TryParse(values[4], out EndDate);
            UserGrade = Convert.ToDouble(values[5]);
        }
    }
}
