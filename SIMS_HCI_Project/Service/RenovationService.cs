using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class RenovationService
    {
        private IRenovationRepository _repository;
        private AccommodationReservationService accommodationReservationService = new AccommodationReservationService();
        public RenovationService()
        {
            _repository = Injector.CreateInstance<IRenovationRepository>();
        }
        public List<Renovations> GetAll()
        {
            return _repository.GetAll();
        }
        public void Delete(Renovations renovations)
        {
            _repository.Delete(renovations);
        }
        public void Create(Renovations entity)
        {
            _repository.Create(entity);
        }
        public List<(DateTime, DateTime)> GetAllReservations(int id)
        {
            List<(DateTime, DateTime)> allApartmentReservation = new List<(DateTime, DateTime)>();
            TimeOnly timeMidnight = new TimeOnly(0, 0);
            foreach (var reservation in accommodationReservationService.GetAllByID(id))
            {
                allApartmentReservation.Add((reservation.StartDate.ToDateTime(timeMidnight), reservation.EndDate.ToDateTime(timeMidnight)));
            }
            return allApartmentReservation;
        }
        public List<(DateTime, DateTime)> GetAllPossibleDates(DateTime startDate, DateTime endDate, int numberOfDays)
        {
            List<(DateTime, DateTime)> possibleDates = new List<(DateTime, DateTime)>();
            TimeSpan span = endDate - startDate;

            if (span.TotalDays < numberOfDays)
            {
                return possibleDates;
            }

            for (DateTime start = startDate; start <= endDate.AddDays(-numberOfDays); start = start.AddDays(1))
            {
                DateTime end = start.AddDays(numberOfDays);
                possibleDates.Add((start, end));
            }

            return possibleDates;
        }
        public List<(DateTime, DateTime)> GetNonOverlappingRenovationDates(List<(DateTime, DateTime)> renovationDates, List<(DateTime, DateTime)> reservationDates)
        {
            List<(DateTime, DateTime)> nonOverlappingDates = new List<(DateTime, DateTime)>();

            foreach (var renovationDate in renovationDates)
            {
                bool isOverlapping = false;
                foreach (var reservationDate in reservationDates)
                {
                    if (renovationDate.Item1 >= reservationDate.Item1 && renovationDate.Item1 <= reservationDate.Item2)
                        isOverlapping = true;
                    else if (renovationDate.Item1 <= reservationDate.Item1 && renovationDate.Item2 >= reservationDate.Item2)
                        isOverlapping = true;
                    else if (renovationDate.Item2 >= reservationDate.Item1 && renovationDate.Item2 <= reservationDate.Item2)
                        isOverlapping = true;
                }
                if (!isOverlapping)
                {
                    if (!nonOverlappingDates.Any(date => date.Item1 == renovationDate.Item1 && date.Item2 == renovationDate.Item2))
                    {
                        nonOverlappingDates.Add(renovationDate);
                    }
                }
            }
            return nonOverlappingDates;
        }
        public int DaysToCancel(DateOnly startDate)
        {
            TimeOnly timeMidnight = new TimeOnly(0, 0);
            DateTime start = startDate.ToDateTime(timeMidnight);
            TimeSpan razlika = start.Subtract(DateTime.Today);
            return razlika.Days;
        }
        public void Subscribe(IObserver observer)
        {
            _repository.Subscribe(observer);
        }
    }
}
