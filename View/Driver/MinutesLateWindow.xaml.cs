using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.Driver.Pages;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace BookingApp.View.Driver
{
    public partial class MinutesLateWindow : Window, INotifyPropertyChanged
    {
        private DriveDTO selectedDrive;
        private DrivesWindow drivesWindow;
        private bool IsSuperDriver;
        private DriveRepository _driveRepository;

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
        public MinutesLateWindow(DriveDTO drive, DrivesWindow DrivesWindow, bool isSuperDriver)
        {
            DataContext = this;
            IsSuperDriver = isSuperDriver;
            selectedDrive = drive;
            drivesWindow = DrivesWindow;
            _driveRepository = new DriveRepository();
            CheckIfFastDriver(IsSuperDriver);
            InitializeComponent();
            CenterWindowOnScreen();

            Closed += DriveReservationWindow_Closed;
        }
        private void DriveReservationWindow_Closed(object sender, System.EventArgs e)
        {
            drivesWindow.RefreshDriveList();
        }

        private void btnConfirmation_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MinutesLateTextBox.Text, out int minutesLate) && MinutesLateTextBox.Text != "")
            {
                this.Close();
                drivesWindow.IsOverlayVisible = true;

                Drive drive = _driveRepository.GetById(selectedDrive.Id);
                drive.Delay = minutesLate;
                _driveRepository.Update(drive);

                var driverAtAddressWindow = new DriverAtAddressWindow(selectedDrive, drivesWindow, IsSuperDriver);
                driverAtAddressWindow.Show();
            }
            else
            {
                MessageBox.Show("Please enter an integer value for the delay.");
            }
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
        private void MinutesLateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
    }
}
