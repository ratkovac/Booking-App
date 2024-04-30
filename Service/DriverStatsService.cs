using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace BookingApp.Service
{
    public class DriverStatsService
    {
        private IDriverStatsRepository driverStatsRepository;
        private readonly SuccessfulDrivesRepository _successfulDrivesRepository;
        private readonly DrivesDrivenRepository _drivesDrivenRepository;

        public DriverStatsService()
        {
            driverStatsRepository = Injector.CreateInstance<IDriverStatsRepository>();
            _successfulDrivesRepository = new SuccessfulDrivesRepository();
            _drivesDrivenRepository = new DrivesDrivenRepository(); 
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

        public List<string> GetYears()
        {
            return _successfulDrivesRepository.GetYears();
        }
        public DriverStats GetStatsByDriverId(int id)
        {
            return driverStatsRepository.GetByDriverId(id);
        }

        public List<int> GetDrivesByMonthAndYear(int month, int year, int id)
        {
            return (_successfulDrivesRepository.GetDriveIdsByMonthAndYear(month,year,id));
        }
        public double GetAveragePriceForDrives(List<int> idsPerMonth)
        {
            return _drivesDrivenRepository.CalculateAveragePriceForDrives(idsPerMonth);
        }
        public double GetAverageDuration(List<int> idsPerMonth)
        {
            return _drivesDrivenRepository.CalculateAverageDurationForDrives(idsPerMonth);
        }
        public int GetNumberOfDrives(int month, int year, int id)
        {
            return _successfulDrivesRepository.GetNumberOfDrivesByMonthAndYear(month, year, id);
        }

        public void Subscribe(IObserver observer)
        {
            driverStatsRepository.Subscribe(observer);
        }
    }
}
