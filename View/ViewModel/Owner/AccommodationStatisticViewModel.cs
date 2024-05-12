using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.View.ViewModel.Owner
{
    public class AccommodationStatisticViewModel : IObserver
    {
        public AccommodationDTO SelectedAccommodation { get; }
        private AccommodationReservationService accommodationReservationService;
        private AccommodationEventService accommodationEventService;
        public ObservableCollection<YearAccommodationStatisticDTO> yearStatistic;
        public AccommodationStatisticViewModel(AccommodationDTO selectedAccommodation)
        {
            SelectedAccommodation = selectedAccommodation;
            accommodationReservationService = new AccommodationReservationService();
            accommodationEventService = new AccommodationEventService();
            yearStatistic = new ObservableCollection<YearAccommodationStatisticDTO>();
            Update();
        }
        private List<int> AllYears(int accommodationId)
        {
            HashSet<int> uniqueYears = new HashSet<int>();
            foreach (var eventt in accommodationEventService.GetByAccommodationId(accommodationId))
            {
                uniqueYears.Add(eventt.EventDate.Year);
            }
            List<int> years = uniqueYears.ToList();
            return years;
        }
        private int getReservationsByYear(int year)
        {
            int numberOfReservations = 0;
            foreach (var reserved in accommodationEventService.GetAll())
            {
                if(reserved.Accommodation.Id == SelectedAccommodation.Id)
                {
                    if (reserved.EventDate.Year == year && reserved.EventType.Equals(EventEnum.EventType.Reserved))
                    {
                        numberOfReservations++;
                    }
                }
            }
            return numberOfReservations;
        }
        private int getCancelledByYear(int year)
        {
            int numberOfCancelledReservations = 0;
            foreach (var reserved in accommodationEventService.GetAll())
            {
                if (reserved.Accommodation.Id == SelectedAccommodation.Id)
                {
                    if (reserved.EventDate.Year == year && reserved.EventType.Equals(EventEnum.EventType.Cancelled))
                    {
                        numberOfCancelledReservations++;
                    }
                }
            }
            return numberOfCancelledReservations;
        }
        private int getMovedByYear(int year)
        {
            int numberOfMovedReservations = 0;
            foreach (var reserved in accommodationEventService.GetAll())
            {
                if (reserved.Accommodation.Id == SelectedAccommodation.Id)
                {
                    if (reserved.EventDate.Year == year && reserved.EventType.Equals(EventEnum.EventType.Moved))
                    {
                        numberOfMovedReservations++;
                    }
                }
            }
            return numberOfMovedReservations;
        }
        private int getRenovationProposalByYear(int year)
        {
            int numberOfRenovationProposal = 0;
            foreach (var reserved in accommodationEventService.GetAll())
            {
                if (reserved.Accommodation.Id == SelectedAccommodation.Id)
                {
                    if (reserved.EventDate.Year == year && reserved.EventType.Equals(EventEnum.EventType.RenovationProposal))
                    {
                        numberOfRenovationProposal++;
                    }
                }
            }
            return numberOfRenovationProposal;
        }
        private void ByYear()
        {
            yearStatistic.Clear();
            foreach(var year in AllYears(SelectedAccommodation.Id))
            {
                yearStatistic.Add(new YearAccommodationStatisticDTO
                {
                    Year = year,
                    NumberOfReservations = getReservationsByYear(year),
                    NumberOfMovedReservations = getMovedByYear(year),
                    NumberOfCancelledReservations = getCancelledByYear(year),
                    NumberOfRenovationProposal = getRenovationProposalByYear(year)
                });
            }
        }
        public void Update()
        {
            ByYear();
        }
    }
}
