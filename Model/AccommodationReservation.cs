﻿using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public int ReservationDays { get; set; }
        public double UserGrade { get; set; }

        public AccommodationReservation(int id, Accommodation accommodation, User user, DateOnly startDate, DateOnly endDate,int reservationDays, double userGrade)
        {
            Id = id;
            Accommodation = accommodation;
            User = user;
            StartDate = startDate;
            EndDate = endDate;
            ReservationDays = reservationDays;
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
                ReservationDays.ToString(),
                UserGrade.ToString()
            };
            return values;
        }

        public void FromCSV(string[] values)
        {

            DateTime startDateDateTime;
            DateTime endDateDateTime;

            Id = Convert.ToInt32(values[0]);
            int accommodationId = Convert.ToInt32(values[1]);
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            Accommodation = accommodationRepository.GetByID(accommodationId);
            int userId = Convert.ToInt32(values[2]);
            UserRepository userRepository = new UserRepository();
            User = userRepository.GetByID(userId);
            bool startDateSuccess = DateTime.TryParseExact(values[3].Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateDateTime);
            bool endDateSuccess = DateTime.TryParseExact(values[4].Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateDateTime);
            ApplyParsedDates(startDateSuccess, endDateSuccess, startDateDateTime, endDateDateTime);
            ReservationDays = Convert.ToInt32(values[5]);
            UserGrade = Convert.ToDouble(values[6]);
        }

        private void ApplyParsedDates(bool startDateSuccess, bool endDateSuccess, DateTime startDateDateTime, DateTime endDateDateTime)
        {
            if (startDateSuccess && endDateSuccess)
            {
                this.StartDate = DateOnly.FromDateTime(startDateDateTime);
                this.EndDate = DateOnly.FromDateTime(endDateDateTime);
            }
        }
    }
}
