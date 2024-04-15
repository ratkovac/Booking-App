using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.Service;

namespace BookingApp.View.ViewModel.Guide
{
    internal class CancelTourViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private User user;
        private TourInstanceService tourInstanceService = new TourInstanceService();
        private TourReservationService tourReservationService = new TourReservationService();
        private TourGuestService tourGuestService = new TourGuestService();
        private TouristService touristService = new TouristService();
        private VoucherService voucherService = new VoucherService();

        private ObservableCollection<TourInstance> _tourInstances;
        public ObservableCollection<TourInstance> TourInstances
        {
            get { return _tourInstances; }
            set
            {
                _tourInstances = value;
                OnPropertyChanged(nameof(TourInstances));
            }
        }

        public CancelTourViewModel(User user)
        {
            this.user = user;
            TourInstances = new ObservableCollection<TourInstance>(tourInstanceService.GetInactiveToursByUser(user.Id));
            
        }

        public void CancelTour(TourInstance instance)
        {
            if (instance.StartTime - DateTime.Now < TimeSpan.FromHours(48))
            {
                instance.State = TourInstanceState.Cancelled;
                tourInstanceService.Update(instance);
                TourInstances.Remove(instance);
                VoucherForTourists(instance.Id);
            }

        }

        private void VoucherForTourists(int tourInstanceId)
        {
            List<TourReservation> tourReservations = tourReservationService.GetAllByTourInstanceId(tourInstanceId);
            foreach (TourReservation tourReservation in tourReservations)
            {
                GiveVoucher(tourReservation.Id);
            }
        }

        private void GiveVoucher(int tourReservationId)
        {
            List<TourGuest> tourGuests = tourGuestService.GetAllByTourReservationId(tourReservationId);
            foreach (TourGuest tourGuest in tourGuests)
            {
                Voucher voucher = new Voucher(DateTime.Now, DateTime.Now.AddYears(1), false, true);
                voucherService.Create(voucher);

                BookingApp.Model.Tourist tourist = touristService.GetById(tourGuest.TouristId);
                tourist.VoucherIds.Add(voucher.Id);
                touristService.Update(tourist);

            }
        }
    }
}
