using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.WPF.View.Tourist.Pages;
using GalaSoft.MvvmLight.Command;
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
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class DriveDisplayViewModel : INotifyPropertyChanged
    {
        private DriveRepository driveRepository;
        public DriverUnreliableReportService unreliableReportService;
        public List<Drive> ListDrive { get; set; }
        public Drive SelectedDrive { get; set; }
        public ICommand TouristDelayCommand { get; set; }
        public ICommand UnreliableDriverCommand { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }

        public DriveDisplayViewModel(BookingApp.Model.Tourist tourist)
        {
            Tourist = tourist;
            driveRepository = new DriveRepository();
            ListDrive = new List<Drive>(driveRepository.GetAll());
            unreliableReportService = new DriverUnreliableReportService();
            TouristDelayCommand = new RelayCommand<object>(ExecuteTouristDelayCommand);
            UnreliableDriverCommand = new RelayCommand<object>(ExecuteUnreliableDriverCommand);
        }

        private void ExecuteTouristDelayCommand(object sender)
        {
            var button = sender as Button;
            if (button != null && button.DataContext is Drive selectedDrive)
            {
                TouristDelay delayWindow = new TouristDelay();
                delayWindow.ShowDialog();

                double delayMinutes = delayWindow.DelayMinutes;

                selectedDrive.TouristDelay = delayMinutes;
                driveRepository.Update(selectedDrive);
            }
        }

        private void ExecuteUnreliableDriverCommand(object sender)
        {
            var button = sender as Button;
            if (button != null && button.DataContext is Drive selectedDrive)
            {
                SelectedDrive = selectedDrive;
                if (!IsDriverLate())
                {
                    MessageBox.Show("You can report only if the driver is 10 minutes late.");
                    return;
                }
                else if (IsReported())
                {
                    MessageBox.Show("This driver was already reported.");
                    return;
                }
                else
                {
                    ReportUnreliableDriver();
                    MessageBox.Show("Your report was successful!");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateDriveList()
        {
            ListDrive.Clear();
            foreach (var drive in driveRepository.GetAll())
            {
                ListDrive.Add(drive);
            }
        }

        public void Update()
        {
            UpdateDriveList();
        }

        public bool IsReported()
        {
            /*if (SelectedDrive == null)
            {
                MessageBox.Show("You have to select a drive!");
                return false;
            }*/
            var driverId = SelectedDrive.DriverId;
            return unreliableReportService.ReportAlreadyExists(driverId, SelectedDrive.Id, Tourist.Id);
        }

        public bool IsDriverLate()
        {
            /*if (SelectedDrive == null)
            {
                MessageBox.Show("You have to select a drive!");
                return false;
            }*/
            DateTime currentTime = DateTime.Now;
            DateTime scheduledTime = SelectedDrive.Date;
            DateTime scheduledPlusTenMinutes = scheduledTime.AddMinutes(10);

            if (currentTime > scheduledPlusTenMinutes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReportUnreliableDriver()
        {
            /*if (SelectedDrive == null)
            {
                MessageBox.Show("You have to select a drive!");
                return;
            }*/
            var driverId = SelectedDrive.DriverId;
            DateTime time = DateTime.Now;
            DriverUnreliableReport report = new DriverUnreliableReport(Tourist.Id, driverId, SelectedDrive.Id, time);
            unreliableReportService.Create(report);
        }
    }
}
