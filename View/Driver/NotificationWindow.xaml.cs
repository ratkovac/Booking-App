using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.Tourist.Pages;
using BookingApp.ViewModel.Driver;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace BookingApp.View.Driver
{
    public partial class NotificationWindow : Window
    {
        public User LoggedDriver { get; set; }
        DriverNotificationViewModel viewModel { get; set; }
        public NotificationWindow(User driver)
        {
            InitializeComponent();
            LoggedDriver = driver;
            viewModel = new DriverNotificationViewModel(LoggedDriver);
            DataContext = viewModel;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (NotificationList.SelectedItem is DriveNotification selectedNotification)
            {
                viewModel.CancelAction(selectedNotification);
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (NotificationList.SelectedItem is DriveNotification selectedNotification)
            {
                viewModel.AcceptAction(selectedNotification);
            }
        }
        private void SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (NotificationList.SelectedItem != null)
            {
                if (NotificationList.SelectedIndex != -1)
                {
                    int selectedIndex = NotificationList.SelectedIndex;
                    viewModel.SelectionChanged(selectedIndex);
                    btnCancel.Visibility = Visibility.Visible;
                    btnAccept.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Driver.Example example = new Driver.Example(LoggedDriver);
            example.Show();
            Close();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To navigate, use TAB butoon.\nTo go to the lower menu, use CTRL+TAB");

        }
    }
}
