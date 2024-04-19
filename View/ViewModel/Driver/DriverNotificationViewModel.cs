using BookingApp.Model;
using BookingApp.Service;
using BookingApp.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

public class DriverNotificationViewModel : BaseViewModel
{
    private readonly FastDriveService _fastDriveService;
    private DispatcherTimer _timer;
    private DateTime StartTime;

    private ObservableCollection<DriveNotification> _notifications;
    public ObservableCollection<DriveNotification> Notifications
    {
        get { return _notifications; }
        set
        {
            _notifications = value;
            OnPropertyChanged(nameof(Notifications));
        }
    }
    public ObservableCollection<int> cancelledIds;
    FastDriveService fastDriveService = new FastDriveService();
    public User LoggedDriver {  get; set; }
    ObservableCollection<int> locations = new ObservableCollection<int>();
    ObservableCollection<FastDrive> fastDrives = new ObservableCollection<FastDrive>();
    private string _messageDisplay;
    public string MessageDisplay
    {
        get { return _messageDisplay; }
        set
        {
            _messageDisplay = value;
            OnPropertyChanged(nameof(MessageDisplay));
        }
    }

    private string _notificationText;
    public string NotificationText
    {
        get { return _notificationText; }
        set
        {
            _notificationText = value;
            OnPropertyChanged(nameof(NotificationText));
        }
    }

    public DriverNotificationViewModel(User driver)
    {
        cancelledIds = new ObservableCollection<int>();
        _fastDriveService = fastDriveService;
        LoggedDriver = driver;
        LoadNotifications();
        StartTimer();
        StartTime = DateTime.Now;
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
    private void RefreshNotifications()
    {
        Notifications.Clear();
        var fastDrivesList = _fastDriveService.GetAllFastDrives();
        ObservableCollection<FastDrive> fastDrives = new ObservableCollection<FastDrive>(fastDrivesList);
        foreach (var fastDrive in fastDrives)
        {
            DriveNotification newNotification = new DriveNotification($"Brza voznja{fastDrive.Id}", "Da li prihvatate brzu voznju?");
            newNotification.fastDrive = fastDrive;
            if (FilterOneByTime(newNotification))
            {
                if (newNotification.fastDrive.DriverId == 0 && !cancelledIds.Contains(newNotification.fastDrive.Id))
                {
                    Notifications.Add(newNotification);
                }
            }
        }
    }
    private void FilterByTime(ObservableCollection<DriveNotification> notifications)
    {
        foreach (DriveNotification notification in notifications)
        {
            TimeSpan duration = DateTime.Now - notification.fastDrive.TimeOfReservation;
            if (duration.TotalMinutes > 0.2)
            {
                _fastDriveService.Delete(notification.fastDrive);
                fastDrives.Remove(notification.fastDrive);
            }
        }
    }
    private bool FilterOneByTime(DriveNotification notification)
    {
        TimeSpan duration = DateTime.Now - notification.fastDrive.TimeOfReservation;
        if (duration.TotalMinutes > 0.2)
            {
            return false;
        }
        return true;
    }

    private void LoadNotifications()
    {
        var fastDrivesList = _fastDriveService.GetAllFastDrives();
        ObservableCollection<FastDrive> fastDrives = new ObservableCollection<FastDrive>(fastDrivesList);

        Notifications = new ObservableCollection<DriveNotification>();

        foreach (var fastDrive in fastDrives)
        {
            DriveNotification newNotification = new DriveNotification($"Brza voznja{fastDrive.Id}", "Da li prihvatate brzu voznju?");
            newNotification.fastDrive = fastDrive;
            if (FilterOneByTime(newNotification))
            {
                if (newNotification.fastDrive.DriverId == 0 && !cancelledIds.Contains(newNotification.fastDrive.Id))
                {
                    Notifications.Add(newNotification);
                }
            }
        }
    }

    public void AcceptAction(object parameter)
    {
        if (parameter is DriveNotification selectedNotification)
        {
            Drive drive = new Drive(selectedNotification.fastDrive, LoggedDriver);
            _fastDriveService.SaveDrive(drive);
            FastDrive newFastDrive = selectedNotification.fastDrive;
            newFastDrive.DriverId = LoggedDriver.Id;
            _fastDriveService.Update(selectedNotification.fastDrive);
            Notifications.Remove(selectedNotification);
            MessageDisplay = "";
            NotificationText = "";
        }
    }

    public void CancelAction(object parameter)
    {
        if (parameter is DriveNotification selectedNotification)
        {
            cancelledIds.Add(selectedNotification.fastDrive.Id);
            Notifications.Remove(selectedNotification);
        }
    }
    public void SelectionChanged(int parameter)
    {
        DriveNotification driveNotification = Notifications[parameter];
        if (driveNotification is DriveNotification selectedNotification)
        {
            MessageDisplay = selectedNotification.Caption;
            NotificationText = selectedNotification.Text;
        }
    }
}
