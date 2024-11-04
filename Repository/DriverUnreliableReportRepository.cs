using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class DriverUnreliableReportRepository : IDriverUnreliableReportRepository
    {
        private const string FilePath = "../../../Resources/Data/driverUnreliableReports.csv";

        private readonly Serializer<DriverUnreliableReport> _serializer;
        private List<IObserver> observers;
        private List<DriverUnreliableReport> _reports;

        public DriverUnreliableReportRepository()
        {
            _serializer = new Serializer<DriverUnreliableReport>();
            _reports = _serializer.FromCSV(FilePath);
        }

        public DriverUnreliableReport Save(DriverUnreliableReport driverUnreliableReport)
        {
            driverUnreliableReport.Id = NextId();
            _reports.Add(driverUnreliableReport);
            _serializer.ToCSV(FilePath, _reports);
            return driverUnreliableReport;
        }

        public int NextId()
        {
            if (_reports.Count == 0)
            {
                return 0;
            }
            return _reports.Max(udr => udr.Id) + 1;
        }

        public void Create(DriverUnreliableReport driverUnreliableReport)
        {
            driverUnreliableReport.Id = NextId();
            _reports.Add(driverUnreliableReport);
            _serializer.ToCSV(FilePath, _reports);
        }

        public void Delete(DriverUnreliableReport driverUnreliableReport)
        {
            DriverUnreliableReport found = _reports.Find(udr => udr.Id == driverUnreliableReport.Id);
            _reports.Remove(found);
            _serializer.ToCSV(FilePath, _reports);
        }

        public void Update(DriverUnreliableReport driverUnreliableReport)
        {
            int index = _reports.FindIndex(udr => driverUnreliableReport.Id == udr.Id);
            if (index != -1)
            {
                _reports[index] = driverUnreliableReport;
                _serializer.ToCSV(FilePath, _reports);
            }
        }

        public List<DriverUnreliableReport> GetAll()
        {
            return _reports;
        }

        public DriverUnreliableReport GetById(int id)
        {
            return _reports.Find(udr => udr.Id == id);
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }

        public bool ReportAlreadyExists(int driverId, int driveId, int touristId)
        {
            var matchingReportExists = _reports.Any(report =>
                report.DriverId == driverId &&
                report.DriveId == driveId &&
                report.TouristId == touristId);

            return matchingReportExists;
        }
    }
}
