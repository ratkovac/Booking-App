using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    internal interface ICheckPointRepository : IGenericRepository<CheckPoint, int>
    {
        public CheckPoint Save(CheckPoint checkPoint);
        public List<CheckPoint> GetCheckPoints(int tourId);
        public CheckPoint GetById(int id);


    }
}
