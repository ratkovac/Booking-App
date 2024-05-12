using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace BookingApp.View.Driver
{
    public partial class DriverWaitingWindow : Window, INotifyPropertyChanged
    {
        private DriveDTO selectedDrive;
        private DispatcherTimer timer;
        private int remainingTimeInSeconds = 20 * 60;
        public DriveRepository _driveRepository;
        public DrivesWindow drivesWindow;
        public SuccessfulDrivesRepository _successfulDrivesRepository;
        public UnsuccessfulDrivesRepository _unsuccessfulDrivesRepository;
        public bool IsSuperDriver;
        private string _colorOne;
        public string ColorOne
        {
            get { return _colorOne; }
            set
            {
                if (_colorOne != value)
                {
                    _colorOne = value;
                    OnPropertyChanged(nameof(ColorOne));
                }
            }
        }

        private string _colorTwo;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string ColorTwo
        {
            get { return _colorTwo; }
            set
            {
                if (_colorTwo != value)
                {
                    _colorTwo = value;
                    OnPropertyChanged(nameof(ColorTwo));
                }
            }
        }
        public DriverWaitingWindow(DriveDTO drive, DrivesWindow DrivesWindow, bool isSuperDriver)
        {
            IsSuperDriver = isSuperDriver;
            CheckIfFastDriver(IsSuperDriver);

            DataContext = this;
            InitializeComponent();
            CenterWindowOnScreen(); selectedDrive = drive;
            _driveRepository = new DriveRepository();
            _successfulDrivesRepository = new SuccessfulDrivesRepository();
            _unsuccessfulDrivesRepository = new UnsuccessfulDrivesRepository();
            StartTimer();
            drivesWindow = DrivesWindow;

            Closed += DriveReservationWindow_Closed;
        }
        private void CheckIfFastDriver(bool isFastDriver)
        {
            if (isFastDriver == true)
            {
                ColorOne = "White";
                ColorTwo = "LightBlue";
            }
            else
            {
                ColorOne = "PaleTurquoise";
                ColorTwo = "White";
            }
        }
        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingTimeInSeconds--;

            if (remainingTimeInSeconds <= 0)
            {
                timer.Stop();
                MessageBox.Show("Time's up!");
                _driveRepository.Delete(selectedDrive.ToDrive());
                _unsuccessfulDrivesRepository.Save(selectedDrive.ToDrive());

                OpenDrivesPage();
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTimeInSeconds);
            string timeLeft = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            txtRemainingTime.Text = timeLeft;
        }

        private void btnTouristArrived_Click(object sender, RoutedEventArgs e)
        {
            _driveRepository.Delete(selectedDrive.ToDrive());
            _successfulDrivesRepository.Save(selectedDrive.ToDrive());
            this.Close();
            drivesWindow.IsOverlayVisible = true;

            StartingPriceWindow startingPriceWindow = new StartingPriceWindow(selectedDrive, drivesWindow, IsSuperDriver);
            startingPriceWindow.Show();
        }

        private void OpenDrivesPage()
        {
            drivesWindow.RefreshDriveList();
            Close();
        }
        private void DriveReservationWindow_Closed(object sender, System.EventArgs e)
        {
            drivesWindow.RefreshDriveList();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth - windowWidth) / 2;
            Top = (screenHeight - windowHeight) / 2;
        }
    }
}
