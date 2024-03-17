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
    /// <summary>
    /// Interaction logic for DriverFrontPage.xaml
    /// </summary>
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

        private void btnVehicleAtAddress_Click(object sender, RoutedEventArgs e)
        {
            Driver.VehicleAtAddressWindow vehicleAtAddressWindow = new Driver.VehicleAtAddressWindow(LoggedInUser);
            vehicleAtAddressWindow.Show();
            Close();
        }

        private void btnDriveCreation_Click(object sender, RoutedEventArgs e)
        {
            Driver.DriveCreationWindow driveCreationWindow = new Driver.DriveCreationWindow(LoggedInUser);
            driveCreationWindow.Show();
            Close();
        }
    }
}
