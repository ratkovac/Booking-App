using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;

namespace BookingApp.View.Driver
{
    public partial class DriveInProgressWindow : Window, INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private DriveDriven driveDriven;
        private DateTime startTime;
        private DrivesDrivenRepository _drivesDrivenRepository;
        private DrivesWindow drivesWindow;
        private bool IsSuperDriver;
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

        public event PropertyChangedEventHandler PropertyChanged;

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


        public DriveInProgressWindow(DriveDTO driveDTO, int startingPrice, DrivesWindow DrivesWindow, bool isSuperDriver)
        {
            DataContext = this;
            IsSuperDriver = isSuperDriver;
            drivesWindow = DrivesWindow;
            Drive drive = driveDTO.ToDrive();
            driveDriven = new DriveDriven();
            _drivesDrivenRepository = new DrivesDrivenRepository();
            driveDriven.DriveId = drive.Id;
            driveDriven.Price = startingPrice;
            driveDriven.DriveId = drive.Id;
            StartTimer();
            startTime = DateTime.Now;
            CheckIfFastDriver(IsSuperDriver);
            InitializeComponent();
            txtPrice.Text = startingPrice.ToString();
            CenterWindowOnScreen();

            Closed += DriveReservationWindow_Closed;
        }
        private void DriveReservationWindow_Closed(object sender, System.EventArgs e)
        {
            drivesWindow.RefreshDriveList();
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

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            driveDriven.Price += 5;

            TimeSpan duration = DateTime.Now - startTime;
            driveDriven.Duration = duration;


            txtPrice.Text = driveDriven.Price.ToString();
        }

        private void btnEndDrive_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _drivesDrivenRepository.Save(driveDriven);
            OpenDrivesPage();
        }
        private void OpenDrivesPage()
        {
            this.Close();
            drivesWindow.RefreshDriveList();
            drivesWindow.MakeVisible();
        }
    }
}
