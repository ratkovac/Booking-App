using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Xml;
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

namespace BookingApp.View.Driver.Pages
{

    public partial class MinutesLatePage : Page, INotifyPropertyChanged
    {
        private DriveDTO selectedDrive;
        private DrivesWindow drivesWindow;
        private bool IsSuperDriver;

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
        public MinutesLatePage(DriveDTO drive, DrivesWindow DrivesWindow, bool isSuperDriver)
        {
            DataContext = this;
            IsSuperDriver = isSuperDriver;
            InitializeComponent();
            selectedDrive = drive;
            drivesWindow= DrivesWindow;
        }

        private void btnConfirmation_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MinutesLateTextBox.Text, out int minutesLate))
            {
                NavigationService.Navigate(new DriverAtAddress(selectedDrive, drivesWindow));
            }
            else
            {
                MessageBox.Show("Molimo unesite celobrojnu vrednost za kašnjenje.");
            }
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
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
