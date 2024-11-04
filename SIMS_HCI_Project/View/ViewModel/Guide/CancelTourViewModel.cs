using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using BookingApp.Repository;
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

        private TourInstanceService tourInstanceService;
        private TourReservationService tourReservationService;
        private TourGuestService tourGuestService;
        private TouristService touristService;
        private VoucherService voucherService;
        private TourService tourService;

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

        private List<Tours> tours = new List<Tours>();
        public struct Tours : INotifyPropertyChanged
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set
                {
                    if (_name != value)
                    {
                        _name = value;
                        OnPropertyChanged(nameof(Name));
                    }
                }
            }

            private int _tourInstanceId;
            public int TourInstanceId
            {
                get { return _tourInstanceId; }
                set
                {
                    if (_tourInstanceId != value)
                    {
                        _tourInstanceId = value;
                        OnPropertyChanged(nameof(TourInstanceId));
                    }
                }
            }

            private int _tourId;
            public int TourId
            {
                get { return _tourId; }
                set
                {
                    if (_tourId != value)
                    {
                        _tourInstanceId = value;
                        OnPropertyChanged(nameof(TourId));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public Tours(string name, int tourInstanceId, int tourId)
            {
                _name = name;
                _tourInstanceId = tourInstanceId;
                _tourId = tourId;
                PropertyChanged = null;
            }
        }

        private ObservableCollection<Tours> _toursBind;
        public ObservableCollection<Tours> ToursBind
        {
            get { return _toursBind; }
            set
            {
                _toursBind = value;
                OnPropertyChanged(nameof(ToursBind));
            }
        }

        private void GetNamesForTourInstances(List<TourInstance> tourInstances)
        {
            foreach (TourInstance tourInstance in tourInstances)
            {
                tourInstanceService = new TourInstanceService();
                Tour tour = tourService.GetById(tourInstance.TourId);
                Tours tourBind = new Tours(tour.Name + " " + tourInstance.StartTime.ToString(), tourInstance.Id, tour.Id);
                tours.Add(tourBind);

            }
        }

        TourInstanceService tourInstanceRepository;

        public CancelTourViewModel(User user)
        {
            tourInstanceService = new TourInstanceService();
            tourGuestService = new TourGuestService();
            tourReservationService = new TourReservationService();
            voucherService = new VoucherService();
            touristService = new TouristService();
            tourService = new TourService();
            //tourInstanceRepository = new TourInstanceRepository();
            tourInstanceRepository = new TourInstanceService();
            GetNamesForTourInstances(tourInstanceRepository.GetInactiveToursByUser(user.Id));
            ToursBind = new ObservableCollection<Tours>(tours);
            OnPropertyChanged(nameof(ToursBind));
        }

        public void CancelTour(Tours tour)
        {
            TourInstance instance = tourInstanceRepository.GetById(tour.TourInstanceId);
            if (instance.StartTime - DateTime.Now < TimeSpan.FromHours(48))
            {
                instance.State = TourInstanceState.Cancelled;
                tourInstanceService.Update(instance);
                tours.Remove(tour);
                ToursBind = new ObservableCollection<Tours>(tours);
                OnPropertyChanged(nameof(ToursBind));
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
