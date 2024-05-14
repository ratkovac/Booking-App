using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.WPF.View.Tourist.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourGuestRepository : ITourGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourGuests.csv";

        private readonly Serializer<TourGuest> _serializer;

        private List<TourGuest> _tourGuests;

        public TourGuestRepository()
        {
            _serializer = new Serializer<TourGuest>();
            _tourGuests = _serializer.FromCSV(FilePath);
        }

        public List<TourGuest> GetAll()
        {
            return _tourGuests;
        }

        public List<TourGuest> GetAllByTouristId(int touristId)
        {
            return _serializer.FromCSV(FilePath).Where(tg => tg.TouristId == touristId).ToList();
        }

        public TourGuest GetById(int id)
        {
            return _tourGuests.Find(r => r.Id == id);
        }

        public List<TourGuest> GetAllPresentByTourReservationId(int tourReservationId)
        {
            return _serializer.FromCSV(FilePath)
                   .Where(tg => tg.TourReservationId == tourReservationId && tg.State == GuestState.Present)
                   .ToList();
        }

        public List<TourGuest> GetAllByTourInstanceId(int tourInstanceId)
        {
            var allTourGuests = _serializer.FromCSV(FilePath);
            var filteredTourGuests = allTourGuests.Where(tg => tg.TourReservationId == tourInstanceId).ToList();
            return filteredTourGuests;
        }
        public List<TourGuest> GetAllByTourReservationId(int tourReservationId)
        {
            return _serializer.FromCSV(FilePath).Where(tg => tg.TourReservationId == tourReservationId).ToList();
        }

        public List<TourGuest> GetByTouristAndReservationId(int tourReservationId, int touristId)
        {
            return _serializer.FromCSV(FilePath).Where(tg => tg.TourReservationId == tourReservationId && tg.TouristId == touristId).ToList();
        }

        public int GetTouristNumberByTourReservationId(int tourReservationId)
        {
            List<TourGuest> guests = GetAllByTourReservationId(tourReservationId);
            return guests.Count;
        }

        public TourGuest Save(TourGuest tourGuest)
        {
            tourGuest.Id = NextId();
            _tourGuests.Add(tourGuest);
            _serializer.ToCSV(FilePath, _tourGuests);
            return tourGuest;
        }

        public void SaveMultiple(List<TourGuest> tourGuests)
        {
            foreach (var tourGuest in tourGuests)
            {
                tourGuest.Id = NextId();
                _tourGuests.Add(tourGuest);
            }

            _serializer.ToCSV(FilePath, _tourGuests);
        }

        public int NextId()
        {
            if (_tourGuests.Count < 1)
            {
                return 1;
            }
            return _tourGuests.Max(tg => tg.Id) + 1;
        }

        public void Create(TourGuest tourGuest)
        {
            tourGuest.Id = NextId();
            _tourGuests.Add(tourGuest);
            _serializer.ToCSV(FilePath, _tourGuests);
        }

        public void Delete(TourGuest tourGuest)
        {
            TourGuest found = _tourGuests.Find(tg => tg.Id == tourGuest.Id);
            _tourGuests.Remove(found);
            _serializer.ToCSV(FilePath, _tourGuests);
        }

        public void Update(TourGuest tourGuest)
        {
            TourGuest current = _tourGuests.Find(tg => tg.Id == tourGuest.Id);
            int index = _tourGuests.IndexOf(current);
            _tourGuests.Remove(current);
            _tourGuests.Insert(index, tourGuest);
            _serializer.ToCSV(FilePath, _tourGuests);
        }

    }
}
