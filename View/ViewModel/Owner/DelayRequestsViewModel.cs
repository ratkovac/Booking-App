using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.Owner;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.View.ViewModel.Owner
{
    public class DelayRequestsViewModel : IObserver
    {
        public ObservableCollection<DelayReservationDTO> delayRequsets { get; set; }
        private User user { get; set; }

        private DelayReservationService delayReservationService;

        private int delayRequestsNumber = 0;
        public DelayRequestsViewModel(User user)
        {
            this.user = user;
            delayReservationService = new DelayReservationService();
            delayReservationService.Subscribe(this);
            delayRequsets = new ObservableCollection<DelayReservationDTO>();
            Update();

        }

        public ObservableCollection<DelayReservationDTO> allRequests()
        {
            delayRequsets.Clear();
            foreach (var delayRequest in delayReservationService.GetAll())
            {
                if (user.Id == delayRequest.Reservation.Accommodation.User.Id)
                {
                    if(delayRequest.Status.Equals(DelayReservationStatusEnum.Pending))
                    {
                        delayRequsets.Add(new DelayReservationDTO
                        {
                            Id = delayRequest.Id,
                            ReservationId = delayRequest.Reservation.Id,
                            UserName = delayRequest.Reservation.User.Username,
                            AccommodationName = delayRequest.Reservation.Accommodation.Name,
                            OldStartDate = delayRequest.Reservation.StartDate,
                            OldEndDate = delayRequest.Reservation.EndDate,
                            NewStartDate = delayRequest.NewStartDate,
                            NewEndDate = delayRequest.NewEndDate
                        }) ;
                        delayRequestsNumber ++;
                    }
                }
            }
            return delayRequsets;
        }
        /*public void UpdateDelayReservation()
        {
            //DelayReservation delayReservation = 
        }*/
        public int DelayRequestsNumber()
        {
            return delayRequestsNumber;
        }
        public void Update()
        {
            delayRequsets = allRequests();
        }
    }
}
