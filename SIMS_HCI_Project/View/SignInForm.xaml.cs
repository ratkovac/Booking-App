﻿using BookingApp.Model;
using BookingApp.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using BookingApp.View.Owner;
using BookingApp.WPF.View.Tourist;
using BookingApp.View.GuideView;
using BookingApp.Service;
using BookingApp.View.GuideView.Pages;
using BookingApp.View.NGuest;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private int loggedUserId;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password)
                {
                    switch (user.Role)
                    {
                        case "Owner":
                            OwnerFrontPage ownerFrontPage = new OwnerFrontPage(user);
                            ownerFrontPage.Show();
                            Close();
                            break;
                        case "Guest":
                            Home home = new Home(user);
                            home.Show();
                            Close();
                            break;
                        case "Guide":
                            TourForm tourForm = new TourForm(user);
                            tourForm.Show();
                            //TourManagementWindow tourManagementWindow = new TourManagementWindow();
                            //tourManagementWindow.Show();
                            Close();
                            break;
                        case "Tourist":
                            TouristService touristService = new TouristService();
                            BookingApp.Model.Tourist tourist = touristService.GetTouristByUserId(user.Id);
                            TouristMainPage touristMainPage = new TouristMainPage(tourist);
                            touristMainPage.Show();
                            Close();
                            break;
                        case "Driver":
                            Driver.Example example = new Driver.Example(user);
                            example.Show();
                            Close();
                            break;
                        default:
                            break;
                    }
                    //CommentsOverview commentsOverview = new CommentsOverview(user);
                    //commentsOverview.Show();
                    //Close();
                }
                else if (user.Password != txtPassword.Password)
                {
                    MessageBox.Show("Wrong password!");
                }
                else
                {
                    MessageBox.Show("Wrong role!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }

        }

    }
}
