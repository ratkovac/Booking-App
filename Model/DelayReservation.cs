using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.DependencyInjection;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.View.Owner;
using static BookingApp.Model.AccommodationTypeEnum;

namespace BookingApp.Model
{
    public enum DelayReservationStatusEnum { Pending, Approved, Declined };
    public class DelayReservation : ISerializable
    {
        public int Id { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public DateOnly NewStartDate { get; set; }
        public DateOnly NewEndDate { get; set; }
        public bool Read { get; set; }
        public DelayReservationStatusEnum Status { get; set; }

        public DelayReservation(AccommodationReservation reservation, DateOnly newStartDate, DateOnly newEndDate)
        {
            Reservation = reservation;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            Read = false;
        }

        public DelayReservation()
        {

        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                Reservation.Id.ToString(),
                NewStartDate.ToString("yyyy-MM-dd"),
                NewEndDate.ToString("yyyy-MM-dd"),
                ("UNREAD"),
                ("Pending")
            };
            return values;
        }



        public void FromCSV(string[] values)
        {
            DateTime startDateDateTime;
            DateTime endDateDateTime;

            Id = Convert.ToInt32(values[0]);
            int reservationId = Convert.ToInt32(values[1]);
            AccommodationReservationRepository accommodationReservationRepository =
                new AccommodationReservationRepository();
            Reservation = accommodationReservationRepository.GetByID(reservationId);
            startDateDateTime = DateTime.Parse(values[2], CultureInfo.InvariantCulture);
            endDateDateTime = DateTime.Parse(values[3], CultureInfo.InvariantCulture);
            ApplyParsedDates(startDateDateTime, endDateDateTime);
            Read = values[4].Contains("READ");
            bool success = Enum.TryParse(values[5], out DelayReservationStatusEnum status);
            Status = status;
        }

        private void ApplyParsedDates(DateTime startDateDateTime, DateTime endDateDateTime)
        {
            this.NewStartDate = DateOnly.FromDateTime(startDateDateTime);
            this.NewEndDate = DateOnly.FromDateTime(endDateDateTime);
        }
    }
}
