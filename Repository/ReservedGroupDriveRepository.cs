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
    public class ReservedGroupDriveRepository : IReservedGroupDriveRepository
    {
        private const string FilePath = "../../../Resources/Data/reservedGroupDrives.csv";

        private readonly Serializer<GroupDrive> _serializer;
        private List<IObserver> observers;
        private List<GroupDrive> _groupDrives;

        public ReservedGroupDriveRepository()
        {
            _serializer = new Serializer<GroupDrive>();
            _groupDrives = _serializer.FromCSV(FilePath);
        }

        public GroupDrive Save(GroupDrive groupDrive)
        {
            groupDrive.Id = NextId();
            _groupDrives.Add(groupDrive);
            _serializer.ToCSV(FilePath, _groupDrives);
            return groupDrive;
        }

        public int NextId()
        {
            if (_groupDrives.Count == 0)
            {
                return 0;
            }
            return _groupDrives.Max(gd => gd.Id) + 1;
        }

        public void Create(GroupDrive groupDrive)
        {
            groupDrive.Id = NextId();
            _groupDrives.Add(groupDrive);
            _serializer.ToCSV(FilePath, _groupDrives);
        }

        public void Delete(GroupDrive groupDrive)
        {
            GroupDrive found = _groupDrives.Find(gd => gd.Id == groupDrive.Id);
            _groupDrives.Remove(found);
            _serializer.ToCSV(FilePath, _groupDrives);
        }

        public void Update(GroupDrive groupDrive)
        {
            int index = _groupDrives.FindIndex(gd => groupDrive.Id == gd.Id);
            if (index != -1)
            {
                _groupDrives[index] = groupDrive;
                _serializer.ToCSV(FilePath, _groupDrives);
            }
        }

        public List<GroupDrive> GetAll()
        {
            return _groupDrives;
        }

        public GroupDrive GetById(int id)
        {
            return _groupDrives.Find(gd => gd.Id == id);
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
