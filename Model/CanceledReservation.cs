using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DependencyInjection;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.Service;

namespace BookingApp.Model
{
    public class CanceledReservation : ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        public bool Canceled { get; set; }

        public CanceledReservation()
        {

        }
        public CanceledReservation(int reservationId)
        {
            ReservationId = reservationId;
            AccommodationReservationRepository accommodationReservationRepository = new AccommodationReservationRepository();
            AccommodationReservation = accommodationReservationRepository.GetByID(reservationId);
            Canceled = true;
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                AccommodationReservation.ToString(),
                ("Canceled")
            };
            return values;
        }

        public void FromCSV(string[] values)
        {
            DateTime startDateDateTime;
            DateTime endDateDateTime;

            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            int accommodationId = Convert.ToInt32(values[2]);
            int userId = Convert.ToInt32(values[3]);
            startDateDateTime = DateTime.Parse(values[4], CultureInfo.InvariantCulture);
            endDateDateTime = DateTime.Parse(values[5], CultureInfo.InvariantCulture);
            DateOnly startDate = DateOnly.FromDateTime(startDateDateTime);
            DateOnly endDate = DateOnly.FromDateTime(endDateDateTime);
            int reservationDays = Convert.ToInt32(values[6]);
            int userGrade = Convert.ToInt32(values[7]);
            int accommodationGrade = Convert.ToInt32(values[8]);
            int capacity = Convert.ToInt32(values[9]);
            ApplyParsedDates(startDateDateTime, endDateDateTime);

            AccommodationReservation accommodationReservation = new AccommodationReservation(accommodationId, userId,
                startDate, endDate, reservationDays, userGrade, accommodationGrade, capacity);
            AccommodationReservation = accommodationReservation;
            Canceled = values[10].Contains("Canceled");
        }

        private void ApplyParsedDates(DateTime startDateDateTime, DateTime endDateDateTime)
        {

        }

    }
}
