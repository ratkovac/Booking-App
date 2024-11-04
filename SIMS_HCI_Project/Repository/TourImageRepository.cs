using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourImageRepository : ITourImageRepository
    {
        private const string FilePath = "../../../Resources/Data/tourImages.csv";

        private readonly Serializer<TourImage> _serializer;

        private List<TourImage> _tourImages;

        public TourImageRepository()
        {
            _serializer = new Serializer<TourImage>();
            _tourImages = _serializer.FromCSV(FilePath);
        }

        public TourImage GetById(int id)
        {
            return _tourImages.Find(ti => ti.Id == id);
        }

        public List<TourImage> GetAll()
        {
            return _tourImages;
        }

        public TourImage Save(TourImage tourImage)
        {
            tourImage.Id = NextId();
            _tourImages.Add(tourImage);
            _serializer.ToCSV(FilePath, _tourImages);
            return tourImage;
        }

        public int NextId()
        {
            if (_tourImages.Count < 1)
            {
                return 1;
            }
            return _tourImages.Max(ti => ti.Id) + 1;
        }

        public void Create(TourImage tourImage)
        {
            tourImage.Id = NextId();
            _tourImages.Add(tourImage);
            _serializer.ToCSV(FilePath, _tourImages);
        }

        public void Delete(TourImage tourImage)
        {
            TourImage found = _tourImages.Find(ti => ti.Id == tourImage.Id);
            _tourImages.Remove(found);
            _serializer.ToCSV(FilePath, _tourImages);
        }

        public void Update(TourImage tourImage)
        {
            TourImage current = _tourImages.Find(ti => ti.Id == tourImage.Id);
            if (current != null)
            {
                int index = _tourImages.IndexOf(current);
                _tourImages.RemoveAt(index);
                _tourImages.Insert(index, tourImage);
                _serializer.ToCSV(FilePath, _tourImages);
            }
        }

    }
}
