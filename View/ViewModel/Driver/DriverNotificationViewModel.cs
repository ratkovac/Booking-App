using BookingApp.Model;
using BookingApp.Service;
using BookingApp.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

public class DriverNotificationViewModel : BaseViewModel
{
    private readonly FastDriveService _fastDriveService;
    private DispatcherTimer _timer;
    private DateTime StartTime;

    private ObservableCollection<DriveNotification> _notifications;
    private ObservableCollection<DriveNotification> CancelledNotifications;


    public User LoggedDriver {  get; set; }
    ObservableCollection<int> locations = new ObservableCollection<int>();
    ObservableCollection<FastDrive> fastDrives = new ObservableCollection<FastDrive>();

    private DriveNotification _selectedNotification;
    public DriveNotification SelectedNotification
    {
        get { return _selectedNotification; }
        set
        {
            _selectedNotification = value;
            OnPropertyChanged(nameof(SelectedNotification));
        }
    }
    private Visibility _ButtonVisibility;
    public Visibility ButtonVisibility
    {
        get { return _ButtonVisibility; }
        set
        {
            _ButtonVisibility = value;
            OnPropertyChanged(nameof(ButtonVisibility)); 
        }
    }

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
    public string NumberOfNotifications
    {
        get { return _notifications?.Count.ToString(); }
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
            OnPropertyChanged(nameof(NumberOfNotifications));
        }
    }

    public DriverNotificationViewModel(User driver)
    {
        ButtonVisibility = Visibility.Hidden;
        _fastDriveService = new FastDriveService();
        CancelledNotifications = new ObservableCollection<DriveNotification>();  
        LoggedDriver = driver;
        Notifications = new ObservableCollection<DriveNotification>();
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
        int IntNumberOfNotifications = 0;
        foreach (var fastDrive in fastDrives)
        {
            Location location = _fastDriveService.GetLocationById(fastDrive.StartAddress.LocationId);
            DriveNotification newNotification = new DriveNotification($"Brza voznja u {location.City}", $"Da li prihvatate brzu voznju u {fastDrive.StartAddress.Street} {fastDrive.StartAddress.Number}, {location.City}, {location.Country} ?");
            newNotification.fastDrive = fastDrive;
            if (FilterOneByTime(newNotification))
            {
                if (newNotification.fastDrive.DriverId == 0)
                {
                    Notifications.Add(newNotification);
                    IntNumberOfNotifications++;
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
            Location location = _fastDriveService.GetLocationById(fastDrive.StartAddress.LocationId);
            DriveNotification newNotification = new DriveNotification($"Brza voznja u  {location.City}", $"Da li prihvatate brzu voznju u {fastDrive.StartAddress.Street} {fastDrive.StartAddress.Number}, {location.City}, {location.Country} ?");
            newNotification.fastDrive = fastDrive;
            if (FilterOneByTime(newNotification))
            {
                if (newNotification.fastDrive.DriverId == 0)
                {
                    if(!CancelledNotifications.Contains(newNotification))
                    Notifications.Add(newNotification);
                }
            }
            else
            {
                if (newNotification.fastDrive.DriverId == 0)
                {
                    FastDrive fastDriveEnd = newNotification.fastDrive;
                    fastDriveEnd.DriverId = _fastDriveService.GetAvailableDriver();
                    _fastDriveService.Update(fastDriveEnd);
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
            _fastDriveService.AddBonusPoints(LoggedDriver.Id);
        }
    }

    public void CancelAction(object parameter)
    {
        if (parameter is DriveNotification selectedNotification)
        {
            FastDrive fastDrive = selectedNotification.fastDrive;
            //fastDrive.DriverId = 0;
            //_fastDriveService.Update(selectedNotification.fastDrive);
            CancelledNotifications.Add(selectedNotification);
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
            if (selectedNotification != null) {
                ButtonVisibility = Visibility.Visible;
            }
        }
    }
}
