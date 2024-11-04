using BookingApp.DependencyInjection;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;

namespace BookingApp.Service
{
    public class TourService
    {
        private TourRepository tourRepository;

        public TourService()
        {
            tourRepository = new TourRepository();
        }

        public Tour GetById(int id)
        {
            return tourRepository.GetById(id);
        }
    }
}
