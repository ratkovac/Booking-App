using BookingApp.Model;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository.RepositoryInterface;

namespace BookingApp.Repository
{
    public class SuperGuestRepository : ISuperGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/superguest.csv";

        private readonly Serializer<SuperGuest> _serializer;

        private List<SuperGuest> _superGuests;

        public Subject SuperGuestSubject;

        public SuperGuestRepository()
        {
            _serializer = new Serializer<SuperGuest>();
            _superGuests = _serializer.FromCSV(FilePath);
            SuperGuestSubject = new Subject();
        }

        public List<SuperGuest> GetAll()
        {
            return _superGuests;
        }

        public int NextId()
        {
            _superGuests = _serializer.FromCSV(FilePath);
            if (_superGuests.Count < 1)
            {
                return 1;
            }
            return _superGuests.Max(c => c.Id) + 1;
        }

        public void Create(SuperGuest entity)
        {
            entity.Id = NextId();
            _superGuests = _serializer.FromCSV(FilePath);
            _superGuests.Add(entity);
            _serializer.ToCSV(FilePath, _superGuests);
        }


        public void Delete(SuperGuest superGuest)
        {
            _superGuests = _serializer.FromCSV(FilePath);
            SuperGuest founded = _superGuests.Find(c => c.Id == superGuest.Id);
            if (founded != null)
            {
                _superGuests.Remove(founded);
            }
            _serializer.ToCSV(FilePath, _superGuests);
        }

        public SuperGuest GetById(int key)
        {
            return _superGuests.Find(c => c.Id == key);
        }

        public void Update(SuperGuest superGuest)
        {
            _superGuests = _serializer.FromCSV(FilePath);
            SuperGuest current = _superGuests.Find(c => c.Id == superGuest.Id);
            int index = _superGuests.IndexOf(current);
            _superGuests.Remove(current);
            _superGuests.Insert(index, superGuest);
            _serializer.ToCSV(FilePath, _superGuests);
        }
        public SuperGuest GetByID(int accommodationId)
        {
            return _superGuests.Find(c => c.Id == accommodationId);

        }
    }
}
