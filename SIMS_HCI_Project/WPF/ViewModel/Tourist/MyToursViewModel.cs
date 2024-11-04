using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class MyToursViewModel
    {
        public ObservableCollection<TourInstanceViewModel> Tours { get; set; }
        private User _tourist;

        private TourInstanceService _tourInstanceService;
        private TourReservationService _tourReservationService;
        private TourGuestService _tourGuestService;
        private TourRepository _tourRepository;
        private CheckPointRepository _checkPointRepository;

        public MyToursViewModel(User loggedUser)
        {
            _tourist = loggedUser;
            Tours = new ObservableCollection<TourInstanceViewModel>();
            _tourRepository = new TourRepository();
            _tourGuestService = new TourGuestService();
            _tourReservationService = new TourReservationService();
            _tourInstanceService = new TourInstanceService();
            _checkPointRepository = new CheckPointRepository();
            CreateViewModels();
        }

        private void CreateViewModels()
        {
            foreach (var tourInstanceId in GetTourInstanceIds())
            {
                var tourInstance = _tourInstanceService.GetById(tourInstanceId);
                if (IsTourInstanceValid(tourInstance))
                {
                    var viewModel = CreateTourInstanceViewModel(tourInstance);
                    Tours.Add(viewModel);
                }
            }
        }

        private bool IsTourInstanceValid(TourInstance tourInstance)
        {
            if (tourInstance == null)
            {
                return false;
            }

            return tourInstance.State == TourInstanceState.Active;
        }

        private TourInstanceViewModel CreateTourInstanceViewModel(TourInstance tourInstance)
        {
            var tour = _tourRepository.GetById(tourInstance.TourId);
            var checkpoints = _checkPointRepository.GetAllByTourId(tour.Id);
            var tourGuests = _tourGuestService.GetAllByTouristForTourInstance(_tourist.Id, tourInstance.Id).ToList();

            return new TourInstanceViewModel
            {
                IsFinished = tourInstance.IsCompleted,
                Guests = tourGuests,
                Date = tourInstance.StartTime,
                Name = tour.Name,
                CheckpointNames = checkpoints.Select(cp => cp.PointText).ToList(),
                CurrentCheckpoint = tourInstance.CurrentCheckpoint,
            };
        }


        private List<int> GetTourInstanceIds()
        {
            var tourReservations = _tourReservationService.GetAllByUserId(_tourist.Id);
            var tourInstanceIds = tourReservations.Select(reservation => reservation.TourInstanceId).ToList();
            return tourInstanceIds;
        }
    }
}
