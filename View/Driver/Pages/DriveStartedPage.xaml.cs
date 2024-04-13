using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;

namespace BookingApp.View.Driver.Pages
{
    public partial class DriveStartedPage : Page
    {
        private DispatcherTimer _timer;
        private DriveDriven driveDriven;
        private DateTime startTime;
        private DrivesDrivenRepository _drivesDrivenRepository;
        private DrivesWindow drivesWindow;

        public DriveStartedPage(DriveDTO driveDTO, int startingPrice, DrivesWindow DrivesWindow)
        {
            drivesWindow = DrivesWindow;
            Drive drive = driveDTO.ToDrive();
            InitializeComponent();
            driveDriven = new DriveDriven();
            _drivesDrivenRepository = new DrivesDrivenRepository();
            driveDriven.DriveId = drive.Id;
            driveDriven.Price = startingPrice;
            driveDriven.DriveId = drive.Id;
            DataContext = driveDriven;
            StartTimer();
            startTime = DateTime.Now;
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Povećava početnu cenu za 1 svaku sekundu
            driveDriven.Price += 1;

            TimeSpan duration = DateTime.Now - startTime;
            driveDriven.Duration = duration;


            txtPrice.Text = driveDriven.Price.ToString();
        }

        private void btnEndDrive_Click(object sender, RoutedEventArgs e)
        {
            // Prekida tajmer kada se klikne dugme "End Drive"
            _timer.Stop();
            // Dodajte logiku za završetak vožnje ovde
            _drivesDrivenRepository.Save(driveDriven);
            OpenDrivesPage();
        }
        private void OpenDrivesPage()
        {
            Window.GetWindow(this)?.Close();
            drivesWindow.RefreshDriveList();
        }
    }
}
