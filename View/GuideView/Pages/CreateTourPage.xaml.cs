using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using BookingApp.Model;
using BookingApp.Repository;
using Microsoft.VisualBasic;

namespace BookingApp.View.GuideView.Pages
{
    /// <summary>
    /// Interaction logic for CreateTourPage.xaml
    /// </summary>
    public partial class CreateTourPage : Page
    {
        private CheckPointRepository checkPointRepository = new CheckPointRepository();
        private DateRealizationRepository dateRealizationRepository = new DateRealizationRepository();
        private TourRepository tourRepository = new TourRepository();
        private LocationRepository locationRepository = new LocationRepository();

        private List<Tour> tours = new List<Tour>();
        
        private List<TextBox> ListCheckPoints = new List<TextBox>();
        private List<TextBox> ListDates = new List<TextBox>();

        public CreateTourPage()
        {
            InitializeComponent();
            ListCheckPoints.Add(txtStartCheckPoint);
            ListCheckPoints.Add(txtOptionCheckPoint);

            ListDates.Add(txtDates);

        }


        // Collects information from input fields, validates them, creates a new tour object.
        private void btnCreateTour_Click(object sender, RoutedEventArgs e)
        {

            string name = txtName.Text;
            string city = txtCity.Text;
            string country = txtCountry.Text;
            string description = txtDescription.Text;
            string lang = ((ComboBoxItem)txtLanguage.SelectedItem).Content.ToString();
            int maxGuests;
            float duration;

            // Validate input data
            if (!int.TryParse(txtMaxGuests.Text, out maxGuests))
            {
                MessageBox.Show("Invalid Max Number of Tourists");
                return;
            }

            if (!float.TryParse(txtDuration.Text, out duration))
            {
                MessageBox.Show("Invalid Duration");
                return;
            }

            // Create tour object
            Language language = new Language(lang);
            Location location = new Location(city, country);
            Tour tour = new Tour(name, description, maxGuests, duration, location, language);

            locationRepository.Save(location);
            tourRepository.Save(tour);

            GetCheckPoints(ListCheckPoints, tour.Id);
            GetDateAndTimes(ListDates, tour.Id);



            ClearFields();
        }

        // Extracts CheckPoints from TextBoxes and adds them to a List
        private List<CheckPoint> GetCheckPoints(List<TextBox> listCheckPoints, int tourId)
        {
            List<CheckPoint> checkPoints = new List<CheckPoint>();

            foreach (TextBox textBox in listCheckPoints)
            {
                CheckPoint checkPoint = new CheckPoint(textBox.Text, tourId);
                checkPoints.Add(checkPoint);
                checkPointRepository.Save(checkPoint);
            }
            CheckPoint endCheckPoint = new CheckPoint(txtEndCheckPoint.Text, tourId);
            checkPoints.Add(endCheckPoint);
            checkPointRepository.Save(endCheckPoint);

            return checkPoints;
        }

        // Extracts DatesAndTimes from TextBoxes and adds them to a List
        private List<DateRealization> GetDateAndTimes(List<TextBox> listDates, int tourId)
        {
            List<DateRealization> dates = new List<DateRealization>();

            foreach (TextBox textBox in listDates)
            {
                string dateString = textBox.Text;

                DateTime parsedDate;
                if (DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    DateRealization dateRealization = new DateRealization(parsedDate, tourId);
                    dates.Add(dateRealization);
                    dateRealizationRepository.Save(dateRealization);
                }
                else
                {
                    MessageBox.Show($"Nije moguće parsirati datum: {dateString}");
                }
            }
            return dates;
        }
        private void ClearFields()
        {
            txtName.Text = "Enter Name here..";
            txtName.Foreground = Brushes.Gray;

            txtCity.Text = "Enter City here..";
            txtCity.Foreground = Brushes.Gray;

            txtCountry.Text = "Enter Country here..";
            txtCountry.Foreground = Brushes.Gray;

            txtDescription.Text = "Enter Description here..";
            txtDescription.Foreground = Brushes.Gray;

            txtMaxGuests.Text = "0";
            txtMaxGuests.Foreground = Brushes.Gray;

            txtStartCheckPoint.Text = "Start Point";
            txtStartCheckPoint.Foreground = Brushes.Gray;

            txtOptionCheckPoint.Text = "Additional Check Point";
            txtOptionCheckPoint.Foreground = Brushes.Gray;

            txtEndCheckPoint.Text = "End Point";
            txtEndCheckPoint.Foreground = Brushes.Gray;

            txtDates.Text = "dd-MM-yyyy HH:mm";
            txtDates.Foreground = Brushes.Gray;

            txtDuration.Text = "Hours.Minutes";
            txtDuration.Foreground = Brushes.Gray;

            txtLanguage.SelectedIndex = 0;
            txtLanguage.Foreground = Brushes.Gray;
        }

