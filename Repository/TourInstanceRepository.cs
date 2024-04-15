using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourInstanceRepository : ITourInstanceRepository
    {
        private const string FilePath = "../../../Resources/Data/tourInstances.csv";

        private readonly Serializer<TourInstance> _serializer;

        private List<TourInstance> _tourInstances;

        public TourInstanceRepository()
        {
            _serializer = new Serializer<TourInstance>();
            _tourInstances = _serializer.FromCSV(FilePath);
        }

        public List<TourInstance> GetAll()
        {
            return _tourInstances;
        }

        public List<TourInstance> GetFinishedTourInstances()
        {
            List<TourInstance> finishedTourInstances = new List<TourInstance>();
            List<TourInstance> allTourInstances = GetAll();

            foreach (TourInstance tourInstance in allTourInstances)
            {
                if (tourInstance.State == TourInstanceState.Finished)
                {
                    finishedTourInstances.Add(tourInstance);
                }
            }

            return finishedTourInstances;
        }


        public List<TourInstance> GetAllById(int tourId)
        {
            return _serializer.FromCSV(FilePath).Where(ti => ti.TourId == tourId).ToList(); ;
        }

        public TourInstance Save(TourInstance tourInstance)
        {
            tourInstance.Id = NextId();
            _tourInstances.Add(tourInstance);
            _serializer.ToCSV(FilePath, _tourInstances);
            return tourInstance;
        }

        public int NextId()
        {
            if (_tourInstances.Count < 1)
            {
                return 1;
            }
            return _tourInstances.Max(ti => ti.Id) + 1;
        }

        public void Create(TourInstance tourInstance)
        {
            tourInstance.Id = NextId();
            _tourInstances.Add(tourInstance);
            _serializer.ToCSV(FilePath, _tourInstances);
        }

        public void Delete(TourInstance tourInstance)
        {
            TourInstance found = _tourInstances.Find(ti => ti.Id == tourInstance.Id);
            _tourInstances.Remove(found);
            _serializer.ToCSV(FilePath, _tourInstances);
        }

        public void Update(TourInstance tourInstance)
        {
            TourInstance current = _tourInstances.Find(ti => ti.Id == tourInstance.Id);
            if (current != null)
            {
                int index = _tourInstances.IndexOf(current);
                _tourInstances.RemoveAt(index);
                _tourInstances.Insert(index, tourInstance);
                _serializer.ToCSV(FilePath, _tourInstances);
            }
        }
        public List<TourInstance> GetAllTourInstancesByTour(Tour tour)
        {
            return _tourInstances.Where(r => r.Id == tour.Id).ToList();
        }
        public TourInstance GetById(int id)
        {
            return _tourInstances.Find(r => r.Id == id);
        }

        public TourInstance GetByIdAndDate(int tourId, DateTime date)
        {
            return _serializer.FromCSV(FilePath).FirstOrDefault(tourInstance => tourInstance.TourId == tourId && tourInstance.StartTime == date);
        }

        public List<TourInstance> GetToursForToday()
        {
            DateTime today = DateTime.Now.Date;
            string formattedToday = today.ToString("M/d/yyyy");

            List<int> tourInstanceIds = GetTourIdsForToday(formattedToday);
            return _tourInstances.Where(t => tourInstanceIds.Contains(t.Id)).ToList();
        }

        private List<int> GetTourIdsForToday(string formattedToday)
        {
            string[] lines = File.ReadAllLines(FilePath);
            List<int> tourInstanceIds = new List<int>();

            foreach (string line in lines)
            {
                string[] values = line.Split('|');
                string[] dateParts = values[3].Split(' ')[0].Split('/'); // Razdvajanje delova datuma
                string dateFormat = dateParts[0].PadLeft(1, '0') + "/" + dateParts[1].PadLeft(1, '0') + "/" + dateParts[2].PadLeft(4, '0'); // Formatiranje datuma u "m/d/yyyy" format

                if (dateFormat == formattedToday)
                {
                    int tourInstanceId = Convert.ToInt32(values[0]);
                    tourInstanceIds.Add(tourInstanceId);
                }
            }

            return tourInstanceIds;
        }

    }
}
