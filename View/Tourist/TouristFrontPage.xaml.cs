using BookingApp.Model;
using System.Windows;

namespace BookingApp.View.Tourist
{
    public partial class TouristFrontPage : Window
    {
        public User LoggedInUser { get; set; }
        public TouristFrontPage(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
        }
    }
}
