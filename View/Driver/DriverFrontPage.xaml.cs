using BookingApp.Model;
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
using System.Windows.Shapes;

namespace BookingApp.View.Driver
{

    public partial class DriverFrontPage : Window
    {
        public User LoggedInUser { get; set; }

        public DriverFrontPage()
        {
            InitializeComponent();
        }
        public DriverFrontPage(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
        }

        private void btnOpenVehicleRegistration_Click(object sender, RoutedEventArgs e)
        {
            Driver.VehicleRegistrationWindow vehicleRegistrationWindow = new Driver.VehicleRegistrationWindow(LoggedInUser);
            vehicleRegistrationWindow.Show();
            Close();
        }


        private void btnShowDrives_Click(object sender, RoutedEventArgs e)
        {
            Driver.DrivesWindow driveWindow = new Driver.DrivesWindow(LoggedInUser);
            driveWindow.Show();
            Close();
        }

        private void btnOpenStats_Click(object sender, RoutedEventArgs e)
        {
            Driver.Example example = new Driver.Example();
            example.Show();
            Close();
        }

        private void btnNotification_Click(object sender, RoutedEventArgs e)
        {
            Driver.NotificationPage notificationPage = new NotificationPage();
            notificationPage.Show();
            Close();
        }
    }
}
