
﻿using BookingApp.Serializer;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using BookingApp.Repository.RepositoryInterface;

 namespace BookingApp.Repository
{
    public class ImageRepository : IImageRepository
    {
        private const string FilePath = "../../../Resources/Data/images.csv";

        private readonly Serializer<Image> _serializer;

        private List<Image> _images;

        public ImageRepository()
        {
            _serializer = new Serializer<Image>();
            _images = _serializer.FromCSV(FilePath);
        }

        public List<Image> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _images = _serializer.FromCSV(FilePath);
            if (_images.Count < 1)
            {
                return 1;
            }
            return _images.Max(i => i.Id) + 1;
        }

        public void Create(Image entity)
        {
            entity.Id = NextId();
            _images = _serializer.FromCSV(FilePath);
            _images.Add(entity);
            _serializer.ToCSV(FilePath, _images);
        }

        void IGenericRepository<Image, int>.Update(Image entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Image image)
        {
            _images = _serializer.FromCSV(FilePath);
            Image founded = _images.Find(i => i.Id == image.Id);
            _images.Remove(founded);
            _serializer.ToCSV(FilePath, _images);
        }

        public Image GetById(int key)
        {
            return _images.Find(c => c.Id == key);
        }
        public Image? GetByAccommodationId(int accommodationId)
        {
            return _images.Find(c => c.AccomodationId == accommodationId);
        }

        public Image Update(Image image)
        {
            _images = _serializer.FromCSV(FilePath);
            Image current = _images.Find(i => i.Id == image.Id);
            int index = _images.IndexOf(current);
            _images.Remove(current);
            _images.Insert(index, image);
            _serializer.ToCSV(FilePath, _images);
            return image;
        }
    }
}
