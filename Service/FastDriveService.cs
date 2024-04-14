using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class FastDriveService
    {
        private IFastDriveRepository fastDriveRepository;
        public FastDriveService()
        {
            fastDriveRepository = Injector.CreateInstance<IFastDriveRepository>();
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
        public void Subscribe(IObserver observer)
        {
            fastDriveRepository.Subscribe(observer);
        }
    }
}
