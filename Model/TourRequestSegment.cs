using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public enum TourRequestStatus { ACCEPTED, CANCELLED, WAITING }
    public class TourRequestSegment : ISerializable
    {
        public int Id { get; set; }
        public int TourRequestId { get; set; }
        public TourRequest TourRequest { get; set; }
        public DateTime DateAccepted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? TourDescription { get; set; }
        public int Capacity { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public Language Language { get; set; }
        public int LanguageId { get; set; }
        public TourRequestStatus IsAccepted { get; set; }

        public TourRequestSegment(int tourRequestId, string tourDescription, int locationId, int languageId, int capacity, DateTime startDate, DateTime endDate)
        {
            TourRequestId = tourRequestId;
            TourDescription = tourDescription;
            LocationId = locationId;
            LanguageId = languageId;
            Capacity = capacity;
            StartDate = startDate;
            EndDate = endDate;
            DateAccepted = DateTime.Now;
            IsAccepted = TourRequestStatus.WAITING;
        }

        public TourRequestSegment() { }

        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), TourRequestId.ToString(), TourDescription, LocationId.ToString(), LanguageId.ToString(), Capacity.ToString(), StartDate.ToString(), EndDate.ToString(), DateAccepted.ToString(), IsAccepted.ToString() };
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourRequestId = Convert.ToInt32(values[1]);
            TourRequestRepository tourRequestRepository = new TourRequestRepository();
            TourRequest = tourRequestRepository.GetById(TourRequestId);
            TourDescription = values[2];
            LocationId = Convert.ToInt32(values[3]);
            LocationRepository locationRepository = new LocationRepository();
            Location = locationRepository.GetById(LocationId);
            LanguageId = Convert.ToInt32(values[4]);
            LanguageRepository languageRepository = new LanguageRepository();
            Language = languageRepository.GetById(LanguageId);
            Capacity = Convert.ToInt32(values[5]);
            StartDate = DateTime.Parse(values[6]);
            EndDate = DateTime.Parse(values[7]);
            DateAccepted = DateTime.Parse(values[8]);
            IsAccepted = (TourRequestStatus)Enum.Parse(typeof(TourRequestStatus), values[9]);
        }
    }
}
