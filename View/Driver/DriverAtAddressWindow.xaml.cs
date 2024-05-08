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
            CheckIfFastDriver(IsSuperDriver);
            InitializeComponent();
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

        private void btnVehicleAtAddress_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
