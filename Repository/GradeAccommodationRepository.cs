using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.View.Owner;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository.RepositoryInterface;

namespace BookingApp.Repository
{
    public class GradeAccommodationRepository : IGradeAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationgrades.csv";

        private readonly Serializer<GradeAccommodation> _serializer;

        private List<GradeAccommodation> _gradeAccommodations;

        public Subject GradeAccommodationSubject;
        public GradeAccommodationRepository()
        {
            _serializer = new Serializer<GradeAccommodation>();
            _gradeAccommodations = _serializer.FromCSV(FilePath);
            GradeAccommodationSubject = new Subject();
        }
        public GradeAccommodation Save(GradeAccommodation GradeAccommodation)
        {
            GradeAccommodation.Id = NextId();
            _gradeAccommodations = _serializer.FromCSV(FilePath);
            _gradeAccommodations.Add(GradeAccommodation);
            _serializer.ToCSV(FilePath, _gradeAccommodations);
            return GradeAccommodation;
        }
        public List<GradeAccommodation> GetAll()
        {
            return _gradeAccommodations;
        }

        public int NextId()
        {
            _gradeAccommodations = _serializer.FromCSV(FilePath);
            if (_gradeAccommodations.Count < 1)
            {
                return 1;
            }
            return _gradeAccommodations.Max(c => c.Id) + 1;
        }

        public void Create(GradeAccommodation entity)
        {
            throw new NotImplementedException();
        }
        void IGenericRepository<GradeAccommodation, int>.Update(GradeAccommodation entity)
        {
            throw new NotImplementedException();
        }

        public void Update(GradeAccommodation entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(GradeAccommodation GradeAccommodation)
        {
            _gradeAccommodations = _serializer.FromCSV(FilePath);
            GradeAccommodation founded = _gradeAccommodations.Find(c => c.Id == GradeAccommodation.Id);
            if (founded != null)
            {
                _gradeAccommodations.Remove(founded);
            }
            _serializer.ToCSV(FilePath, _gradeAccommodations);
        }

        public GradeAccommodation GetById(int key)
        {
            throw new NotImplementedException();
        }
        public List<GradeAccommodation> GetAllByUser(int userId)
        {
            List<GradeAccommodation> gradesByUser = new List<GradeAccommodation>();

            foreach (GradeAccommodation grade in _gradeAccommodations)
            {
                if (grade.AccommodationReservation.User.Id == userId)
                {
                    gradesByUser.Add(grade);
                }
            }

            return gradesByUser;
        }
    }
}
