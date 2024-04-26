using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;

namespace BookingApp.Service
{
    public class ImageService
    {
        private IImageRepository _imageRepository;

        public ImageService()
        {
            _imageRepository = Injector.CreateInstance<IImageRepository>();
        }

        public void Create(Image image)
        {
            _imageRepository.Create(image);
        }
        public void Subscribe(IObserver observer)
        {
            _imageRepository.Subscribe(observer);
        }
    }
}
