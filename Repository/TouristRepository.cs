using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.View;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    internal class TouristRepository : ITouristRepository
    {
        private const string FilePath = "../../../Resources/Data/tourists.csv";

        private readonly Serializer<Tourist> _serializer;
        private List<IObserver> observers;
        private List<Tourist> _tourists;

        public TouristRepository()
        {
            _serializer = new Serializer<Tourist>();
            _tourists = _serializer.FromCSV(FilePath);
        }

        public Tourist Save(Tourist tourist)
        {
            tourist.Id = NextId();
            _tourists.Add(tourist);
            _serializer.ToCSV(FilePath, _tourists);
            return tourist;
        }

        public int NextId()
        {
            if (_tourists.Count == 0)
            {
                return 0;
            }
            return _tourists.Max(t => t.Id) + 1;
        }

        public void Create(Tourist tourist)
        {
            tourist.Id = NextId();
            _tourists.Add(tourist);
            _serializer.ToCSV(FilePath, _tourists);
        }

        public void Delete(Tourist tourist)
        {
            Tourist found = _tourists.Find(t => t.Id == tourist.Id);
            _tourists.Remove(found);
            _serializer.ToCSV(FilePath, _tourists);
        }

        public void Update(Tourist tourist)
        {
            int index = _tourists.FindIndex(t => tourist.Id == t.Id);
            if (index != -1)
            {
                _tourists[index] = tourist;
                _serializer.ToCSV(FilePath, _tourists);
            }
        }

        public List<Tourist> GetAll()
        {
            return _tourists;
        }

        public Tourist GetById(int id)
        {
            return _tourists.Find(t => t.Id == id);
        }

        public Tourist GetByUserId(int userId)
        {
            return _tourists.FirstOrDefault(g => g.UserId == userId);
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
