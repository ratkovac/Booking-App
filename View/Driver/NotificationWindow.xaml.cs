using BookingApp.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookingApp.View.Driver
{
    public partial class NotificationWindow : Window
    {
        private ObservableCollection<string> notificationCaptions = new ObservableCollection<string>();

        private ObservableCollection<DriveNotification> notifications = new ObservableCollection<DriveNotification>();

        private User LoggedDriver;
  
        public NotificationWindow(User driver)
        {
            InitializeComponent();

            NotificationList.ItemsSource = notificationCaptions;
            LoggedDriver = driver;

            DriveNotification newNotification1 = new DriveNotification("Naslov1", "Tekst1");
            notifications.Add(newNotification1);
            notificationCaptions.Add(newNotification1.Caption);
            DriveNotification newNotification2 = new DriveNotification("Naslov2", "Tekst2");
            notifications.Add(newNotification2);
            notificationCaptions.Add(newNotification2.Caption);
            DriveNotification newNotification3 = new DriveNotification("Naslov3", "Tekst3");
            notifications.Add(newNotification3);
            notificationCaptions.Add(newNotification3.Caption);
            DriveNotification newNotification4 = new DriveNotification("Naslov4", "Tekst4");
            notifications.Add(newNotification4);
            notificationCaptions.Add(newNotification4.Caption);
        }

        private void AddNotification_Click(object sender, RoutedEventArgs e)
        {
            DriveNotification newNotification = new DriveNotification("Naslov", "Tekst");
            notifications.Add(newNotification); 
            notificationCaptions.Add(newNotification.Caption); 
        }

        private void RemoveNotification_Click(object sender, RoutedEventArgs e)
        {
            if (NotificationList.SelectedIndex != -1)
            {
                int selectedIndex = NotificationList.SelectedIndex;
                notifications.RemoveAt(selectedIndex);
                notificationCaptions.RemoveAt(selectedIndex);
            }
        }

        private void NotificationList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (NotificationList.SelectedItem != null)
            {
                if (NotificationList.SelectedIndex != -1)
                {
                    int selectedIndex = NotificationList.SelectedIndex;
                    MessageDisplay.Text = notifications[selectedIndex].Caption;
                }
            }
        }

        private void FastDrivesCheck()
        {
            //_fastDrivesRepository = new FastDrivesRepository();
            //_fastDrivesRepository.checkFastDrivesForDriversLocation();


        }
    }
}
