using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Service;
using BookingApp.View.Owner;
using CLI.Observer;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.View.ViewModel.Owner
{
    public class DelayRequestsViewModel : IObserver
    {
        public ObservableCollection<DelayReservationDTO> delayRequsets { get; set; }
        private User user { get; set; }

        private DelayReservationService delayReservationService;

        private List<DelayReservation> delayReservations;

        private AccommodationReservationService accommodationReservationService;
        private AccommodationEventService accommodationEventService;
        private List<AccommodationReservation> reservationsByAccommodation;
        public User LoggedInUser;
        private ImageRepository imageRepository;
        public ICommand AcceptCommand { get; private set; }
        public ICommand DeclineCommand { get; private set; }

        private int delayRequestsNumber = 0;
        public DelayRequestsViewModel()
        {
            // Inicijalizacija po potrebi
        }
        public DelayRequestsViewModel(User user)
        {
            this.user = user;
            LoggedInUser = user;
            delayReservationService = new DelayReservationService();
            delayReservationService.Subscribe(this);
            delayRequsets = new ObservableCollection<DelayReservationDTO>();
            delayReservations = new List<DelayReservation>();
            imageRepository = new ImageRepository();
            accommodationReservationService = new AccommodationReservationService();
            accommodationEventService = new AccommodationEventService();
            AcceptCommand = new RelayCommand<DelayReservationDTO>(ExecuteAcceptCommand);
            DeclineCommand = new RelayCommand<DelayReservationDTO>(ExecuteDeclineCommand);
            Update();
        }
        public User LoggedUser()
        {
            return LoggedInUser;
        }
        private void ExecuteAcceptCommand(DelayReservationDTO SelectedRequest)
        {
            AccommodationReservation accommodationReservation = accommodationReservationService.GetById(SelectedRequest.ReservationId);
            accommodationReservation.StartDate = SelectedRequest.NewStartDate;
            accommodationReservation.EndDate = SelectedRequest.NewEndDate;

            DelayReservation oldDelayReservation = delayReservationService.GetByID(SelectedRequest.Id);
            oldDelayReservation.Status = DelayReservationStatusEnum.Approved;
            accommodationReservationService.Update(accommodationReservation);
            delayReservationService.Update(oldDelayReservation);
            MessageBox.Show("Request is accepted!");

            AccommodationEvent accommodationEvent = new AccommodationEvent();
            accommodationEvent.EventDate = DateOnly.FromDateTime(DateTime.Now);
            accommodationEvent.EventType = EventEnum.EventType.Moved;
            accommodationEvent.Accommodation = oldDelayReservation.Reservation.Accommodation;
            accommodationEventService.Create(accommodationEvent);
            Update(); 
        }
        private void ExecuteDeclineCommand(DelayReservationDTO SelectedRequest)
        {
            DelayReservation oldDelayReservation = delayReservationService.GetByID(SelectedRequest.Id);
            oldDelayReservation.Status = DelayReservationStatusEnum.Declined;
            delayReservationService.Update(oldDelayReservation);

            AddRejectionCommentViewModel addRejectionCommentViewModel = new AddRejectionCommentViewModel();
            AddRejectionComment addRejectionComment = new AddRejectionComment(addRejectionCommentViewModel, SelectedRequest);
            addRejectionComment.Show();
            Update();
        }

        private List<int> allReservedDates()
        {
            List<int> allImpossibleDelays = new List<int>();
            Zahtevi();
            foreach (DelayReservation dr in delayReservations)
            {
                reservationsByAccommodation = accommodationReservationService.GetAllByID(dr.Reservation.Accommodation.Id);
                reservationsByAccommodation.Remove(dr.Reservation);
                foreach (AccommodationReservation ar in reservationsByAccommodation)
                {
                    if (!allImpossibleDelays.Contains(dr.Id))
                    {
                        if (dr.NewEndDate >= ar.StartDate && dr.NewEndDate <= ar.EndDate)
                            allImpossibleDelays.Add(dr.Id);
                        else if (dr.NewStartDate >= ar.StartDate && dr.NewStartDate <= ar.EndDate)
                            allImpossibleDelays.Add(dr.Id);
                        else if (dr.NewEndDate >= ar.EndDate && dr.NewStartDate <= ar.StartDate)
                            allImpossibleDelays.Add(dr.Id);
                    }
                }
                reservationsByAccommodation.Clear();
            }
            return allImpossibleDelays;
        }

        private void Zahtevi()
        {
            delayRequsets.Clear();
            foreach (var delayRequest in delayReservationService.GetAll())
            {
                if (delayRequest.Status.Equals(DelayReservationStatusEnum.Pending))
                {
                    delayRequestsNumber++;
                    delayReservations.Add(delayRequest);
                }
            }
        }
        public void allRequests()
        {
            delayRequsets.Clear();
            List<int> reservedDates = new List<int>();
            reservedDates = allReservedDates();
            foreach (var delayRequest in delayReservationService.GetAll())
            {
                    if (delayRequest.Status.Equals(DelayReservationStatusEnum.Pending))
                    {
                        string imagePath;
                        Image frontImage = imageRepository.GetByAccommodationId(delayRequest.Reservation.Accommodation.Id);
                        if (frontImage != null)
                        {
                            imagePath = frontImage.Path;
                        }
                        else
                        {
                            imagePath = "/View/Owner/noimage.png";
                        }
                    delayRequsets.Add(new DelayReservationDTO
                        {
                            Id = delayRequest.Id,
                            ReservationId = delayRequest.Reservation.Id,
                            UserName = delayRequest.Reservation.User.Username,
                            AccommodationName = delayRequest.Reservation.Accommodation.Name,
                            OldStartDate = delayRequest.Reservation.StartDate,
                            OldEndDate = delayRequest.Reservation.EndDate,
                            NewStartDate = delayRequest.NewStartDate,
                            NewEndDate = delayRequest.NewEndDate,
                            Busy = reservedDates.Contains(delayRequest.Id) ? true : false,
                            FrontImagePath = imagePath
                        });
                        delayRequestsNumber++;
                    }
            }
        }

        public int DelayRequestsNumber()
        {
            return delayRequestsNumber;
        }
        public void Update()
        {
            allRequests();
        }
    }
}