        // Adds an Additional CheckPoint TextBox to the TextBox list
        private void btnAddCheckPoint_Click(object sender, RoutedEventArgs e)
        {
            TextBox lastBox = ListCheckPoints.LastOrDefault();

            if (lastBox.Text == "" || lastBox.Foreground == Brushes.Gray)
            {
                MessageBox.Show("Prazan", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            TextBox newTextBox = new TextBox
            {
                Margin = new Thickness(19, 3, 18, 3),
                Text = "",
                Foreground = Brushes.Black,
                Height = 22
            };

            StackPanel stackPanel = btnAddCheckPoint.Parent as StackPanel;
            int secondIndex = stackPanel.Children.IndexOf(btnAddCheckPoint);

            stackPanel.Children.Insert(secondIndex, newTextBox);

            ListCheckPoints.Add(newTextBox);

            newTextBox.Focus();
        }


        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Multiselect = true;

            bool? response = ofd.ShowDialog();

            if (response == true)
            {
                foreach (var item in ofd.FileNames)
                {
                    MessageBox.Show(item);
                }
            }
        }

        private void btnCancelTour_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }


        // Adds an additional DatesAndTime TextBox to the TextBox list
        private void btnAddDate_Click(object sender, RoutedEventArgs e)
        {
            TextBox lastBox = ListDates.Last();
            string pattern = @"^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-\d{4} (0\d|1\d|2[0-3]):([0-5]\d)$";

            if (lastBox.Text == "" || lastBox.Foreground == Brushes.Gray)
            {
                MessageBox.Show("You did not enter a date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!Regex.IsMatch(lastBox.Text, pattern))
            {
                MessageBox.Show("You did not enter a valid date format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            TextBox newTextBox = new TextBox
            {
                Margin = new Thickness(19, 3, 18, 0),
                Text = "",
                Foreground = Brushes.Black,
                Height = 22

            };
            ListDates.Add(newTextBox);


            StackPanel stackPanel = btnAddDate.Parent as StackPanel;
            int secondIndex = stackPanel.Children.IndexOf(btnAddDate);

            stackPanel.Children.Insert(secondIndex, newTextBox);

            newTextBox.Focus();

        }

        private bool ValidateFormData()
        {
            bool isValid = true;

            if (txtName.Text == "" || txtName.Foreground == Brushes.Gray)
            {

            }
            else
            {

            }

            return isValid;
        }

        private void GotFocus(string txt, TextBox tb)
        {
            if (tb.Text.Trim().Equals(txt))
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }

        }

        private void LostFocus(string txt, TextBox tb)
        {
            if (tb.Text.Trim().Equals(""))
            {
                tb.Text = txt;
                tb.Foreground = Brushes.Gray;
            }
        }

        private void txtDates_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtDates.Text, txtDates);
        }

        private void txtDates_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("dd-MM-yyyy HH:mm", txtDates);
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtName.Text, txtName);
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("Enter Name here..", txtName);
        }

        private void txtCity_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtCity.Text, txtCity);
        }

        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("Enter City here..", txtCity);
        }

        private void txtCountry_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtCountry.Text, txtCountry);
        }

        private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("Enter Country here..", txtCountry);
        }

        private void txtDescription_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtDescription.Text, txtDescription);
        }

        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("Enter Description here..", txtDescription);
        }

        private void txtMaxGuests_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtMaxGuests.Text, txtMaxGuests);
        }

        private void txtMaxGuests_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("0", txtMaxGuests);
        }

        private void txtStartCheckPoint_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtStartCheckPoint.Text, txtStartCheckPoint);
        }

        private void txtStartCheckPoint_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("Start Point", txtStartCheckPoint);
        }

        private void txtOptionCheckPoint_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtOptionCheckPoint.Text, txtOptionCheckPoint);
        }

        private void txtOptionCheckPoint_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("Additional Check Point", txtOptionCheckPoint);
        }

        private void txtEndCheckPoint_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtEndCheckPoint.Text, txtEndCheckPoint);
        }

        private void txtEndCheckPoint_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("End Point", txtEndCheckPoint);
        }

        private void txtDuration_GotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus(txtDuration.Text, txtDuration);
        }

        private void txtDuration_LostFocus(object sender, RoutedEventArgs e)
        {
            LostFocus("Hours.Minutes", txtDuration);
        }

        private void txtDuration_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtDates_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
