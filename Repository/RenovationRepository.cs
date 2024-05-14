using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.View.NGuest;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class RenovationRepository : IRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/renovations.csv";
        private readonly Serializer<Renovations> _serializer;
        private List<Renovations> _renovations;
        public Subject RenovationSubject;
        public RenovationRepository()
        {
            _serializer = new Serializer<Renovations>();
            _renovations = _serializer.FromCSV(FilePath);
            RenovationSubject = new Subject();
        }

        public void Create(Renovations renovation)
        {
            renovation.Id = NextId();
            _renovations = _serializer.FromCSV(FilePath);
            _renovations.Add(renovation);
            RenovationSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _renovations);
        }

        public void Delete(Renovations renovations)
        {
            _renovations = _serializer.FromCSV(FilePath);
            Renovations founded = _renovations.Find(c => c.Id == renovations.Id);
            if (founded != null)
            {
                _renovations.Remove(founded);
            }
            RenovationSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _renovations);
        }

        public List<Renovations> GetAll()
        {
            return _renovations;
        }
        public Renovations GetById(int key)
        {
            throw new NotImplementedException();
        }

        public int NextId()
        {
            _renovations = _serializer.FromCSV(FilePath);
            if (_renovations.Count < 1)
            {
                return 1;
            }
            return _renovations.Max(c => c.Id) + 1;
        }

        public void Update(Renovations entity)
        {
            throw new NotImplementedException();
        }
    }
}
