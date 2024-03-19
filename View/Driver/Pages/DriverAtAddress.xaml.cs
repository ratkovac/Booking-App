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
    /// <summary>
    /// Interaction logic for DriverAtAddress.xaml
    /// </summary>
    public partial class DriverAtAddress : Page
    {
        DriveDTO selectedDrive;
        public DriverAtAddress(DriveDTO drive)
        {
            InitializeComponent();
            selectedDrive = drive;
        }
        private void btnVehicleAtAddress_Click(object sender, RoutedEventArgs e)
        {
            DriverWaitingPage driverWaitingPage = new DriverWaitingPage(selectedDrive);

            NavigationService.Navigate(driverWaitingPage);
        }
    }
}
