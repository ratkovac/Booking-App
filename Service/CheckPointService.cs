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
    internal class CheckPointService
    {
        private CheckPointRepository checkPointRepository = new CheckPointRepository();

        public CheckPoint GetById(int id)
        {
            return checkPointRepository.GetById(id);
        }
    }
}
