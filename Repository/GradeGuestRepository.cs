using BookingApp.Model;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Repository
{
    public class GradeGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/guestgrades.csv";

        private readonly Serializer<GradeGuest> _serializer;

        private List<GradeGuest> _grades;
        public Subject GradeGuestSubject;

        public GradeGuestRepository()
        {
            _serializer = new Serializer<GradeGuest>();
            _grades = _serializer.FromCSV(FilePath);
            GradeGuestSubject = new Subject();
        }

        public List<GradeGuest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public GradeGuest Save(GradeGuest gradeGuest)
        {
            gradeGuest.Id = NextId();
            _grades = _serializer.FromCSV(FilePath);
            _grades.Add(gradeGuest);
            GradeGuestSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _grades);
            return gradeGuest;
        }

        public int NextId()
        {
            _grades = _serializer.FromCSV(FilePath);
            if (_grades.Count < 1)
            {
                return 1;
            }
            return _grades.Max(c => c.Id) + 1;
        }

        public void Delete(GradeGuest gradeGuest)
        {
            _grades = _serializer.FromCSV(FilePath);
            GradeGuest founded = _grades.Find(c => c.Id == gradeGuest.Id);
            if (founded != null)
            {
                _grades.Remove(founded);
            }
            GradeGuestSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _grades);
        }

        public GradeGuest Update(GradeGuest gradeGuest)
        {
            _grades = _serializer.FromCSV(FilePath);
            GradeGuest current = _grades.Find(c => c.Id == gradeGuest.Id);
            int index = _grades.IndexOf(current);
            _grades.Remove(current);
            _grades.Insert(index, gradeGuest);
            _serializer.ToCSV(FilePath, _grades);
            GradeGuestSubject.NotifyObservers();
            return gradeGuest;
        }
        /*public List<Accommodation> GetByUser(User user)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            return _accommodations.FindAll(c => c.User.Id == user.Id);
        }*/
        public GradeGuest? GetByID(int gradeId)
        {
            return _grades.Find(c => c.Id == gradeId);

        }
        public void Subscribe(IObserver observer)
        {

        }
    }
}
