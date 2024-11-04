using BookingApp.DTO;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DependencyInjection;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;

namespace BookingApp.Service
{
    public class SuggestReservationService
    {
        private IAccommodationReservationRepository _accommodationReservationRepository;

        public SuggestReservationService()
        {
            _accommodationReservationRepository = Injector.CreateInstance<IAccommodationReservationRepository>();
        }



        public ObservableCollection<AccommodationReservationDTO> FillReservationsDTO()
        {
            var reservationDTOs = new ObservableCollection<AccommodationReservationDTO>();

            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                var reservationDTO = new AccommodationReservationDTO(reservation);
                reservationDTOs.Add(reservationDTO);
            }

            return reservationDTOs;
        }

        public List<AccommodationReservationDTO> SuggestReservation(List<AccommodationReservationDTO> accommodationReservations, int reservationDays, List<AccommodationReservationDTO> availableAccommodationPeriods,
            DateOnly startDate, DateOnly endDate)
        {
            List<AccommodationReservationDTO> sortedAccommodationReservations =
                SortAccommodationReservations(accommodationReservations, startDate, endDate);

            FillGapsBetweenReservations(sortedAccommodationReservations, reservationDays, availableAccommodationPeriods);
            FillGapBeforeFirstReservation(sortedAccommodationReservations, startDate, reservationDays, availableAccommodationPeriods);
            FillGapAfterLastReservation(sortedAccommodationReservations, endDate, reservationDays, availableAccommodationPeriods);
            FillEntirePeriodIfNoReservations(sortedAccommodationReservations, startDate, endDate, reservationDays, availableAccommodationPeriods);

            return availableAccommodationPeriods;
        }

        private List<AccommodationReservationDTO> SortAccommodationReservations(List<AccommodationReservationDTO> accommodationReservations, DateOnly startDate, DateOnly endDate)
        {
            var sorted = accommodationReservations.OrderBy(reservation => reservation.StartDate).ToList();
            List<AccommodationReservationDTO> sortedAccommodationReservations = new List<AccommodationReservationDTO>();

            foreach (var reservation in sorted)
            {
                if (reservation.StartDate >= startDate && reservation.EndDate <= endDate)
                {
                    sortedAccommodationReservations.Add(reservation);
                }
            }

            return sortedAccommodationReservations;
        }

        private void FillGapsBetweenReservations(
            List<AccommodationReservationDTO> reservations,
            int reservationDays,
            List<AccommodationReservationDTO> availableAccommodationPeriods)
        {
            for (int i = 0; i < reservations.Count - 1; i++)
            {
                var gapDays = CalculateGapDays(reservations[i].EndDate, reservations[i + 1].StartDate);
                DateOnly nextStartDate = reservations[i].EndDate.AddDays(1);
                FillGap(nextStartDate, gapDays, reservationDays, availableAccommodationPeriods);
            }
        }

        private void FillGapBeforeFirstReservation(
            List<AccommodationReservationDTO> reservations,
            DateOnly periodStart, int reservationDays,
            List<AccommodationReservationDTO> availableAccommodationPeriods)
        {
            if (reservations.Any())
            {
                var firstReservationStartDate = reservations.First().StartDate;
                var gapDays = CalculateGapDays(periodStart, firstReservationStartDate, adjustForInclusiveEndDate: true);
                FillGap(periodStart, gapDays, reservationDays, availableAccommodationPeriods);
            }
        }

        private void FillGapAfterLastReservation(
            List<AccommodationReservationDTO> reservations,
            DateOnly periodEnd, int reservationDays,
            List<AccommodationReservationDTO> availableAccommodationPeriods)
        {
            if (reservations.Any())
            {
                var lastReservationEndDate = reservations.Last().EndDate;
                var gapDays = CalculateGapDays(lastReservationEndDate, periodEnd, adjustForInclusiveEndDate: false);
                FillGap(lastReservationEndDate.AddDays(1), gapDays, reservationDays, availableAccommodationPeriods);
            }
        }

        private void FillEntirePeriodIfNoReservations(
            List<AccommodationReservationDTO> reservations,
            DateOnly periodStart, DateOnly periodEnd, int reservationDays,
            List<AccommodationReservationDTO> availableAccommodationPeriods)
        {
            if (!reservations.Any())
            {
                var gapDays = CalculateGapDays(periodStart, periodEnd, adjustForInclusiveEndDate: true);
                FillGap(periodStart, gapDays, reservationDays, availableAccommodationPeriods);
            }
        }

        private int CalculateGapDays(DateOnly start, DateOnly end, bool adjustForInclusiveEndDate = false)
        {
            return (end.ToDateTime(new TimeOnly(0, 0)) - start.ToDateTime(new TimeOnly(0, 0))).Days + (adjustForInclusiveEndDate ? 1 : 0);
        }

        private void FillGap(
            DateOnly start, int gapDays,
            int reservationDays,
            List<AccommodationReservationDTO> availableAccommodationPeriods)
        {
            while (gapDays >= reservationDays)
            {
                var newReservationDTO = new AccommodationReservationDTO
                {
                    StartDate = start,
                    EndDate = start.AddDays(reservationDays - 1),
                    ReservationDays = reservationDays,
                };

                availableAccommodationPeriods.Add(newReservationDTO);

                start = start.AddDays(reservationDays);
                gapDays -= reservationDays;
            }
        }

        public List<AccommodationReservationDTO> CreateReservationsWithoutChecking(
            DateOnly startDate,
            DateOnly endDate,
            int reservationDays)
        {
            var newReservations = new List<AccommodationReservationDTO>();
            DateOnly currentDate = startDate;

            while (currentDate <= endDate)
            {
                var newReservationDTO = new AccommodationReservationDTO
                {
                    StartDate = currentDate,
                    EndDate = currentDate.AddDays(reservationDays - 1),
                    ReservationDays = reservationDays,
                };

                newReservations.Add(newReservationDTO);

                currentDate = currentDate.AddDays(reservationDays);
            }

            return newReservations;
        }

        public void Subscribe(IObserver observer)
        {
            _accommodationReservationRepository.Subscribe(observer);
        }
    }
}
