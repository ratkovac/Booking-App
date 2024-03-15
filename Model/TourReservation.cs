using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public enum TouristState { InactiveTour, ActiveTour, Waiting, Present, NotPresent }
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public int NumberGuest { get; set; }
        public int UserId { get; set; }
        public TouristState State { get; set; }
        //public int TourTimeId { get; set; }

        public TourReservation() { }

        public TourReservation(int id, int tourId, Tour tour, int numberGuest, int userId)
        {
            Id = id;
            TourId = tourId;
            Tour = tour;
            NumberGuest = numberGuest;
            UserId = userId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            NumberGuest = Convert.ToInt32(values[2]);
            UserId = Convert.ToInt32(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvvalues = { Id.ToString(), TourId.ToString(), NumberGuest.ToString(), UserId.ToString() ,
                State.ToString()};
            return csvvalues;
        }
    }
}
