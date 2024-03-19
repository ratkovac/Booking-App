using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourGuestRepository
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

        public List<TourGuest> GetAllByTourInstanceId(int tourInstanceId)
        {
            return _serializer.FromCSV(FilePath).Where(tg => tg.TourReservationId == tourInstanceId).ToList();
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

        public TourGuest Update(TourGuest tourGuest)
        {
            TourGuest current = _tourGuests.Find(tg => tg.Id == tourGuest.Id);
            int index = _tourGuests.IndexOf(current);
            _tourGuests.Remove(current);
            _tourGuests.Insert(index, tourGuest);
            _serializer.ToCSV(FilePath, _tourGuests);
            return tourGuest;
        }
    }
}
