using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourRequestSegmentService
    {
        private ITourRequestSegmentRepository tourRequestSegmentRepository;
        public TourRequestSegmentService()
        {
            tourRequestSegmentRepository = Injector.CreateInstance<ITourRequestSegmentRepository>();
        }
        public int NextId()
        {
            return tourRequestSegmentRepository.NextId();
        }
        public List<TourRequestSegment> GetAllTourRequestSegments()
        {
            return tourRequestSegmentRepository.GetAll();
        }
        public TourRequestSegment GetTourRequestSegmentById(int id)
        {
            return tourRequestSegmentRepository.GetById(id);
        }
        public void Create(TourRequestSegment tourRequestSegment)
        {
            tourRequestSegmentRepository.Create(tourRequestSegment);
        }
        public void Delete(TourRequestSegment tourRequestSegment)
        {
            tourRequestSegmentRepository.Delete(tourRequestSegment);
        }
        public void Update(TourRequestSegment tourRequestSegment)
        {
            tourRequestSegmentRepository.Update(tourRequestSegment);
        }
        public void Subscribe(IObserver observer)
        {
            tourRequestSegmentRepository.Subscribe(observer);
        }
        public List<TourRequestSegment> GetAllNonComplexRequests()
        {
            return tourRequestSegmentRepository.GetAllNonComplexRequests();
        }
        public List<TourRequestSegment> GetAllComplexRequests()
        {
            return tourRequestSegmentRepository.GetAllComplexRequests();
        }
        public List<TourRequestSegment> GetAllComplexSegmentsByComplexTourRequestId(int tourRequestId)
        {
            return tourRequestSegmentRepository.GetAllComplexSegmentsByTourRequestId(tourRequestId);
        }
        public TourRequestSegment GetByTourRequestId(int id)
        {
            return tourRequestSegmentRepository.GetAll().FirstOrDefault(request => request.TourRequestId == id);
        }
        public int FindEarliestYear()
        {
            int earliest = DateTime.Now.Year;

            foreach (var tourRequest in GetAllTourRequestSegments())
            {
                earliest = Math.Min(earliest, tourRequest.StartDate.Year);
            }

            return earliest;
        }
        public void SetAsAccepted(TourRequestSegment tourRequestSegment)
        {
            var tourRequest = tourRequestSegmentRepository.GetById(tourRequestSegment.Id);

            if (tourRequest == null)
            {
                throw new ArgumentException("Tour request not found");
            }

            tourRequest.IsAccepted = TourRequestStatus.ACCEPTED;
            tourRequest.DateAccepted = tourRequestSegment.DateAccepted;
            Update(tourRequest);
        }
        public (Dictionary<string, int>, int) GetTourRequestsForMonth(int year, Language language, Location location)
        {
            Dictionary<string, int> requestsForMonth = Enumerable.Range(1, 12).ToDictionary(i => i.ToString(), _ => 0);
            int total = 0;

            foreach (var tourRequest in GetAllTourRequestSegments())
            {
                if (tourRequest.StartDate.Year == year)
                {
                    bool isValidLanguage = language == null || tourRequest.LanguageId == language.Id;
                    bool isValidLocation = location == null || tourRequest.LocationId == location.Id;

                    if ((language == null && location == null) ||
                        (language != null && isValidLanguage && (location == null || isValidLocation)) ||
                        (location != null && isValidLocation && (language == null || isValidLanguage)))
                    {
                        string monthKey = tourRequest.StartDate.Month.ToString();
                        if (requestsForMonth.ContainsKey(monthKey))
                        {
                            requestsForMonth[monthKey]++;
                            total++;
                        }
                    }
                }
            }

            return (requestsForMonth, total);
        }
        public (int, int) FindBest()
        {
            DateTime oneYearAgo = DateTime.Now.AddYears(-1);
            List<TourRequestSegment> recentTourRequests = GetAllTourRequestSegments().Where(tourRequest => tourRequest.StartDate >= oneYearAgo).ToList();

            Dictionary<int, int> languageCounts = new Dictionary<int, int>();
            Dictionary<int, int> locationCounts = new Dictionary<int, int>();

            foreach (var tourRequest in recentTourRequests)
            {
                IncrementCount(languageCounts, tourRequest.LanguageId);
                IncrementCount(locationCounts, tourRequest.LocationId);
            }

            int mostFrequentLanguageId = GetMostFrequentId(languageCounts);
            int mostFrequentLocationId = GetMostFrequentId(locationCounts);

            return (mostFrequentLanguageId, mostFrequentLocationId);
        }

        private void IncrementCount(Dictionary<int, int> counts, int id)
        {
            if (counts.ContainsKey(id))
            {
                counts[id]++;
            }
            else
            {
                counts[id] = 1;
            }
        }

        private int GetMostFrequentId(Dictionary<int, int> counts)
        {
            return counts.OrderByDescending(pair => pair.Value).FirstOrDefault().Key;
        }

        public Dictionary<string, int> GetTourRequestsForYear(List<object> years, Language language, Location location)
        {
            List<string> validYears = years.Where(yr => yr.ToString() != "AllTime").Select(yr => yr.ToString()).ToList();
            Dictionary<string, int> requestsForChosenYear = validYears.ToDictionary(year => year, _ => 0);

            foreach (var tourRequest in GetAllTourRequestSegments())
            {
                bool isValidLanguage = language == null || tourRequest.LanguageId == language.Id;
                bool isValidLocation = location == null || tourRequest.LocationId == location.Id;

                if (isValidLanguage && isValidLocation)
                {
                    string yearKey = tourRequest.StartDate.Year.ToString();
                    if (requestsForChosenYear.ContainsKey(yearKey))
                    {
                        requestsForChosenYear[yearKey]++;
                    }
                }
            }
            return requestsForChosenYear;
        }
    }
}