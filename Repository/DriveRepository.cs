using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.View.Tourist.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.Repository
{
    public class DriveRepository
    {
        private const string FilePath = "../../../Resources/Data/drives.csv";

        private readonly Serializer<Drive> _serializer;

        private List<Drive> _drives;

        public DriveRepository()
        {
            _serializer = new Serializer<Drive>();
            _drives = _serializer.FromCSV(FilePath);
        }

        public List<Drive> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Drive Save(Drive drive)
        {
            drive.Id = NextId();
            _drives = _serializer.FromCSV(FilePath);
            _drives.Add(drive);
            _serializer.ToCSV(FilePath, _drives);
            return drive;
        }

        public int NextId()
        {
            _drives = _serializer.FromCSV(FilePath);
            if (_drives.Count < 1)
            {
                return 1;
            }
            return _drives.Max(c => c.Id) + 1;
        }

        public void Delete(Drive drive)
        {
            _drives = _serializer.FromCSV(FilePath);
            Drive founded = _drives.Find(c => c.Id == drive.Id);
            if (founded != null)
            {
                _drives.Remove(founded);
            }
            _serializer.ToCSV(FilePath, _drives);
        }

        public Drive Update(Drive drive)
        {
            _drives = _serializer.FromCSV(FilePath);
            Drive current = _drives.Find(c => c.Id == drive.Id);
            int index = _drives.IndexOf(current);
            _drives.Remove(current);
            _drives.Insert(index, drive);
            _serializer.ToCSV(FilePath, _drives);
            return drive;
        }

        public List<Drive> GetDrivesByDriver(User user)
        {
            _drives = _serializer.FromCSV(FilePath);
            List<Drive> filteredDrives = _drives.FindAll(c => c.Driver.Id == user.Id);
            return new List<Drive>(filteredDrives);
        }

        public List<Drive> GetByDriverId(int driverId)
        {
            _drives = _serializer.FromCSV(FilePath);
            return _drives.FindAll(r => r.DriverId == driverId);
        }

        public List<Drive> GetByTourist(int guestId)
        {
            _drives = _serializer.FromCSV(FilePath);
            return _drives.FindAll(r => r.GuestId == guestId);
        }

        public List<Drive> GetDrivesForToday()
        {
            _drives = _serializer.FromCSV(FilePath);
            return _drives.FindAll(c => c.Date.Date == DateTime.Today);
        }
    }
}
