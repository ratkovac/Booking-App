using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Service
{
    public class DriverStatsService
    {
        private IDriverStatsRepository driverStatsRepository;

        public DriverStatsService()
        {
            driverStatsRepository = Injector.CreateInstance<IDriverStatsRepository>();
        }

        public int NextId()
        {
            return driverStatsRepository.NextId();
        }

        public List<DriverStats> GetAllDriverStats()
        {
            return driverStatsRepository.GetAll();
        }

        public DriverStats GetDriverStatsById(int id)
        {
            return driverStatsRepository.GetById(id);
        }

        public void Create(DriverStats driverStats)
        {
            driverStatsRepository.Create(driverStats);
        }

        public void Delete(DriverStats driverStats)
        {
            driverStatsRepository.Delete(driverStats);
        }

        public void Update(DriverStats driverStats)
        {
            driverStatsRepository.Update(driverStats);
        }

        public void Subscribe(IObserver observer)
        {
            driverStatsRepository.Subscribe(observer);
        }
    }
}
