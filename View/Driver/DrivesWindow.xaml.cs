﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Serializer;
using BookingApp.View.Driver.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace BookingApp.View.Driver
{
    public partial class DrivesWindow : Window, INotifyPropertyChanged
    {

        DriveRepository _driveRepository;
        UserRepository _userRepository;
        public UnsuccessfulDrivesRepository _unsuccessfulDriveRepository;
        public DriverStatsUpdateRepository _driverStatsUpdateRepository;
        public DriverStatsRepository _driverStatsRepository;
        public ObservableCollection<DriveDTO> ListDrive { get; set; } = new ObservableCollection<DriveDTO>();
        public User LoggedInUser { get; }
        public bool IsOverlayVisible
        {
            get { return (bool)GetValue(IsOverlayVisibleProperty); }
            set { SetValue(IsOverlayVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsOverlayVisibleProperty =
            DependencyProperty.Register("IsOverlayVisible", typeof(bool), typeof(DrivesWindow), new PropertyMetadata(false));
        private string _colorOne;
        public string ColorOne
        {
            get { return _colorOne; }
            set
            {
                if (_colorOne != value)
                {
                    _colorOne = value;
                    OnPropertyChanged(nameof(ColorOne));
                }
            }
        }

        private string _colorTwo;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string ColorTwo
        {
            get { return _colorTwo; }
            set
            {
                if (_colorTwo != value)
                {
                    _colorTwo = value;
                    OnPropertyChanged(nameof(ColorTwo));
                }
            }
        }
        private bool IsSuperDriver;

        public DrivesWindow(User user, bool isFastDriver)
        {
            LoggedInUser = user;
            InitializeComponent();
            IsOverlayVisible = false;
            DataContext = this;
            _userRepository = new UserRepository();
            _driveRepository = new DriveRepository();
            _unsuccessfulDriveRepository = new UnsuccessfulDrivesRepository();
            _driverStatsRepository = new DriverStatsRepository();
            _driverStatsUpdateRepository = new DriverStatsUpdateRepository();
            IsSuperDriver = isFastDriver;
            CheckIfFastDriver(isFastDriver);
            Window_Loaded(this, null);
        }

        private void CheckIfFastDriver(bool isFastDriver)
        {
            if (isFastDriver == true)
            {
                ColorOne = "White";
                ColorTwo = "LightBlue";
            }
            else
            {
                ColorOne = "PaleTurquoise";
                ColorTwo = "White";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListDrive.Clear();

            var drives = _driveRepository.GetDrivesByDriver(LoggedInUser);

            foreach (var drive in drives)
            {
                DriveDTO driveDTO = new DriveDTO(drive);
                ListDrive.Add(driveDTO);
            }

        }

        private void btnDriveReservation_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DriveDTO selectedDrive = dataGrid.SelectedItem as DriveDTO;
                DriveReservationWindow reservationWindow = new DriveReservationWindow(selectedDrive, this, IsSuperDriver);

                reservationWindow.Owner = this;
                IsOverlayVisible = true;
                reservationWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a drive first.");
            }
        }

        internal void RefreshDriveList()
        {
            ListDrive.Clear();

            var drives = _driveRepository.GetDrivesByDriver(LoggedInUser);

            foreach (var drive in drives)
            {
                DriveDTO driveDTO = new DriveDTO(drive);
                ListDrive.Add(driveDTO);
            }
            IsOverlayVisible= false;            
        }

        private void btnCanelDrive_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DriveDTO selectedDrive = dataGrid.SelectedItem as DriveDTO;

                _unsuccessfulDriveRepository.Save(selectedDrive.ToDrive());
                _driveRepository.Delete(selectedDrive.ToDrive());
                DriverStatsUpdate update = new DriverStatsUpdate(LoggedInUser.Id);
                update.CancelledDrivesUpdate = 1;
                _driverStatsUpdateRepository.Create(update);
                DriverStats stats = _driverStatsRepository.GetById(LoggedInUser.Id);
                stats.CancelledDrives += 1;
                _driverStatsRepository.Update(stats);
                RefreshDriveList();
            }
            else
            {
                MessageBox.Show("Please select a drive first.");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Driver.Example example = new Driver.Example(LoggedInUser);
            example.Show();
            Close();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        internal void MakeVisible()
        {
            IsOverlayVisible = false;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}