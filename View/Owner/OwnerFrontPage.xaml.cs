﻿using BookingApp.Model;
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
            this.DataContext = ownerFrontPageViewModel;
            LoggedInUser = user;
            ownerFrontPageViewModel = new OwnerFrontPageViewModel(LoggedInUser);
            Username.Content = LoggedInUser.Username;
            Role.Content = LoggedInUser.Role;
            Username_Copy.Content = LoggedInUser.Username;
            Role_Copy.Content = LoggedInUser.Role;
            if (ownerFrontPageViewModel.SuperOwner(LoggedInUser))
            {
                SuperOwnerImage.Source = new BitmapImage(new Uri("/View/Owner/Images/star.png", UriKind.Relative));
                SuperOwnerBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7B421"));
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
            if (guestGrade.UnratedGuestsNumber() == 0)
            {
                AccommodationsGradesViewModel accommodationsGradesViewModel = new AccommodationsGradesViewModel(LoggedInUser);
                AccommodationsGrades accommodationsGrades = new AccommodationsGrades(accommodationsGradesViewModel);
                accommodationsGrades.Show();
            }
            else
            {
                MessageBox.Show("Still, you have unrated guests!\nFirst, you need to rate all your guests to see how they've rated you!", "Rate guests!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
