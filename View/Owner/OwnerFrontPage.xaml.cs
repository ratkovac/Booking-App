using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BookingApp.View.ViewModel.Owner;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace BookingApp.View.Owner
{
    public partial class OwnerFrontPage : Window
    {
        public User LoggedInUser;
        OwnerFrontPageViewModel ownerFrontPageViewModel;
        public OwnerFrontPage(User user)
        {
            InitializeComponent();
            LoggedInUser = user;
            ownerFrontPageViewModel = new OwnerFrontPageViewModel(LoggedInUser);
            this.DataContext = ownerFrontPageViewModel;
            Username.Content = LoggedInUser.Username;
            Role.Content = LoggedInUser.Role;
            Username_Copy.Content = LoggedInUser.Username;
            Role_Copy.Content = LoggedInUser.Role;
            if (ownerFrontPageViewModel.SuperOwner(LoggedInUser))
            {
                SuperOwnerImage.Source = new BitmapImage(new Uri("/View/Owner/star.png", UriKind.Relative));
                SuperOwnerBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7B421"));
            }
            delayRequestWarning();
        }
        private void delayRequestWarning()
        {
            DelayRequestsViewModel delayRequestsViewModel = new DelayRequestsViewModel(LoggedInUser);
            if (delayRequestsViewModel.DelayRequestsNumber() > 0)
            {
                DelayRequests.Foreground = Brushes.Red;
            }
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

        private void AccommodationGrades_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GuestGrade guestGrade = new GuestGrade(LoggedInUser);
            AccommodationsGradesViewModel accommodationsGradesViewModel = new AccommodationsGradesViewModel(LoggedInUser);
            AccommodationsGrades accommodationsGrades = new AccommodationsGrades(accommodationsGradesViewModel);
            accommodationsGrades.Show();
            
        }

        private void DelayRequests_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DelayRequestsViewModel delayRequestsViewModel = new DelayRequestsViewModel(LoggedInUser);
            DelayRequests delayRequests = new DelayRequests(delayRequestsViewModel);
            delayRequests.Show();
        }

        private void Renovations_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AllRenovationsViewModel allRenovationsViewModel = new AllRenovationsViewModel(LoggedInUser);
            AllRenovations allRenovations = new AllRenovations(allRenovationsViewModel);
            allRenovations.Show();
        }
    }
}
