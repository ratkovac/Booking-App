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
    public class TourRequestService
    {
        private ITourRequestRepository tourRequestRepository;
        public LocationRepository locationRepository;
        public TourRequestService()
        {
            tourRequestRepository = Injector.CreateInstance<ITourRequestRepository>();
            locationRepository = new LocationRepository();
        }
        public int NextId()
        {
            return tourRequestRepository.NextId();
        }
        public List<TourRequest> GetAllTourRequests()
        {
            return tourRequestRepository.GetAll();
        }
        public TourRequest GetTourRequestById(int id)
        {
            return tourRequestRepository.GetById(id);
        }
        public void Create(TourRequest tourRequest)
        {
            tourRequestRepository.Create(tourRequest);
        }
        public void Delete(TourRequest tourRequest)
        {
            tourRequestRepository.Delete(tourRequest);
        }
        public void Update(TourRequest tourRequest)
        {
            tourRequestRepository.Update(tourRequest);
        }
        public void Subscribe(IObserver observer)
        {
            tourRequestRepository.Subscribe(observer);
        }
        public List<TourRequest> GetRequestForTourist(int touristId)
        {
            var requests = GetAllRequests();
            if (requests == null)
            {
                return new List<TourRequest>();
            }
            return requests.Where(request => request.TouristId == touristId).ToList();
        }

        public List<TourRequest> GetRequests()
        {
            var allRequests = tourRequestRepository.GetAll();
            if (allRequests == null)
            {
                return new List<TourRequest>();
            }
            return allRequests.Where(request => request.IsComplex == false).ToList();
        }

        public List<TourRequest> GetAllRequests()
        {
            var allRequests = tourRequestRepository.GetAll();
            if (allRequests == null)
            {
                return new List<TourRequest>();
            }
            return allRequests.ToList();
        }
    }
}