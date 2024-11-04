using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TourRequestGuestService
    {
        private ITourRequestGuestRepository tourRequestGuestRepository;
        public TourRequestGuestService()
        {
            tourRequestGuestRepository = Injector.CreateInstance<ITourRequestGuestRepository>();
        }
        public int NextId()
        {
            return tourRequestGuestRepository.NextId();
        }
        public List<TourRequestGuest> GetAllTourRequestGuests()
        {
            return tourRequestGuestRepository.GetAll();
        }
        public List<TourRequestGuest> GetAllForSegmentId(int tourSegmentId)
        {
            return tourRequestGuestRepository.GetAllForSegmentId(tourSegmentId);
        }
        public TourRequestGuest GetTourRequestGuestsById(int id)
        {
            return tourRequestGuestRepository.GetById(id);
        }
        public void Create(TourRequestGuest tourRequestGuest)
        {
            tourRequestGuestRepository.Create(tourRequestGuest);
        }
        public void Delete(TourRequestGuest tourRequestGuest)
        {
            tourRequestGuestRepository.Delete(tourRequestGuest);
        }
        public void Update(TourRequestGuest tourRequestGuest)
        {
            tourRequestGuestRepository.Update(tourRequestGuest);
        }
        public void Subscribe(IObserver observer)
        {
            tourRequestGuestRepository.Subscribe(observer);
        }
    }
}