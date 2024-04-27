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
    public ObservableCollection<DriveNotification> Notifications
    {
        get { return _notifications; }
        set
        {
            _notifications = value;
            OnPropertyChanged(nameof(Notifications));
        }
    }

    public DriverNotificationViewModel(User driver)
    {
        _fastDriveService = fastDriveService;
        LoggedDriver = driver;
        LoadNotifications();
        StartTimer();
        StartTime = DateTime.Now;
    }
    private void StartTimer()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(15);
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
                if (newNotification.fastDrive.DriverId == 0)
                {
                    Notifications.Add(newNotification);
                }
            }
        }
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
                if (newNotification.fastDrive.DriverId == 0)
                {
                    Notifications.Add(newNotification);
                }
            }
        }
    }

    private bool FilterOneByTime(DriveNotification notification)
    {
        TimeSpan duration = DateTime.Now - notification.fastDrive.TimeOfReservation;
        return _fastDriveService.CheckDuration(duration.TotalMinutes);
    }

    public void AcceptAction(object parameter)
    {
        if (parameter is DriveNotification selectedNotification)
        {
            Drive drive = new Drive(selectedNotification.fastDrive, LoggedDriver);
            _fastDriveService.SaveDrive(drive);
            FastDrive newFastDrive = selectedNotification.fastDrive;
            newFastDrive.DriverId = LoggedDriver.Id;
            _fastDriveService.Update(newFastDrive);
            Notifications.Remove(selectedNotification);
            MessageDisplay = "";
            NotificationText = "";
        }
    }

    public void CancelAction(object parameter)
    {
        if (parameter is DriveNotification selectedNotification)
        {
            FastDrive fastDrive = selectedNotification.fastDrive;
            fastDrive.DriverId = -1;
            _fastDriveService.Update(selectedNotification.fastDrive);
            Notifications.Remove(selectedNotification);
            MessageDisplay = "";
            NotificationText = "";
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
