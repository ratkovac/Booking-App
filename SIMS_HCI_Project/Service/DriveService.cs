using BookingApp.DependencyInjection;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Repository;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;

namespace BookingApp.Service
{
    public class DriveService
    {
        private IDriveRepository driveRepository;

        public DriveService()
        {
            driveRepository = Injector.CreateInstance<IDriveRepository>();
        }
        public int NextId()
        {
            return driveRepository.NextId();
        }
        public List<Drive> GetAllDrives()
        {
            return driveRepository.GetAll();
        }
        public Drive GetDriveById(int id)
        {
            return driveRepository.GetById(id);
        }
        public void Create(Drive drive)
        {
            driveRepository.Create(drive);
        }
        public void Delete(Drive drive)
        {
            driveRepository.Delete(drive);
        }
        public void Update(Drive drive)
        {
            driveRepository.Update(drive);
        }
        public void Subscribe(IObserver observer)
        {
            driveRepository.Subscribe(observer);
        }
    }
}
