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
            CenterWindowOnScreen();
            LoggedDriver = driver;
            viewModel = new StatisticsViewModel(LoggedDriver);
            DataContext = viewModel;
            this.PreviewKeyDown += DrivesWindow_PreviewKeyDown;
        }
        private void DrivesWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.H && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                btnHelp_Click(null, null);
            }
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                btnOpenVehicleRegistration_Click(null, null);
            }
            if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                btnShowDrives_Click(null, null);
            }
            if (e.Key == Key.N && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                btnNotifications_Click(null, null);
            }
            if (e.Key == Key.W&& (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                btnHoliday_Click(null, null);
            }
            if (e.Key == Key.L && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                LogOutButton_Click(null, null);
            }
            if (e.Key == Key.R && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                DriverReport_Click(null, null);
            }
        }
        private void btnOpenVehicleRegistration_Click(object sender, RoutedEventArgs e)
        {
            Driver.VehicleRegistrationWindow vehicleRegistrationWindow = new Driver.VehicleRegistrationWindow(LoggedDriver);
            vehicleRegistrationWindow.Show();
            Close();
        }


        private void btnShowDrives_Click(object sender, RoutedEventArgs e)
        {
            Driver.DrivesWindow driveWindow = new Driver.DrivesWindow(LoggedDriver, viewModel.IsFastDriver());
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
            HelpWindow helpWindow = new HelpWindow(this);
            helpWindow.Show();
            //Close();
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth - windowWidth) / 2;
            Top = (screenHeight - windowHeight) / 2;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signIn = new SignInForm();
            signIn.Show();
            Close();
        }

        private void btnHoliday_Click(object sender, RoutedEventArgs e)
        {
            HolidayWindow holidayWindow = new HolidayWindow();
            //Close();
            holidayWindow.Show();
        }

        private void DriverReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Report is temporary unavailable. ");
        }

    }
}
       