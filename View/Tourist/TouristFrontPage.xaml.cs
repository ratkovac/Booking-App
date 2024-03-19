using BookingApp.Model;
using BookingApp.View.Tourist.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.View.Tourist
{
    public partial class TouristFrontPage : Window
    {
        public User User { get; set; }
        public TouristFrontPage(User user)
        {
            InitializeComponent();
            DataContext = this;
            User = user;
        }

        private void AvailableTours_Click(object sender, RoutedEventArgs e)
        {
            AvailableTours availableToursPage = new AvailableTours(User);
            MainFrame.Navigate(availableToursPage);
        }
    }
}
