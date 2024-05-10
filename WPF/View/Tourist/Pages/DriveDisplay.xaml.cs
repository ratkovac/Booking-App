using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View;
using BookingApp.View.NGuest;
using BookingApp.WPF.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class DriveDisplay : Page
    {
        public DriveRepository driveRepository { get; set; }
        private DriveDisplayViewModel ViewModel;
        public DriveDisplay(DriveDisplayViewModel driveDisplayViewModel)
        {
            InitializeComponent();
            this.DataContext = driveDisplayViewModel;
            driveRepository = new DriveRepository();
            ViewModel = driveDisplayViewModel;
        }

        private void TouristDelay_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Drive selectedDrive = (Drive)button.DataContext;

            TouristDelay delayWindow = new TouristDelay();
            delayWindow.ShowDialog();

            double delayMinutes = delayWindow.DelayMinutes;

            selectedDrive.TouristDelay = delayMinutes;
            driveRepository.Update(selectedDrive);
        }

        private void UnreliableDriver_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Drive selectedDrive = (Drive)button.DataContext;
            ViewModel.SelectedDrive = selectedDrive;

            if (!ViewModel.IsDriverLate())
            {
                MessageBox.Show("You can report only if the driver is 10 minutes late.");
                return;
            }
            else if (ViewModel.IsReported())
            {
                MessageBox.Show("This driver was already reported.");
                return;
            }
            else
            {
                ViewModel.ReportUnreliableDriver();
                MessageBox.Show("Your report was successful!");
            }
        }
    }
}
