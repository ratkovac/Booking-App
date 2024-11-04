using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ReservationsDisplayViewModel : INotifyPropertyChanged
    {
        public TourReservationRepository tourReservationRepository;
        public BookingApp.Model.Tourist Tourist { get; set; }

        private ObservableCollection<TourReservation> _listReservation;
        public ObservableCollection<TourReservation> ListReservation
        {
            get => _listReservation;
            set
            {
                _listReservation = value;
                OnPropertyChanged();
            }
        }

        private TourReservation _selectedReservation;
        public TourReservation SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged();
            }
        }

        public ReservationsDisplayViewModel(BookingApp.Model.Tourist tourist)
        {
            Tourist = tourist;
            tourReservationRepository = new TourReservationRepository();
            ListReservation = new ObservableCollection<TourReservation>(tourReservationRepository.GetFinishedAndStartedReservations());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateReservationsList()
        {
            ListReservation.Clear();
            foreach (var reservation in tourReservationRepository.GetAll())
            {
                ListReservation.Add(reservation);
            }
        }

        public void Update()
        {
            UpdateReservationsList();
        }
    }
}
