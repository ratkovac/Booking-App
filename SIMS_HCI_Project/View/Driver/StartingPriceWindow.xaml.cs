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
            drivesWindow = DrivesWindow;

            DataContext = this;
            IsSuperDriver = isSuperDriver;
            InitializeDriverColor();

            selectedDrive = drive;
            InitializeComponent();
            CenterWindowOnScreen();

            Closed += DriveReservationWindow_Closed;
            Loaded += (sender, e) =>
            {
                txtStartingPrice.Focus();
            };

            txtStartingPrice.PreviewTextInput += (sender, e) =>
            {
                if (!char.IsDigit(e.Text, e.Text.Length - 1))
                {
                    e.Handled = true;
                }
            };
        }
        private void DriveReservationWindow_Closed(object sender, System.EventArgs e)
        {
            drivesWindow._viewModel.RefreshDriveList();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtStartingPrice.Text, out int startingPrice) && txtStartingPrice.Text != "")
            {
                StartingPrice = startingPrice;
                DriveInProgressWindow driveInProgressWindow = new DriveInProgressWindow(selectedDrive, StartingPrice, drivesWindow, IsSuperDriver);
                this.Close();
                drivesWindow._viewModel.IsOverlayVisible = true;
                driveInProgressWindow.Show();                
            }
            else
            {
                MessageBox.Show("Please enter a valid starting price.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        private void MinutesLateTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
    }
}
