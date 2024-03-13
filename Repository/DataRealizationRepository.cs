using BookingApp.Model;
using BookingApp.Serializer;
using System;
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
