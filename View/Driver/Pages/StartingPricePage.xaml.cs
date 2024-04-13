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
    /// Interaction logic for StartingPrice.xaml
    /// </summary>
    public partial class StartingPricePage : Page
    {
        private DriveDTO selectedDrive;
        private DrivesWindow drivesWindow;
        private int StartingPrice; 

        public StartingPricePage()
        {
            InitializeComponent();
        }

        public StartingPricePage(DriveDTO selectedDrive, DrivesWindow drivesWindow)
        {
            InitializeComponent();
            this.selectedDrive = selectedDrive;
            this.drivesWindow = drivesWindow;
        }

        private void SetStartingPrice(string input)
        {
            if (int.TryParse(input, out int price))
            {
                StartingPrice = price;
            }
            else
            {
                MessageBox.Show("Unesite celobrojnu vrednost.");
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string input = txtStartingPrice.Text;
            SetStartingPrice(input);

            DriveStartedPage driveStartedPage = new DriveStartedPage(selectedDrive, StartingPrice, drivesWindow);

            NavigationService.Navigate(driveStartedPage);
        }
    }
}
