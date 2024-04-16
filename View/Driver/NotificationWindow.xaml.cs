using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.View.Tourist.Pages;
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
                    // MessageDisplay.Text = notifications[selectedIndex].Caption;
                    // NotificationText.Text = notifications[selectedIndex].Text;
                    viewModel.SelectionChanged(selectedIndex);
                    btnCancel.Visibility = Visibility.Visible;
                    btnAccept.Visibility = Visibility.Visible;
                }
            }
        }

        /*private void NotificationList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            viewModel.NotificationSelectionChangedCommand.Execute(null);
        }
         private ObservableCollection<DriveNotification> notifications = new ObservableCollection<DriveNotification>();

         private DispatcherTimer _timer;

         private User LoggedDriver;

         private DateTime StartTime;

         private FastDriveRepository _fastDriveRepository;

         private VehicleRepository _vehicleRepository;

         private DriveRepository _driveRepository;

         ObservableCollection<int> locations = new ObservableCollection<int>();
         ObservableCollection<FastDrive> fastDrives = new ObservableCollection<FastDrive>();

         public NotificationWindow(User driver)
         {
             InitializeComponent();

             LoggedDriver = driver;
             _fastDriveRepository = new FastDriveRepository();
             _vehicleRepository = new VehicleRepository();
             _driveRepository = new DriveRepository();

             locations = _vehicleRepository.GetLocationsByDriver(LoggedDriver);
             fastDrives = _fastDriveRepository.GetDrivesByLocations(locations);

             RefreshNotifications();
             NotificationList.ItemsSource = notifications;
             NotificationList.DisplayMemberPath = "Caption";

             StartTimer();
             StartTime = DateTime.Now;

             CheckSelectedNotification();
         }

         private void StartTimer()
         {
             _timer = new DispatcherTimer();
             _timer.Interval = TimeSpan.FromSeconds(60);
             _timer.Tick += Timer_Tick;
             _timer.Start();
         }

         private void Timer_Tick(object? sender, EventArgs e)
         {
             RefreshNotifications();
         }

         private void FilterByTime(ObservableCollection<DriveNotification> notifications)
         {
             foreach (DriveNotification notification in notifications)
             {
                 TimeSpan duration = DateTime.Now - notification.fastDrive.TimeOfReservation;
                 if (duration.TotalMinutes > 5)
                 {
                     _fastDriveRepository.Delete(notification.fastDrive);
                     fastDrives.Remove(notification.fastDrive);
                 }
             }
         }

         private void RefreshNotifications()
         {
             notifications.Clear();
             foreach (var fastDrive in fastDrives)
             {
                 DriveNotification newNotification = new DriveNotification($"Brza voznja{fastDrive.Id}", "Da li prihvatate brzu voznju?");
                 newNotification.fastDrive = fastDrive;
                 notifications.Add(newNotification);
             }
             FilterByTime(notifications);
         }

         private void NotificationList_SelectionChanged(object sender, RoutedEventArgs e)
         {
             if (NotificationList.SelectedItem != null)
             {
                 if (NotificationList.SelectedIndex != -1)
                 {
                     int selectedIndex = NotificationList.SelectedIndex;
                     MessageDisplay.Text = notifications[selectedIndex].Caption;
                     NotificationText.Text = notifications[selectedIndex].Text;
                     btnCancel.Visibility = Visibility.Visible;
                     btnAccept.Visibility = Visibility.Visible;
                 }
             }
         }

         public ObservableCollection<FastDrive> FilterFastDrives(ObservableCollection<FastDrive> fastDrives)
         {
             DateTime currentTime = DateTime.Now;

             for (int i = fastDrives.Count - 1; i >= 0; i--)
             {
                 FastDrive fastDrive = fastDrives[i];
                 TimeSpan timeDifference = currentTime - fastDrive.TimeOfReservation;

                 if (timeDifference.TotalMinutes > 35)
                 {
                     _fastDriveRepository.Delete(fastDrive);
                     fastDrives.RemoveAt(i);
                     RefreshNotifications();
                 }
             }
             return fastDrives;
         }

         private void btnCancel_Click(object sender, RoutedEventArgs e)
         {
             if (NotificationList.SelectedItem != null)
             {
                 int selectedIndex = NotificationList.SelectedIndex;
                 if (selectedIndex != -1)
                 {
                     fastDrives.RemoveAt(selectedIndex);
                 }
             }
         }

         private void btnAccept_Click(object sender, RoutedEventArgs e)
         {
             if (NotificationList.SelectedItem != null)
             {
                 if (NotificationList.SelectedIndex != -1)
                 {
                     int i = NotificationList.SelectedIndex;
                     FastDrive fastDrive = fastDrives[i];
                     Drive drive = new Drive(fastDrive, LoggedDriver);
                     _driveRepository.Save(drive);
                     _fastDriveRepository.Delete(fastDrive);
                     fastDrives.Remove(fastDrive);
                     RefreshNotifications();
                     MessageDisplay.Text = "";
                     NotificationText.Text = "";
                     NotificationList.SelectedItem = null;
                 }
             }
         }
         private void CheckSelectedNotification()
         {
             if (NotificationList.SelectedItem == null || !notifications.Contains(NotificationList.SelectedItem as DriveNotification))
             {
                 MessageDisplay.Text = "";
                 NotificationText.Text = "";
                 NotificationList.SelectedItem = null;
             }
         }*/
    }
}
