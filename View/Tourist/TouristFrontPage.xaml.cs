using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.Tourist.Pages;
using BookingApp.View.ViewModel.Tourist;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.View.Tourist
{
    public partial class TouristFrontPage : Window
    {
        public BookingApp.Model.Tourist Tourist { get; set; }
        public FinishedToursViewModel FinishedToursViewModel { get; set; }
        public TourReservationService _tourReservationService { get; set; }
        public TouristService _touristService { get; set; }
        public TourInstanceService _tourInstanceService { get; set; }
        public VoucherService _voucherService { get; set; }
        public TouristFrontPage(BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            Tourist = tourist;
            _tourReservationService = new TourReservationService();
            _touristService = new TouristService();
            _tourInstanceService = new TourInstanceService();
            _voucherService = new VoucherService();
            CheckTouristReservation();
            /*if (_tourReservationService.GetTourInstanceIdWhereTouristIsWaiting(Tourist) != null)
            {
                int tourInstanceId = _tourReservationService.GetTourInstanceIdWhereTouristIsWaiting(Tourist).TourInstanceId;
                TourInstance tourInstance = _tourInstanceService.GetById(tourInstanceId);
                MessageBoxResult answer = MessageBox.Show("Da li ste prisutni na turi " + tourInstance.Tour.Name + "?", "", MessageBoxButton.YesNo);
                if (answer == MessageBoxResult.Yes)
                {
                    _tourReservationService.UpdateTouristState(Tourist.Id, tourInstance, TouristState.Present);
                    CheckTouristReservation();
                }
            }
            _voucherService.UpdateValidVouchers();*/
        }

        public void CheckTouristReservation()
        {
            int number = 0;
            foreach (BookingApp.Model.TourReservation reservation in _tourReservationService.GetReservationsForGuest(Tourist.Id))
            {
                if (reservation.State == TouristState.Present && reservation.TourInstance.StartTime > DateTime.Now.AddYears(-1))
                {
                    number++;
                }
            }
            if (number % 5 == 0)
            {
                _touristService.GiveVoucherForGuestWhenFiveTimePresent(Tourist.Id);
                MessageBox.Show("Cestitamo! Uspjesno ste osvojili vaucer!");
            }
        }

        private void AvailableTours_Click(object sender, RoutedEventArgs e)
        {
            AvailableTours availableToursPage = new AvailableTours(Tourist);
            MainFrame.Navigate(availableToursPage);
        }

        private void DriveReservation_Click(object sender, RoutedEventArgs e)
        {
            DriveReservation driveReservation = new DriveReservation(Tourist.User);
            MainFrame.Navigate(driveReservation);
        }
        private void FastDrive_Click(object sender, RoutedEventArgs e)
        {
            FastDriveViewModel fastDriveViewModel = new FastDriveViewModel(Tourist.User);
            MainFrame.Navigate(new BookingApp.View.Tourist.Pages.FastDrive(fastDriveViewModel));
        }
        private void FinishedTours_Click(object sender, RoutedEventArgs e)
        {
            FinishedToursViewModel finishedToursViewModel = new FinishedToursViewModel(Tourist);
            MainFrame.Navigate(new FinishedTours(finishedToursViewModel));
        }
        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            VouchersViewModel vouchersViewModel = new VouchersViewModel(Tourist);
            MainFrame.Navigate(new Vouchers(vouchersViewModel));
        }
    }
}
