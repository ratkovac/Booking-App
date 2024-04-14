using BookingApp.DTO;
using BookingApp.View.Driver.Pages;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.View.Driver
{
    public partial class DriveReservationWindow : Window
    {
        DriveDTO selectedDrive;

        private DrivesWindow drivesWindow;

        public DriveReservationWindow(DriveDTO drive, DrivesWindow DrivesWindow)
        {
            InitializeComponent();
            selectedDrive = drive;
            drivesWindow = DrivesWindow;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            DriverAtAddress driverAtAddressPage = new DriverAtAddress(selectedDrive, drivesWindow);

            MainFrame.Navigate(driverAtAddressPage);
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            MinutesLatePage minutesLatePage = new MinutesLatePage(selectedDrive, drivesWindow);

            MainFrame.Navigate(minutesLatePage);
        }
    }
}