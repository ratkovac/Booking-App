using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface IDriverStatsRepository : IGenericRepository<DriverStats, int>
    {
        public void UpdateFromDriverStatsUpdates(DriverStatsUpdateRepository driverStatsUpdateRepository);

        DriverStats GetByDriverId(int driverId);
        public void Subscribe(IObserver observer)
        {

        }

    }
}


