using BookingApp.DTO;
using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View.Driver
{
    /// <summary>
    /// Interaction logic for DriverAtAddressWindow.xaml
    /// </summary>
    public partial class DriverAtAddressWindow : Window, INotifyPropertyChanged
    {
        private bool enterPressedAfterRegisterDrive = false;

        DriveDTO selectedDrive;
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
        public DriverAtAddressWindow(DriveDTO drive, DrivesWindow DrivesWindow, bool isSuperDriver)
        {
            selectedDrive = drive;
            drivesWindow = DrivesWindow;
            IsSuperDriver = isSuperDriver;
            DataContext = this;
            InitializeDriverColor();
            InitializeComponent();
            CenterWindowOnScreen();

            Closed += DriveReservationWindow_Closed;
            Loaded += (sender, e) =>
            {
                txtBlock.Focus();
            };

            PreviewKeyDown += Window_PreviewKeyDown;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && enterPressedAfterRegisterDrive)
            {
                btnVehicleAtAddress_Click(null, null);
            }
        }
        private void btnRegisterDrive_Click(object sender, RoutedEventArgs e)
        {

            enterPressedAfterRegisterDrive = true;
        }
        private void DriveReservationWindow_Closed(object sender, System.EventArgs e)
        {
            drivesWindow._viewModel.RefreshDriveList();
        }
        private void InitializeDriverColor()
        {
            ColorOne = "LightGray";
            ColorTwo = "PaleTurquoise";

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

        private void btnVehicleAtAddress_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            drivesWindow._viewModel.IsOverlayVisible = true;
            var driverWaitingWindow = new DriverWaitingWindow(selectedDrive, drivesWindow, IsSuperDriver);
            driverWaitingWindow.Show();
        }
    }
}
