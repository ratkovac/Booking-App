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
    /// Interaction logic for MinutesLatePage.xaml
    /// </summary>
    public partial class MinutesLatePage : Page
    {
        private DriveDTO selectedDrive;
        public MinutesLatePage(DriveDTO drive)
        {
            InitializeComponent();
            selectedDrive = drive;
        }

        private void btnConfirmation_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MinutesLateTextBox.Text, out int minutesLate))
            {
                NavigationService.Navigate(new DriverAtAddress(selectedDrive));
            }
            else
            {
                MessageBox.Show("Molimo unesite celobrojnu vrednost za kašnjenje.");
            }
        }
    }
}
