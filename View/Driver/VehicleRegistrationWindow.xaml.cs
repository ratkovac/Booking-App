using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Serializer;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace BookingApp.View.Driver
{
    /// <summary>
    /// Interaction logic for VehicleRegistrationWindow.xaml
    /// </summary>
    public partial class VehicleRegistrationWindow : Window
    {
        private readonly VehicleRepository _vehicleRepository;

        private readonly Serializer<Vehicle> _serializer;

        private readonly User LoggedInUser;

        public VehicleRegistrationWindow(User user)
        {
            LoggedInUser = user;
            InitializeComponent();
            _vehicleRepository = new VehicleRepository();
        }
        

        private void RegisterVehicle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string location = LocationTextBox.Text;
                int maxCapacity = int.Parse(MaxCapacityTextBox.Text);
                string languages = LanguagesTextBox.Text;
                Vehicle newVehicle = new Vehicle
                {
                    Location = location,
                    Capacity = maxCapacity,
                    Language = languages
                };

                _vehicleRepository.Save(newVehicle);

                MessageBox.Show("Vehicle successfully registered!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while registering vehicle: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        List<string> ImageList = new List<string>();

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            bool? response = dlg.ShowDialog();

            if (response == true)
            {
                foreach (string s in dlg.FileNames)
                {
                    ImageList.Add(s);
                }
            }

        }

        private bool ValidationForm()
        {
            bool isValid = true;

            if (LocationTextBox.Text == "")
            {
                isValid = false;
                LocationLabelError.Content = "Niste uneli lokaciju";
                LocationLabelError.Foreground = Brushes.Red;
            }
            else
            {
                LocationLabelError.Content = "";

            }
            if (MaxCapacityTextBox.Text == "")
            {
                isValid = false;
                MaxCapacityLabelError.Content = "Niste uneli Maksimalni Kapacitet";
                LocationLabelError.Foreground = Brushes.Red;
            }
            else
            {
                MaxCapacityLabelError.Content = "";
            }
            if (LanguagesTextBox.Text == "")
            {
                isValid = false;
                LanguagesLabelError.Content = "Niste uneli jezike";
                LanguagesLabelError.Foreground = Brushes.Red;
            }
            else
            {
                LanguagesLabelError.Content = "";
            }



            return isValid;
        }

        private void btnRegistraterVehicle_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationForm())
            {
                Vehicle vehicle = new Vehicle();
                vehicle.Location = LocationTextBox.Text;
                vehicle.Capacity = int.Parse(MaxCapacityTextBox.Text);
                vehicle.Language = LanguagesTextBox.Text;
                vehicle.ImagePaths = ImageList;
                vehicle.User=LoggedInUser;
                //MessageBox.Show(LoggedInUser.ToString());
                UserRepository userRepository = new UserRepository();
                List<Vehicle> vehicleList = new List<Vehicle>();
                vehicleList.Add(vehicle);
                //MessageBox.Show(VehicleList.Count.ToString());
                //MessageBox.Show(vehicleList[0].ToString());
                //MessageBox.Show(vehicle.ToString());

                _vehicleRepository.Save(vehicle);
                //_serializer.ToCSV("vehicle.csv", vehicleList);

            }
        }
    }
}
