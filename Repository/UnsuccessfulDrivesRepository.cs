using BookingApp.Model;
using BookingApp.Serializer;
using System.Collections.Generic;

namespace BookingApp.Repository
{
    public class UnsuccessfulDrivesRepository
    {
        private const string FilePath = "../../../Resources/Data/unsuccessfulDrives.csv";
        private readonly Serializer<Drive> _serializer;
        private List<Drive> _unsuccessfulDrives;

        public UnsuccessfulDrivesRepository()
        {
            _serializer = new Serializer<Drive>();
            _unsuccessfulDrives = _serializer.FromCSV(FilePath);
        }

        public List<Drive> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(Drive drive)
        {
            _unsuccessfulDrives = _serializer.FromCSV(FilePath);
            _unsuccessfulDrives.Add(drive);
            _serializer.ToCSV(FilePath, _unsuccessfulDrives);
        }

        public void Delete(Drive drive)
        {
            _unsuccessfulDrives = _serializer.FromCSV(FilePath);
            Drive founded = _unsuccessfulDrives.Find(c => c.Id == drive.Id);
            if (founded != null)
            {
                _unsuccessfulDrives.Remove(founded);
            }
            _serializer.ToCSV(FilePath, _unsuccessfulDrives);
        }
    }
}
