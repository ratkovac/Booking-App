using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Repository;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class DriverUnreliableReportService
    {
        private IDriverUnreliableReportRepository driverReportRepository;
        public DriverUnreliableReportService()
        {
            driverReportRepository = Injector.CreateInstance<IDriverUnreliableReportRepository>();
        }
        public int NextId()
        {
            return driverReportRepository.NextId();
        }
        public List<DriverUnreliableReport> GetAll()
        {
            return driverReportRepository.GetAll();
        }
        public DriverUnreliableReport GetById(int id)
        {
            return driverReportRepository.GetById(id);
        }
        public void Create(DriverUnreliableReport driverUnreliableReport)
        {
            driverReportRepository.Create(driverUnreliableReport);
        }
        public void Delete(DriverUnreliableReport driverUnreliableReport)
        {
            driverReportRepository.Delete(driverUnreliableReport);
        }
        public void Update(DriverUnreliableReport driverUnreliableReport)
        {
            driverReportRepository.Update(driverUnreliableReport);
        }
        public void Subscribe(IObserver observer)
        {
            driverReportRepository.Subscribe(observer);
        }
        public bool ReportAlreadyExists(int driverId, int driveId, int touristId)
        {
            return driverReportRepository.ReportAlreadyExists(driverId, driveId, touristId);
        }
    }
}
