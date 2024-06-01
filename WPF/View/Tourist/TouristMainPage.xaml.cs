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
        public DriveRepository _driveRepository { get; set; }
        public FastDriveRepository _fastDriveRepository { get; set; }
        public GroupDriveRepository _groupDriveRepository { get; set; }
        public ReservedDriveRepository _reservedDriveRepository { get; set; }
        public LocationRepository _locationRepository { get; set; }
        public LocationService _locationService { get; set; }
        public LanguageRepository _languageRepository { get; set; }
        public LanguageService _languageService { get; set; }
        public TourRequestService _tourRequestService { get; set; }
        public TourRequestSegmentService _tourRequestSegmentService { get; set; }
        public TourRequestGuestService _tourRequestGuestService { get; set; }
        public ReservedGroupDriveRepository _reservedGroupDriveRepository { get; set; }
        public TouristMainPage(BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            Tourist = tourist;
            _tourReservationService = new TourReservationService();
            _touristService = new TouristService();
            _tourInstanceService = new TourInstanceService();
            _tourRequestService = new TourRequestService();
            _tourRequestSegmentService = new TourRequestSegmentService();
            _tourRequestGuestService = new TourRequestGuestService();
            _voucherService = new VoucherService();
            _driveRepository = new DriveRepository();
            _locationRepository = new LocationRepository();
            _locationService = new LocationService();
            _languageRepository = new LanguageRepository();
            _languageService = new LanguageService();
            _fastDriveRepository = new FastDriveRepository();
            _groupDriveRepository = new GroupDriveRepository();
            _userRepository = new UserRepository();
            _reservedDriveRepository = new ReservedDriveRepository();
            _reservedGroupDriveRepository = new ReservedGroupDriveRepository();
            CheckTouristReservation();
            TourDisplay tourDisplay = new TourDisplay(tourist);
            FrameHomePage.Navigate(tourDisplay);
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

        public void CheckForFastDriveNotification()
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
                    Drive drive = new(fastDrive.StartAddressId, fastDrive.EndAddressId, fastDrive.Date, fastDrive.Driver, fastDrive.Guest, fastDrive.DriveStatusId, fastDrive.Delay, 0);
                    _driveRepository.Save(drive);
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

        public void CheckForTourRequestNotification()
        {
            var tourRequests = _tourRequestService.GetAllTourRequests();

            var lastAcceptedRequest = tourRequests.LastOrDefault(request => request.IsAccepted == TourRequestStatus.ACCEPTED);

            if (lastAcceptedRequest != null)
            {
                MessageBox.Show($"A guide has accepted your tour request with ID={lastAcceptedRequest.Id}!");
            }
        }

        public void CheckForGroupDriveNotification()
        {
            var groupDrives = _groupDriveRepository.GetAll().ToList();

            foreach (var groupDrive in groupDrives)
            {
                int driverId = _groupDriveRepository.IsGroupDriveAccepted(groupDrive);

                if (DateTime.Now - groupDrive.TimeOfReservation > TimeSpan.FromMinutes(5))
                {
                    _groupDriveRepository.Delete(groupDrive);
                }
                else if (driverId != -1)
                {
                    User driver = _userRepository.GetByID(driverId);
                    User tourist = _userRepository.GetByID(groupDrive.GuestId);
                    MessageBox.Show($"Driver {driver.Username} has accepted your reservation!", "Notification", MessageBoxButton.OK);
                    Drive drive = new(groupDrive.StartAddressId, groupDrive.EndAddressId, groupDrive.Date, driver, tourist, groupDrive.DriveStatusId, groupDrive.Delay, 0);
                    _driveRepository.Save(drive);
                    _groupDriveRepository.Delete(groupDrive);
                    _reservedGroupDriveRepository.Save(groupDrive);
                }

                /*if (driverId != -1)
                {
                    User driver = _userRepository.GetByID(driverId);
                    MessageBox.Show($"Driver {driver.Username} has accepted your reservation!", "Notification", MessageBoxButton.OK);
                    _reservedGroupDriveRepository.Save(groupDrive);
                    _groupDriveRepository.Delete(groupDrive);
                }

                if (DateTime.Now - groupDrive.TimeOfReservation > TimeSpan.FromMinutes(5))
                {
                    _groupDriveRepository.Delete(groupDrive);
                }*/
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

        private void Notification_Click(object sender, MouseButtonEventArgs e)
        {
            CheckForGroupDriveNotification();
            CheckForFastDriveNotification();
            CheckForTourRequestNotification();
        }

        private void TourDisplay_Click(object sender, RoutedEventArgs e)
        {
            TourDisplay tourDisplay = new TourDisplay(Tourist);
            FrameHomePage.Navigate(tourDisplay);
        }

        private void TourRequestDisplay_Click(object sender, RoutedEventArgs e)
        {
            TourRequestDisplayViewModel tourRequestDisplayViewModel = new TourRequestDisplayViewModel(Tourist.User);
            FrameHomePage.Navigate(new BookingApp.WPF.View.Tourist.Pages.TourRequestDisplay(tourRequestDisplayViewModel));
        }

        private void ComplexRequest_Click(object sender, RoutedEventArgs e)
        {
            ComplexTourRequestViewModel complexTourRequestViewModel = new ComplexTourRequestViewModel(Tourist.User, _locationService, _languageService, _tourRequestService, _tourRequestSegmentService, _tourRequestGuestService);
            FrameHomePage.Navigate(new ComplexTourRequest(Tourist.User, _locationService, _languageService, _tourRequestService, _tourRequestSegmentService, _tourRequestGuestService));
        }

        private void TourRequest_Click(object sender, RoutedEventArgs e)
        {
            TourRequestViewModel tourRequestViewModel = new TourRequestViewModel(Tourist.User, _locationService, _languageService, _tourRequestService, _tourRequestSegmentService, _tourRequestGuestService);
            FrameHomePage.Navigate(new Pages.TourRequest(Tourist.User, _locationService, _languageService, _tourRequestService, _tourRequestSegmentService, _tourRequestGuestService));
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            TourRequestStatisticsViewModel tourRequestStatisticsViewModel = new TourRequestStatisticsViewModel(Tourist.Id, _tourRequestService, _tourRequestSegmentService);
            FrameHomePage.Navigate(new TourRequestStatsView(Tourist, _tourRequestService, _tourRequestSegmentService));
        }

        private void DriveDisplay_Click(object sender, RoutedEventArgs e)
        {
            DriveDisplayViewModel driveDisplayViewModel = new DriveDisplayViewModel(Tourist);
            FrameHomePage.Navigate(new BookingApp.WPF.View.Tourist.Pages.DriveDisplay(driveDisplayViewModel));
        }

        private void DriveReservation_Click(object sender, RoutedEventArgs e)
        {
            DriveReservationView driveReservation = new DriveReservationView(Tourist.User);
            FrameHomePage.Navigate(driveReservation);
        }
        private void FastDrive_Click(object sender, RoutedEventArgs e)
        {
            FastDriveViewModel fastDriveViewModel = new FastDriveViewModel(Tourist.User);
            FrameHomePage.Navigate(new BookingApp.WPF.View.Tourist.Pages.FastDrivePage(fastDriveViewModel));
        }
        private void GroupDrive_Click(object sender, RoutedEventArgs e)
        {
            GroupDriveViewModel groupDriveViewModel = new GroupDriveViewModel(Tourist.User);
            FrameHomePage.Navigate(new BookingApp.WPF.View.Tourist.Pages.GroupDrivePage(groupDriveViewModel));
        }
        private void FinishedTours_Click(object sender, RoutedEventArgs e)
        {
            FinishedToursViewModel finishedToursViewModel = new FinishedToursViewModel(Tourist);
            FrameHomePage.Navigate(new FinishedTours(finishedToursViewModel));
        }
        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            VouchersViewModel vouchersViewModel = new VouchersViewModel(Tourist);
            FrameHomePage.Navigate(new VouchersView(vouchersViewModel));
        }
        private void Tours_Click(object sender, RoutedEventArgs e)
        {
            ReservationsDisplayViewModel reservationsDisplayViewModel = new ReservationsDisplayViewModel(Tourist);
            FrameHomePage.Navigate(new BookingApp.WPF.View.Tourist.Pages.ReservationsDisplay(reservationsDisplayViewModel));
        }
        private void Profile_Click(object sender, MouseButtonEventArgs e)
        {
            FrameHomePage.Navigate(new MyProfile(Tourist));
        }
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