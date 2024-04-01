﻿using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BookingApp.View.Driver.Pages
{

    public partial class DriverWaitingPage : Page
    {
        private DriveDTO selectedDrive;
        private DispatcherTimer timer;
        private int remainingTimeInSeconds = 20 * 60;
        public DriveRepository _driveRepository;
        public DrivesWindow drivesWindow;
        public SuccessfulDrivesRepository _successfulDrivesRepository;
        public UnsuccessfulDrivesRepository _unsuccessfulDrivesRepository;


        public DriverWaitingPage(DriveDTO drive, DrivesWindow DrivesWindow)
        {
            InitializeComponent();
            selectedDrive = drive;
            _driveRepository = new DriveRepository();
            _successfulDrivesRepository = new SuccessfulDrivesRepository();
            _unsuccessfulDrivesRepository = new UnsuccessfulDrivesRepository();
            StartTimer();
            drivesWindow = DrivesWindow;
        }

        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingTimeInSeconds--;

            if (remainingTimeInSeconds <= 0)
            {
                timer.Stop();
                MessageBox.Show("Time's up!");
                _driveRepository.Delete(selectedDrive.ToDrive());
                _unsuccessfulDrivesRepository.Save(selectedDrive.ToDrive());


                OpenDrivesPage();
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTimeInSeconds);
            string timeLeft = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            txtRemainingTime.Text = timeLeft;
        }

        private void btnTouristArrived_Click(object sender, RoutedEventArgs e)
        {
            _driveRepository.Delete(selectedDrive.ToDrive());
            _successfulDrivesRepository.Save(selectedDrive.ToDrive());

            StartingPricePage startingPricePage = new StartingPricePage(selectedDrive, drivesWindow);

            NavigationService.Navigate(startingPricePage);
            //OpenDrivesPage();
        }

        private void OpenDrivesPage()
        {
            Window.GetWindow(this)?.Close();
            drivesWindow.RefreshDriveList();
        }
    }
}
