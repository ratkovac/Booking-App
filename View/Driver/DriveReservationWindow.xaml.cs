using BookingApp.DTO;
using BookingApp.View.Driver;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.View.Driver
{
    public partial class DriveReservationWindow : Window, INotifyPropertyChanged
    {
        public DriveDTO selectedDrive;
        public bool IsSuperDriver;


        private DrivesWindow drivesWindow;

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
        public DriveReservationWindow(DriveDTO drive, DrivesWindow DrivesWindow, bool isSuperDriver)
        {
            drivesWindow = DrivesWindow;

            IsSuperDriver = isSuperDriver;
            InitializeDriverColor();

            DataContext = this;
            InitializeComponent();
            CenterWindowOnScreen();

            selectedDrive = drive;

            Closed += DriveReservationWindow_Closed;
        }
        private void InitializeDriverColor()
        {
            ColorOne = "LightGray";
            ColorTwo = "PaleTurquoise";

        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            drivesWindow._viewModel.IsOverlayVisible = true;
            var driverAtAddressWindow = new DriverAtAddressWindow(selectedDrive, drivesWindow, IsSuperDriver);
            driverAtAddressWindow.Show();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            drivesWindow._viewModel.IsOverlayVisible = true;

            var minutesLateWindow = new MinutesLateWindow(selectedDrive, drivesWindow, IsSuperDriver);
            minutesLateWindow.Show();
        }
        private void DriveReservationWindow_Closed(object sender, System.EventArgs e)
        {
            drivesWindow._viewModel.RefreshDriveList();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Button_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                if (sender == btnNo)
                {
                    btnYes.Focus();
                }
                else if (sender == btnYes)
                {
                    btnNo.Focus();
                }
            }
            else if (e.Key == Key.Enter)
            {
                if (sender == btnNo)
                {
                    btnNo_Click(sender, e);
                }
                else if (sender == btnYes)
                {
                    btnYes_Click(sender, e);
                }
            }
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