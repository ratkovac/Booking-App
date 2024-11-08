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
        public double AccommodationGrade { get; set; }
        public int Capacity { get; set; }

        public AccommodationReservation(int id, Accommodation accommodation, User user, DateOnly startDate, DateOnly endDate, int reservationDays, double userGrade, double accommodationGrade, int capacity)
        {
            Id = id;
            Accommodation = accommodation;
            User = user;
            StartDate = startDate;
            EndDate = endDate;
            ReservationDays = reservationDays;
            UserGrade = userGrade;
            AccommodationGrade = accommodationGrade;
            Capacity = capacity;
        }

        public AccommodationReservation(int accommodationId, int userId, DateOnly startDate, DateOnly endDate, int reservationDays, double userGrade, double accommodationGrade, int capacity)
        {
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            UserRepository userRepository = new UserRepository();

            Accommodation = accommodationRepository.GetByID(accommodationId);
            User = userRepository.GetByID(userId);
            StartDate = startDate;
            EndDate = endDate;
            ReservationDays = reservationDays;
            UserGrade = userGrade;
            AccommodationGrade = accommodationGrade;
            Capacity = capacity;
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
                StartDate.ToString("yyyy-MM-dd"),
                EndDate.ToString("yyyy-MM-dd"),
                ReservationDays.ToString(),
                UserGrade.ToString(),
                AccommodationGrade.ToString(),
                Capacity.ToString()
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
            startDateDateTime = DateTime.Parse(values[3], CultureInfo.InvariantCulture);
            endDateDateTime = DateTime.Parse(values[4], CultureInfo.InvariantCulture);
            ApplyParsedDates(startDateDateTime, endDateDateTime);
            ReservationDays = Convert.ToInt32(values[5]);
            UserGrade = Convert.ToDouble(values[6]);
            AccommodationGrade = Convert.ToDouble(values[7]);
            Capacity = Convert.ToInt32(values[8]);
        }

        public override string ToString()
        {
            return $" {Id} |" +
                   $" {Accommodation.Id} |" +
                   $" {User.Id} |" +
                   $" {StartDate.ToString("yyyy-MM-dd")} |" +
                   $" {EndDate.ToString("yyyy-MM-dd")} |" +
                   $" {ReservationDays} |" +
                   $" {UserGrade} |" +
                   $" {AccommodationGrade} |" +
                   $" {Capacity}";
        }

        private void ApplyParsedDates(DateTime startDateDateTime, DateTime endDateDateTime)
        {
            this.StartDate = DateOnly.FromDateTime(startDateDateTime);
            this.EndDate = DateOnly.FromDateTime(endDateDateTime);
        }
    }
}
