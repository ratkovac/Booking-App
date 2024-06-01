using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public enum GuestState { Present, NotPresent }
    public class TourGuest : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public int TourReservationId { get; set; }
        public TourReservation TourReservation { get; set; }
        public int TouristId { get; set; }
        public int CheckpointId { get; set; }
        public GuestState State { get; set; }

        public TourGuest() { }

        public TourGuest(string name, string age, int tourInstanceId, int touristId, int checkpointId)
        {
            Name = name;
            Age = age;
            TourReservationId = tourInstanceId;
            TouristId = touristId;
            CheckpointId = checkpointId;
            State = GuestState.NotPresent;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Age = values[2];
            TourReservationId = int.Parse(values[3]);
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            TourReservation = tourReservationRepository.GetById(TourReservationId);
            TouristId = int.Parse(values[4]);
            CheckpointId = int.Parse(values[5]);
            State = (GuestState)Enum.Parse(typeof(GuestState), values[6]);
        }

        public string[] ToCSV()
        {
            return new string[] {
                Id.ToString(),
                Name,
                Age,
                TourReservationId.ToString(),
                TouristId.ToString(),
                CheckpointId.ToString(),
                State.ToString()
            };
        }
    }
}
