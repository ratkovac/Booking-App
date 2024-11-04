using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service.Factories
{
    public class TourRequestStatisticsFactory
    {
        private TourRequestService _tourRequestService;
        private TourRequestSegmentService _tourSegmentService;

        public TourRequestStatisticsFactory(TourRequestService request, TourRequestSegmentService segment)
        {
            _tourRequestService = request;
            _tourSegmentService = segment;
        }

        public TourRequestStatistics CreateTouristStatForYear(int year, int userId)
        {
            var allRequestIds = _tourRequestService.GetRequestForTourist(userId).Select(request => request.Id);
            int acceptedCount = 0;
            int rejectedCount = 0;
            foreach (var id in allRequestIds)
            {
                var segment = _tourSegmentService.GetByTourRequestId(id);
                if (segment.DateAccepted.Year == year)
                {
                    if (segment.IsAccepted == TourRequestStatus.ACCEPTED) acceptedCount++;
                    if (segment.IsAccepted == TourRequestStatus.CANCELLED) rejectedCount++;
                }
            }

            return new TourRequestStatistics(acceptedCount, rejectedCount, year);
        }

        public TourRequestStatistics CreateTouristStatForEveryYear(int touristId)
        {
            var allRequestIds = _tourRequestService.GetRequestForTourist(touristId).Select(request => request.Id);
            int acceptedCount = 0;
            int rejectedCount = 0;
            foreach (var id in allRequestIds)
            {
                var segment = _tourSegmentService.GetByTourRequestId(id);
                if (segment.IsAccepted == TourRequestStatus.ACCEPTED) acceptedCount++;
                if (segment.IsAccepted == TourRequestStatus.CANCELLED) rejectedCount++;
            }

            return new TourRequestStatistics(acceptedCount, rejectedCount, 0);
        }

        public int GetEarliestYear(int touristId)
        {
            var allRequestIds = _tourRequestService.GetRequestForTourist(touristId).Select(request => request.Id);
            var allSegments = allRequestIds.Select(id => _tourSegmentService.GetByTourRequestId(id)).Where(segment => segment != null).ToList();

            if (!allSegments.Any())
            {
                return DateTime.Now.Year;
            }


            var earliestYear = allSegments.Min(segment => segment.DateAccepted.Year);

            return earliestYear;
        }

    }
}
