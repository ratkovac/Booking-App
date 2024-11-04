using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BookingApp.Repository.RepositoryInterface;

namespace BookingApp.Repository
{
    public class CheckPointRepository : ICheckPointRepository
    {
        private const string FilePath = "../../../Resources/Data/checkpoints.csv";

        private readonly Serializer<CheckPoint> _serializer;

        private List<CheckPoint> _checkPoints;

        public CheckPointRepository()
        {
            _serializer = new Serializer<CheckPoint>();
            _checkPoints = _serializer.FromCSV(FilePath);
        }

        public List<CheckPoint> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public CheckPoint GetById(int id)
        {
            return _checkPoints.Find(r => r.Id == id);
        }

        public CheckPoint Save(CheckPoint checkPoint)
        {
            checkPoint.Id = NextId();
            _checkPoints = _serializer.FromCSV(FilePath);
            _checkPoints.Add(checkPoint);
            _serializer.ToCSV(FilePath, _checkPoints);
            return checkPoint;
        }

        public void Create(CheckPoint checkPoint)
        {
            checkPoint.Id = NextId();
            _checkPoints = _serializer.FromCSV(FilePath);
            _checkPoints.Add(checkPoint);
            _serializer.ToCSV(FilePath, _checkPoints);
        }

        public int NextId()
        {
            _checkPoints = _serializer.FromCSV(FilePath);
            if (_checkPoints.Count < 1)
            {
                return 1;
            }
            return _checkPoints.Max(c => c.Id) + 1;
        }

        public void Delete(CheckPoint checkPoint)
        {
            _checkPoints = _serializer.FromCSV(FilePath);
            CheckPoint founded = _checkPoints.Find(c => c.Id == checkPoint.Id);
            _checkPoints.Remove(founded);
            _serializer.ToCSV(FilePath, _checkPoints);
        }

        public void Update(CheckPoint checkPoint)
        {
            _checkPoints = _serializer.FromCSV(FilePath);
            CheckPoint current = _checkPoints.Find(c => c.Id == checkPoint.Id);
            int index = _checkPoints.IndexOf(current);
            _checkPoints.Remove(current);
            _checkPoints.Insert(index, checkPoint);
            _serializer.ToCSV(FilePath, _checkPoints);
        }

        private string[] ReadLinesFromFile(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška prilikom čitanja linija iz datoteke: " + ex.Message);
                return new string[0];
            }
        }

        private CheckPoint ParseLineToCheckPoint(string line)
        {
            string[] values = line.Split('|');

            int id;
            if (!int.TryParse(values[0], out id))
            {
                return null;
            }

            string name = values[1];
            int checkpointTourId;
            if (!int.TryParse(values[2], out checkpointTourId))
            {
                return null;
            }

            return new CheckPoint
            {
                Id = id,
                PointText = name,
                TourId = checkpointTourId
            };
        }

        public List<CheckPoint> GetCheckPoints(int tourId)
        {
            List<CheckPoint> checkPoints = new List<CheckPoint>();

            string[] lines = ReadLinesFromFile(FilePath);

            foreach (string line in lines)
            {
                CheckPoint checkPoint = ParseLineToCheckPoint(line);
                if (checkPoint != null && checkPoint.TourId == tourId)
                {
                    checkPoints.Add(checkPoint);
                }
            }

            return checkPoints;
        }

        public List<CheckPoint> GetAllByTourId(int tourId)
        {
            var allCheckpoints = GetAll();
            var checkpointsForTour = allCheckpoints.Where(checkpoint => checkpoint.TourId == tourId).ToList();


            return checkpointsForTour;
        }

    }
}
