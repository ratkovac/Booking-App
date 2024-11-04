using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.WPF.View.Tourist.Pages;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.Repository
{
    public class DriveRepository : IDriveRepository
    {
        private const string FilePath = "../../../Resources/Data/drives.csv";

        private readonly Serializer<Drive> _serializer;
        private List<IObserver> observers;
        private List<Drive> _drives;

        public DriveRepository()
        {
            _serializer = new Serializer<Drive>();
            _drives = _serializer.FromCSV(FilePath);
        }

        public Drive Save(Drive drive)
        {
            drive.Id = NextId();
            _drives.Add(drive);
            _serializer.ToCSV(FilePath, _drives);
            return drive;
        }

        public int NextId()
        {
            if (_drives.Count == 0)
            {
                return 0;
            }
            return _drives.Max(fd => fd.Id) + 1;
        }

        public void Create(Drive drive)
        {
            drive.Id = NextId();
            _drives.Add(drive);
            _serializer.ToCSV(FilePath, _drives);
        }

        public void Delete(Drive drive)
        {
            Drive found = _drives.Find(fd => fd.Id == drive.Id);
            _drives.Remove(found);
            _serializer.ToCSV(FilePath, _drives);
        }

        public void Update(Drive drive)
        {
            int index = _drives.FindIndex(fd => drive.Id == fd.Id);
            if (index != -1)
            {
                _drives[index] = drive;
                _serializer.ToCSV(FilePath, _drives);
            }
        }

        public List<Drive> GetAll()
        {
            return _drives;
        }

        public Drive GetById(int id)
        {
            return _drives.Find(fd => fd.Id == id);
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
