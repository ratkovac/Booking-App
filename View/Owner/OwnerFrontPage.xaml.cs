using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.View.Owner
{
    public partial class OwnerFrontPage : Window
    {
        public User LoggedInUser;

        public OwnerFrontPage(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
        }

        private void NewAccomodation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AccommodationAdd accommodationAdd = new AccommodationAdd(LoggedInUser);
            accommodationAdd.Show();
        }

        private void GuestGrade_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GuestGrade guestGrade = new GuestGrade(LoggedInUser);
            guestGrade.Show();
        }
    }
}
