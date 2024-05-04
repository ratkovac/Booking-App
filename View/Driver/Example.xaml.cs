using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using BookingApp.Repository;
using System.Collections.ObjectModel;
using BookingApp.ViewModel.Driver;
using BookingApp.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace BookingApp.View.Driver
{
    /// <summary>
    /// Interaction logic for Example.xaml
    /// </summary>
    public partial class Example : Window
    {
        public User LoggedDriver { get; set; }
        StatisticsViewModel viewModel { get; set; }

        public Example(User driver)
        {
            InitializeComponent();
            LoggedDriver = driver;
            viewModel = new StatisticsViewModel(LoggedDriver);
            DataContext = viewModel;

        }

        private void btnOpenVehicleRegistration_Click(object sender, RoutedEventArgs e)
        {
            Driver.VehicleRegistrationWindow vehicleRegistrationWindow = new Driver.VehicleRegistrationWindow(LoggedDriver);
            vehicleRegistrationWindow.Show();
            Close();
        }


        private void btnShowDrives_Click(object sender, RoutedEventArgs e)
        {
            Driver.DrivesWindow driveWindow = new Driver.DrivesWindow(LoggedDriver);
            driveWindow.Show();
            Close();
        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {
            Driver.NotificationWindow notificationWindow = new NotificationWindow(LoggedDriver);
            notificationWindow.Show();
            Close();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
       