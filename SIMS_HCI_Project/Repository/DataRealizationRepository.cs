using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class DateRealizationRepository
    {
        private const string FilePath = "../../../Resources/Data/dateRealizations.csv";

        private readonly Serializer<DateRealization> _serializer;
        private List<DateRealization> _dateRealizations;

        public DateRealizationRepository()
        {
            _serializer = new Serializer<DateRealization>();
            _dateRealizations = _serializer.FromCSV(FilePath);
        }

        public List<DateRealization> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public List<DateRealization> GetAllByTourId(int tourId)
        {
            List<DateRealization> dateRealizations = new List<DateRealization>();

            string[] lines = ReadLinesFromFile(FilePath);

            foreach (string line in lines)
            {
                DateRealization dateRealization = ParseLineToDateRealization(line);
                if (dateRealization != null && dateRealization.TourId == tourId)
                {
                    dateRealizations.Add(dateRealization);
                }
            }

            return dateRealizations;
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

        private DateRealization ParseLineToDateRealization(string line)
        {
            string[] values = line.Split('|');

            int id;
            if (!int.TryParse(values[0], out id))
            {
                return null;
            }

            DateTime date;
            if (!DateTime.TryParse(values[1], out date))
            {
                return null;
            }

            int tourId;
            if (!int.TryParse(values[2], out tourId))
            {
                return null;
            }

            return new DateRealization
            {
                Id = id,
                Date = date,
                TourId = tourId
            };
        }



        public DateRealization Save(DateRealization dateRealization)
        {
            dateRealization.Id = NextId();
            _dateRealizations = _serializer.FromCSV(FilePath);
            _dateRealizations.Add(dateRealization);
            _serializer.ToCSV(FilePath, _dateRealizations);
            return dateRealization;
        }

        public int NextId()
        {
            _dateRealizations = _serializer.FromCSV(FilePath);
            if (_dateRealizations.Count < 1)
            {
                return 1;
            }
            return _dateRealizations.Max(d => d.Id) + 1;
        }

        public void Delete(DateRealization dateRealization)
        {
            _dateRealizations = _serializer.FromCSV(FilePath);
            DateRealization founded = _dateRealizations.Find(d => d.Id == dateRealization.Id);
            _dateRealizations.Remove(founded);
            _serializer.ToCSV(FilePath, _dateRealizations);
        }

        public DateRealization Update(DateRealization dateRealization)
        {
            _dateRealizations = _serializer.FromCSV(FilePath);
            DateRealization current = _dateRealizations.Find(d => d.Id == dateRealization.Id);
            int index = _dateRealizations.IndexOf(current);
            _dateRealizations.Remove(current);
            _dateRealizations.Insert(index, dateRealization);
            _serializer.ToCSV(FilePath, _dateRealizations);
            return dateRealization;
        }
    }
}
