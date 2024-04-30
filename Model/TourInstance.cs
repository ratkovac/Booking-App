using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public enum TourInstanceState { Inactive, Active, Finished, Cancelled }
    public class TourInstance : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int AvailableSlots { get; set; }
        public DateTime StartTime { get; set; }
        public TourInstanceState State { get; set; }
        public Tour Tour { get; set; }
        public string CurrentCheckpoint { get; set; }
        public bool IsCompleted { get; set; }

        public int GuideId { get; set; }

        public TourInstance()
        {

        }

        public TourInstance(int tourId, int availableSlots, DateTime startTime, TourInstanceState state)
        {
            TourId = tourId;
            AvailableSlots = availableSlots;
            StartTime = startTime;
            State = state;
            CurrentCheckpoint = "START";
            IsCompleted = false;
        }
        public TourInstance(int tourId, int availableSlots, DateTime startTime, TourInstanceState state, int guideId)
        {
            TourId = tourId;
            AvailableSlots = availableSlots;
            StartTime = startTime;
            State = state;
            GuideId = guideId;
            CurrentCheckpoint = "START";
            IsCompleted = false;
        }

        public static TourInstanceState GetState(string state)
        {
            return state switch
            {
                "Neaktivna" => TourInstanceState.Inactive,
                "Aktivna" => TourInstanceState.Active,
                "Zavrsena" => TourInstanceState.Finished,
                _ => TourInstanceState.Cancelled
            };
        }
        public static string GetState(TourInstanceState state)
        {
            return state switch
            {
                TourInstanceState.Inactive => "Neaktivna",
                TourInstanceState.Active => "Aktivna",
                TourInstanceState.Finished => "Zavrsena",
                _ => "Otkazana"
            };
        }

        public string[] ToCSV()
        {
            return new string[] {
            Id.ToString(),
            TourId.ToString(),
            AvailableSlots.ToString(),
            StartTime.ToString(),
            State.ToString(),
            GuideId.ToString(),
            IsCompleted.ToString(),
            CurrentCheckpoint
            };
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            AvailableSlots = Convert.ToInt32(values[2]);
            StartTime = DateTime.Parse(values[3]);
            State = (TourInstanceState)Enum.Parse(typeof(TourInstanceState), values[4]);
            GuideId = Convert.ToInt32(values[5]);
            IsCompleted = bool.Parse(values[6]);
            CurrentCheckpoint = values[7];
        }
    }
}
