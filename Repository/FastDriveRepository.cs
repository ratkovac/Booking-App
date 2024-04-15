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
    public class FastDriveRepository : IFastDriveRepository
    {
        private const string FilePath = "../../../Resources/Data/fastDrives.csv";

        private readonly Serializer<FastDrive> _serializer;
        private List<IObserver> observers;
        private List<FastDrive> _fastDrives;

        public FastDriveRepository()
        {
            _serializer = new Serializer<FastDrive>();
            _fastDrives = _serializer.FromCSV(FilePath);
        }

        public FastDrive Save(FastDrive fastDrive)
        {
            fastDrive.Id = NextId();
            _fastDrives.Add(fastDrive);
            _serializer.ToCSV(FilePath, _fastDrives);
            return fastDrive;
        }

        public int NextId()
        {
            if (_fastDrives.Count == 0)
            {
                return 0;
            }
            return _fastDrives.Max(fd => fd.Id) + 1;
        }

        public void Create(FastDrive fastDrive)
        {
            fastDrive.Id = NextId();
            _fastDrives.Add(fastDrive);
            _serializer.ToCSV(FilePath, _fastDrives);
        }

        public void Delete(FastDrive fastDrive)
        {
            FastDrive found = _fastDrives.Find(fd => fd.Id == fastDrive.Id);
            _fastDrives.Remove(found);
            _serializer.ToCSV(FilePath, _fastDrives);
        }

        public void Update(FastDrive fastDrive)
        {
            int index = _fastDrives.FindIndex(fd => fastDrive.Id == fd.Id);
            if (index != -1)
            {
                _fastDrives[index] = fastDrive;
                _serializer.ToCSV(FilePath, _fastDrives);
            }
        }

        public List<FastDrive> GetAll()
        {
            return _fastDrives;
        }

        public FastDrive GetById(int id)
        {
            return _fastDrives.Find(fd => fd.Id == id);
        }

        public List<FastDrive> GetByTourist(int touristId)
        {
            _fastDrives = _serializer.FromCSV(FilePath);
            return _fastDrives.FindAll(r => r.GuestId == touristId);
        }

        public List<FastDrive> GetDrivesForToday()
        {
            _fastDrives = _serializer.FromCSV(FilePath);
            return _fastDrives.FindAll(fd => fd.Date.Date == DateTime.Today);
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
    }
}
