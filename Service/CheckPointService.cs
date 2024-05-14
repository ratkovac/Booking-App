using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class CheckPointService
    {
        private ICheckPointRepository checkPointRepository;

        public CheckPointService()
        {
            checkPointRepository = Injector.CreateInstance<ICheckPointRepository>();
        }

        public CheckPoint GetById(int id)
        {
            return checkPointRepository.GetById(id);
        }
    }
}
