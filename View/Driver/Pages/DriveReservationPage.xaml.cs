using BookingApp.DTO;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Driver.Pages
{

    public partial class DriveReservationPage : Page
    {
        DriveDTO selectedDrive;

        public DriveReservationPage(DriveDTO drive)
        {
            InitializeComponent();
            selectedDrive = drive;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
        //    NavigationService.Navigate(new DriverAtAddress(selectedDrive, DrivesWindow DriveWindow));
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
          //  MinutesLatePage minutesLatePage = new MinutesLatePage(selectedDrive, drivesWindow);

            //MainFrame.Navigate(minutesLatePage);
        }
    }
}
