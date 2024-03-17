using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BookingApp.Repository
{
    public class CheckPointRepository
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

        public CheckPoint Save(CheckPoint checkPoint)
        {
            checkPoint.Id = NextId();
            _checkPoints = _serializer.FromCSV(FilePath);
            _checkPoints.Add(checkPoint);
            _serializer.ToCSV(FilePath, _checkPoints);
            return checkPoint;
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

        public CheckPoint Update(CheckPoint checkPoint)
        {
            _checkPoints = _serializer.FromCSV(FilePath);
            CheckPoint current = _checkPoints.Find(c => c.Id == checkPoint.Id);
            int index = _checkPoints.IndexOf(current);
            _checkPoints.Remove(current);
            _checkPoints.Insert(index, checkPoint);
            _serializer.ToCSV(FilePath, _checkPoints);
            return checkPoint;
        }

        public List<CheckPoint> GetCheckPoints(int tourId)
        {
            List<CheckPoint> checkPoints = new List<CheckPoint>();

            try
            {
                string[] lines = File.ReadAllLines(FilePath);

                // Preskoči prvu liniju (zaglavlje)
                foreach (string line in lines)
                {
                    string[] values = line.Split('|');

                    int id;
                    if (!int.TryParse(values[0], out id))
                    {
                        // Preskoči red ako prvi element nije validan ID
                        continue;
                    }

                    string name = values[1];
                    int checkpointTourId;
                    if (!int.TryParse(values[2], out checkpointTourId))
                    {
                        // Preskoči red ako treći element nije validan ID ture
                        continue;
                    }

                    // Proveri da li je tačka ture za traženu turu
                    if (checkpointTourId == tourId)
                    {
                        CheckPoint checkPoint = new CheckPoint
                        {
                            Id = id,
                            PointText = name,
                            TourId = checkpointTourId
                        };
                        checkPoints.Add(checkPoint);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška prilikom čitanja tačaka ture: " + ex.Message);
            }

            return checkPoints;
        }
    }
}
