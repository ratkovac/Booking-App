using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Serializer;
using BookingApp.View.Driver.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class DrivesWindow : Window
    {

        DriveRepository _driveRepository;
        UserRepository _userRepository;
        public ObservableCollection<DriveDTO> ListDrive { get; set; } = new ObservableCollection<DriveDTO>();
        public User LoggedInUser { get; }
        public bool IsOverlayVisible
        {
            get { return (bool)GetValue(IsOverlayVisibleProperty); }
            set { SetValue(IsOverlayVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsOverlayVisibleProperty =
            DependencyProperty.Register("IsOverlayVisible", typeof(bool), typeof(DrivesWindow), new PropertyMetadata(false));

        public DrivesWindow(User user)
        {
            LoggedInUser = user;
            InitializeComponent(); 
            DataContext = this;
            _userRepository = new UserRepository();
            _driveRepository = new DriveRepository();
            Window_Loaded(this, null);
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
                DriveReservationWindow reservationWindow = new DriveReservationWindow(selectedDrive, this);

                reservationWindow.Owner = this;
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
        }
    }
}