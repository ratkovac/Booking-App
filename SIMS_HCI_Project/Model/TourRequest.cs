using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public bool IsComplex { get; set; }
        public TourRequestStatus IsAccepted { get; set; }
        public int SegmentCount { get; set; }
        public bool CanSeeMore
        {
            get { return IsAccepted != TourRequestStatus.CANCELLED; }
        }
        public bool CanSeeMoreWaiting
        {
            get { return IsAccepted != TourRequestStatus.WAITING; }
        }

        public TourRequest() { }

        public TourRequest(int touristId, bool isComplex)
        {
            TouristId = touristId;
            IsAccepted = TourRequestStatus.WAITING;
            IsComplex = isComplex;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            IsAccepted = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[2]);
            IsComplex = bool.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), TouristId.ToString(), IsAccepted.ToString(), IsComplex.ToString() };
        }
    }
}
