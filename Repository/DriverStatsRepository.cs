using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class DriverStatsRepository : IDriverStatsRepository
    {
        private const string FilePath = "../../../Resources/Data/driverStats.csv";

        private readonly Serializer<DriverStats> _serializer;
        private List<IObserver> _observers;
        private List<DriverStats> _driverStats;

        public DriverStatsRepository()
        {
            _serializer = new Serializer<DriverStats>();
            _observers = new List<IObserver>();
            _driverStats = _serializer.FromCSV(FilePath);
        }

        public DriverStats Save(DriverStats driverStats)
        {
            driverStats.DriverId = NextId();
            _driverStats.Add(driverStats);
            _serializer.ToCSV(FilePath, _driverStats);
            return driverStats;
        }

        public int NextId()
        {
            if (_driverStats.Count == 0)
            {
                return 0;
            }
            return _driverStats.Max(ds => ds.DriverId) + 1;
        }

        public void Create(DriverStats driverStats)
        {
            driverStats.DriverId = NextId();
            _driverStats.Add(driverStats);
            _serializer.ToCSV(FilePath, _driverStats);
        }

        public void Delete(DriverStats driverStats)
        {
            DriverStats found = _driverStats.Find(ds => ds.DriverId == driverStats.DriverId);
            _driverStats.Remove(found);
            _serializer.ToCSV(FilePath, _driverStats);
        }

        public void Update(DriverStats driverStats)
        {
            int index = _driverStats.FindIndex(ds => driverStats.DriverId == ds.DriverId);
            if (index != -1)
            {
                _driverStats[index] = driverStats;
                _serializer.ToCSV(FilePath, _driverStats);
            }
        }

        public List<DriverStats> GetAll()
        {
            return _driverStats;
        }

        public DriverStats GetById(int id)
        {
            return _driverStats.Find(ds => ds.DriverId == id);
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
        public DriverStats GetByDriverId(int driverId)
        {
            return _driverStats.FirstOrDefault(ds => ds.DriverId == driverId);
        }
        public void UpdateFromDriverStatsUpdates(DriverStatsUpdateRepository driverStatsUpdateRepository)
        {
            var driverStatsUpdates = driverStatsUpdateRepository.GetAll();
            var updatesToDelete = new List<DriverStatsUpdate>();

            foreach (var driverStatsUpdate in driverStatsUpdates)
            {
                if (DateTime.Today.Subtract(driverStatsUpdate.DateOfUpdate).TotalDays >= 365)
                {
                    var driverStats = GetById(driverStatsUpdate.DriverId);

                    if (driverStats != null)
                    {
                        driverStats.FastDrives -= driverStatsUpdate.FastDrivesUpdate;
                        driverStats.BonusPoints -= driverStatsUpdate.BonusPointsUpdate;
                        driverStats.CancelledDrives -= driverStatsUpdate.CancelledDrivesUpdate;

                        Update(driverStats);

                        updatesToDelete.Add(driverStatsUpdate);
                    }
                }
            }

            foreach (var update in updatesToDelete)
            {
                driverStatsUpdateRepository.Delete(update);
            }
        }
    }
}
