using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Serializer;
using BookingApp.View;

namespace BookingApp.Model
{
    public enum TouristState { InactiveTour, ActiveTour, Waiting, Present, NotPresent }
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int TourInstanceId { get; set; }
        public TourInstance TourInstance { get; set; }
        public int TouristId { get; set; }
        public bool UsedVoucher { get; set; }
        public bool RatedTour { get; set; }
        public Tourist Tourist { get; set; }
        public TouristState State { get; set; }
        public bool WonVoucher { get; set; }

        public TourReservation() { }

        public TourReservation(int tourInstanceId, int touristId, bool usedVoucher, bool ratedTour)
        {
            TourInstanceId = tourInstanceId;
            State = TouristState.InactiveTour;
            TouristId = touristId;
            UsedVoucher = usedVoucher;
            RatedTour = ratedTour;
            WonVoucher = false;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourInstanceId = Convert.ToInt32(values[1]);
            TourInstanceRepository tourInstanceRepository = new TourInstanceRepository();
            TourInstance = tourInstanceRepository.GetById(TourInstanceId);
            TouristId = Convert.ToInt32(values[2]);
            State = (TouristState)Enum.Parse(typeof(TouristState), values[3]);
            UsedVoucher = Convert.ToBoolean(values[4]);
            RatedTour = bool.Parse(values[5]);
            WonVoucher = bool.Parse(values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvvalues = { Id.ToString(), TourInstanceId.ToString(), TouristId.ToString(), State.ToString(), UsedVoucher.ToString(),
            RatedTour.ToString(), WonVoucher.ToString()};
            return csvvalues;
        }
    }
}
