using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.View.Driver
{
    public partial class StartingPriceWindow : Window, INotifyPropertyChanged
    {
        private DriveDTO selectedDrive;
        private DrivesWindow drivesWindow;
        private int StartingPrice;
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
        public StartingPriceWindow(DriveDTO drive, DrivesWindow DrivesWindow, bool isSuperDriver)
        {
            DataContext = this;
            IsSuperDriver = isSuperDriver;
            CheckIfFastDriver(IsSuperDriver);

            selectedDrive = drive;
            drivesWindow = DrivesWindow;
            InitializeComponent();
            CenterWindowOnScreen();

            Closed += DriveReservationWindow_Closed;
        }
        private void DriveReservationWindow_Closed(object sender, System.EventArgs e)
        {
            drivesWindow.RefreshDriveList();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtStartingPrice.Text, out int startingPrice) && txtStartingPrice.Text != "")
            {
                StartingPrice = startingPrice;
                DriveInProgressWindow driveInProgressWindow = new DriveInProgressWindow(selectedDrive, StartingPrice, drivesWindow, IsSuperDriver);
                this.Close();
                drivesWindow.IsOverlayVisible = true;
                driveInProgressWindow.Show();                
            }
            else
            {
                MessageBox.Show("Please enter a valid starting price.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
