using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using BookingApp.View.Tourist.Pages;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    internal class GradeTourRepository : IGradeTourRepository
    {

        private const string FilePath = "../../../Resources/Data/gradeTour.csv";

        private readonly Serializer<GradeTour> _serializer;
        private List<IObserver> observers;
        private List<GradeTour> _gradeTour;

        public GradeTourRepository()
        {
            _serializer = new Serializer<GradeTour>();
            _gradeTour = _serializer.FromCSV(FilePath);
        }

        public GradeTour Save(GradeTour gradeTours)
        {
            gradeTours.Id = NextId();
            _gradeTour.Add(gradeTours);
            _serializer.ToCSV(FilePath, _gradeTour);
            return gradeTours;
        }

        public int NextId()
        {
            if (_gradeTour.Count == 0)
            {
                return 0;
            }
            return _gradeTour.Max(t => t.Id) + 1;
        }
        public void Create(GradeTour gradeTours)
        {
            gradeTours.Id = NextId();
            _gradeTour.Add(gradeTours);
            _serializer.ToCSV(FilePath, _gradeTour);
        }
        public void Delete(GradeTour gradeTours)
        {
            GradeTour found = _gradeTour.Find(ti => ti.Id == gradeTours.Id);
            _gradeTour.Remove(found);
            _serializer.ToCSV(FilePath, _gradeTour);
        }
        public void Update(GradeTour gradeTours)
        {
            int index = _gradeTour.FindIndex(t => gradeTours.Id == t.Id);
            if (index != -1)
            {
                _gradeTour[index] = gradeTours;
                _serializer.ToCSV(FilePath, _gradeTour);
            }
        }
        public List<GradeTour> GetAll()
        {
            return _gradeTour;
        }
        public GradeTour GetById(int id)
        {
            return _gradeTour.Find(t => t.Id == id);
        }
        public List<GradeTour> GetAllRatingsByTour(BookingApp.Model.TourReservation tourReservation)
        {
            return _gradeTour.Where(r => r.Id == tourReservation.Id).ToList();
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
