using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Service
{
    public class FastDriveService
    {
        private IFastDriveRepository fastDriveRepository;
        private readonly VehicleRepository _vehicleRepository;
        private readonly DriveRepository _driveRepository;
        private readonly DriverStatsRepository _driverStatsRepository;
        private readonly DriverStatsUpdateRepository _driverStatsUpdateRepository;
        private readonly LocationRepository _locationRepository;

        public FastDriveService()
        {
            fastDriveRepository = Injector.CreateInstance<IFastDriveRepository>();
            _vehicleRepository = new VehicleRepository();
            _driveRepository = new DriveRepository();
            _driverStatsRepository = new DriverStatsRepository();
            _driverStatsUpdateRepository = new DriverStatsUpdateRepository();
            _locationRepository = new LocationRepository();
        }
        public int NextId()
        {
            return fastDriveRepository.NextId();
        }
        public List<FastDrive> GetAllFastDrives()
        {
            return fastDriveRepository.GetAll();
        }
        public FastDrive GetFastDriveById(int id)
        {
            return fastDriveRepository.GetById(id);
        }
        public void Create(FastDrive fastDrive)
        {
            fastDriveRepository.Create(fastDrive);
        }
        public void Delete(FastDrive fastDrive)
        {
            fastDriveRepository.Delete(fastDrive);
        }
        public void Update(FastDrive fastDrive)
        {
            fastDriveRepository.Update(fastDrive);
        }
        public void SaveDrive(Drive drive)
        {
            _driveRepository.Save(drive);
        }
        public void Subscribe(IObserver observer)
        {
            fastDriveRepository.Subscribe(observer);
        }

        internal bool CheckDuration(double duration)
        {
            if (duration > 6)
            {
                return false;
            }
            return true;
        }

        internal int GetAvailableDriver()
        {
            return _driveRepository.GetAvailableDriverId();
        }

        public void AddBonusPoints(int id)
        {
            var driverStats = _driverStatsRepository.GetByDriverId(id);

            DriverStatsUpdate update = new DriverStatsUpdate(id);
            if (driverStats != null)
            {
                if (driverStats.FastDrives <= 15)
                {
                    driverStats.FastDrives += 1;
                    update.FastDrivesUpdate += 1;
                    _driverStatsUpdateRepository.Create(update);
                }
                else
                {
                    driverStats.BonusPoints += 5;
                    update.BonusPointsUpdate += 5;
                    _driverStatsUpdateRepository.Create(update);
                }

                _driverStatsRepository.Update(driverStats);
            }
        }
        public Location GetLocationById(int id)
        {
            return _locationRepository.GetLocationById(id);
        }
    }
}
