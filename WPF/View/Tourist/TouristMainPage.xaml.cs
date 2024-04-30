using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.View;
using BookingApp.WPF.View.Tourist.Pages;
using BookingApp.WPF.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Tourist
{
    public partial class TouristMainPage : Window
    {
        public BookingApp.Model.Tourist Tourist { get; set; }
        public FinishedToursViewModel FinishedToursViewModel { get; set; }
        public TourReservationService _tourReservationService { get; set; }
        public UserRepository _userRepository { get; set; }
        public TouristService _touristService { get; set; }
        public TourInstanceService _tourInstanceService { get; set; }
        public VoucherService _voucherService { get; set; }
        public FastDriveRepository _fastDriveRepository { get; set; }
        public ReservedDriveRepository _reservedDriveRepository { get; set; }
        public TouristMainPage(BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            Tourist = tourist;
            _tourReservationService = new TourReservationService();
            _touristService = new TouristService();
            _tourInstanceService = new TourInstanceService();
            _voucherService = new VoucherService();
            _fastDriveRepository = new FastDriveRepository();
            _userRepository = new UserRepository();
            _reservedDriveRepository = new ReservedDriveRepository();
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
            }*/
            //_voucherService.UpdateValidVouchers();
        }

        public void CheckForNotification()
        {
            var fastDrive = _fastDriveRepository.GetAll().FirstOrDefault();

            if (fastDrive != null)
            {
                int driverId = _fastDriveRepository.IsFastDriveAccepted(fastDrive);

                if (driverId != -1)
                {
                    User driver = _userRepository.GetByID(driverId);
                    MessageBox.Show($"Driver {driver.Username} has accepted your reservation!", "Notification", MessageBoxButton.OK);
                    _reservedDriveRepository.Save(fastDrive);
                    _fastDriveRepository.Delete(fastDrive);
                }

                if (DateTime.Now - fastDrive.TimeOfReservation > TimeSpan.FromMinutes(5))
                {
                    _fastDriveRepository.Delete(fastDrive);
                }
            }
            else
            {
                return;
            }
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

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            CheckForNotification();
        }

        private void AvailableTours_Click(object sender, RoutedEventArgs e)
        {
            AvailableTours availableToursPage = new AvailableTours(Tourist);
            FrameHomePage.Navigate(availableToursPage);
        }

        private void DriveReservation_Click(object sender, RoutedEventArgs e)
        {
            DriveReservation driveReservation = new DriveReservation(Tourist.User);
            FrameHomePage.Navigate(driveReservation);
        }
        private void FastDrive_Click(object sender, RoutedEventArgs e)
        {
            FastDriveViewModel fastDriveViewModel = new FastDriveViewModel(Tourist.User);
            FrameHomePage.Navigate(new BookingApp.WPF.View.Tourist.Pages.FastDrivePage(fastDriveViewModel));
        }
        /*private void FinishedTours_Click(object sender, RoutedEventArgs e)
        {
            FinishedToursViewModel finishedToursViewModel = new FinishedToursViewModel(Tourist);
            FrameHomePage.Navigate(new FinishedTours(finishedToursViewModel));
        }*/
        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            VouchersViewModel vouchersViewModel = new VouchersViewModel(Tourist);
            FrameHomePage.Navigate(new Vouchers(vouchersViewModel));
        }
        /*private void Tours_Click(object sender, RoutedEventArgs e)
        {
            FrameHomePage.Navigate(new MyTours(Tourist.User));
        }*/
        private void BackToSignIn(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;
            if (toggleButton != null && toggleButton.IsChecked == true)
            {
                if (toggleButton.Template.FindName("Popup", toggleButton) is Popup popup)
                {
                    popup.IsOpen = true;
                }
            }
        }

    }

}