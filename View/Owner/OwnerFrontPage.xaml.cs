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
        public User LoggedInUser { get; set; }

        public OwnerFrontPage(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
        }

        private void NewAccomodation_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Dodajte željenu logiku koja treba da se izvrši kada korisnik klikne na Labelu
            AccommodationAdd accommodationAdd = new AccommodationAdd(LoggedInUser);
            accommodationAdd.Show();
            Close();
            
        }

        private void GuestGrade_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GuestGrade guestGrade = new GuestGrade(LoggedInUser);
            guestGrade.Show();
            Close();
        }
    }
}
