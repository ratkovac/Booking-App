using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookingApp.Repository
{
    public class DriverStatsUpdateRepository : IDriverStatsUpdateRepository
    {
        private const string FilePath = "../../../Resources/Data/driverStatsUpdates.csv";

        private readonly Serializer<DriverStatsUpdate> _serializer;
        private List<IObserver> _observers;
        private List<DriverStatsUpdate> _driverStatsUpdates;

        public DriverStatsUpdateRepository()
        {
            _serializer = new Serializer<DriverStatsUpdate>();
            _observers = new List<IObserver>();
            _driverStatsUpdates = _serializer.FromCSV(FilePath);
        }

        public List<DriverStatsUpdate> GetAll()
        {
            return _driverStatsUpdates;
        }

        public int NextId()
        {
            if (_driverStatsUpdates.Count == 0)
            {
                return 0;
            }
            return _driverStatsUpdates.Max(ds => ds.Id) + 1;
        }

        public void Create(DriverStatsUpdate driverStatsUpdate)
        {
            driverStatsUpdate.Id = NextId();
            _driverStatsUpdates.Add(driverStatsUpdate);
            _serializer.ToCSV(FilePath, _driverStatsUpdates);
        }

        public void Update(DriverStatsUpdate driverStatsUpdate)
        {
            int index = _driverStatsUpdates.FindIndex(ds => driverStatsUpdate.Id == ds.Id);
            if (index != -1)
            {
                _driverStatsUpdates[index] = driverStatsUpdate;
                _serializer.ToCSV(FilePath, _driverStatsUpdates);
            }
        }

        public void Delete(DriverStatsUpdate driverStatsUpdate)
        {
            DriverStatsUpdate found = _driverStatsUpdates.Find(ds => ds.Id == driverStatsUpdate.Id);
            _driverStatsUpdates.Remove(found);
            _serializer.ToCSV(FilePath, _driverStatsUpdates);
        }

        public DriverStatsUpdate GetById(int id)
        {
            return _driverStatsUpdates.Find(ds => ds.Id == id);
        }
        public void FilterUpdates()
        {

        }
    }
}
