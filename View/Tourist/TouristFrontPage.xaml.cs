using BookingApp.Model;
using BookingApp.View.Tourist.Pages;
using BookingApp.View.ViewModel.Tourist;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.View.Tourist
{
    public partial class TouristFrontPage : Window
    {
        public User Tourist { get; set; }
        public FinishedToursViewModel FinishedToursViewModel { get; set; }
        public TouristFrontPage(User tourist)
        {
            InitializeComponent();
            DataContext = this;
            Tourist = tourist;
        }

        private void AvailableTours_Click(object sender, RoutedEventArgs e)
        {
            AvailableTours availableToursPage = new AvailableTours(Tourist);
            MainFrame.Navigate(availableToursPage);
        }

        private void DriveReservation_Click(object sender, RoutedEventArgs e)
        {
            DriveReservation driveReservation = new DriveReservation(Tourist);
            MainFrame.Navigate(driveReservation);
        }
        private void FastDrive_Click(object sender, RoutedEventArgs e)
        {
            FastDriveSearch fastDrive = new FastDriveSearch(Tourist);
            MainFrame.Navigate(fastDrive);
        }
        private void FinishedTours_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FinishedTours(Tourist));
        }
    }
}
