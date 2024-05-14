using BookingApp.Command;
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
using System.Windows;
using System.Windows.Input;

namespace BookingApp.View.ViewModel.Owner
{
    public class AccommodationStatisticViewModel : IObserver
    {
        public AccommodationDTO SelectedAccommodation { get; set; }
        private AccommodationEventService accommodationEventService;
        public ObservableCollection<YearAccommodationStatisticDTO> yearStatistic;
        public AccommodationStatisticViewModel(AccommodationDTO selectedAccommodation)
        {
            SelectedAccommodation = selectedAccommodation;
            accommodationEventService = new AccommodationEventService();
            yearStatistic = new ObservableCollection<YearAccommodationStatisticDTO>();
            Update();
        }

        private void ByYear()
        {
            yearStatistic.Clear();
            foreach(var year in accommodationEventService.AllYears(SelectedAccommodation.Id))
            {
                yearStatistic.Add(new YearAccommodationStatisticDTO
                {
                    Year = year,
                    NumberOfReservations = accommodationEventService.getStatisticByYear(year,EventEnum.EventType.Reserved, SelectedAccommodation.Id),
                    NumberOfMovedReservations = accommodationEventService.getStatisticByYear(year,EventEnum.EventType.Moved, SelectedAccommodation.Id),
                    NumberOfCancelledReservations = accommodationEventService.getStatisticByYear(year, EventEnum.EventType.Cancelled, SelectedAccommodation.Id),
                    NumberOfRenovationProposal = accommodationEventService.getStatisticByYear(year, EventEnum.EventType.RenovationProposal, SelectedAccommodation.Id)
                });
            }
        }
        public void Update()
        {
            ByYear();
        }
    }
}
