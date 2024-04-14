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
        private LanguageRepository languageRepository = new LanguageRepository();
        private ImageRepository imageRepository = new ImageRepository();

        private List<Tour> Tours = new List<Tour>();
        private List<string> PathImages = new List<string>();
        
        private List<TextBox> CheckPoints = new List<TextBox>();
        private List<TextBox> Dates = new List<TextBox>();

        public CreateTourPage()
        {
            InitializeComponent();
            CheckPoints.Add(txtStartCheckPoint);
            CheckPoints.Add(txtOptionCheckPoint);

            Dates.Add(txtDates);

        }


        private void btnCreateTour_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string city = txtCity.Text;
            string country = txtCountry.Text;
            string description = txtDescription.Text;
            string lang = ((ComboBoxItem)txtLanguage.SelectedItem).Content.ToString();
            int maxGuests;
            float duration;


            if (!ValidateMaxGuests(txtMaxGuests.Text, out maxGuests))
                return;

            if (!ValidateDuration(txtDuration.Text, out duration))
                return;


            Language language = CreateAndSaveLanguage(lang);
            Location location = CreateAndSaveLocation(city, country);
            Tour tour = CreateTour(name, description, maxGuests, duration, location, language);
            SaveTour(tour);


            SaveImages(PathImages, tour.Id);
            GetCheckPoints(CheckPoints, tour.Id);
            GetDateAndTimes(Dates, tour.Id);

            ClearFields();
        }

        private bool ValidateMaxGuests(string input, out int maxGuests)
        {
            if (!int.TryParse(input, out maxGuests))
            {
                MessageBox.Show("Invalid Max Number of Tourists");
                return false;
            }
            return true;
        }

        private bool ValidateDuration(string input, out float duration)
        {
            if (!float.TryParse(input, out duration))
            {
                MessageBox.Show("Invalid Duration");
                return false;
            }
            return true;
        }

        private Language CreateAndSaveLanguage(string lang)
        {
            Language language = new Language(lang);
            languageRepository.Save(language);
            return language;
        }

        private Location CreateAndSaveLocation(string city, string country)
        {
            Location location = new Location(city, country);
            locationRepository.Save(location);
            return location;
        }

        private Tour CreateTour(string name, string description, int maxGuests, float duration, Location location, Language language)
        {
            return new Tour(name, description, maxGuests, duration, location, language);
        }

        private void SaveTour(Tour tour)
        {
            tourRepository.Save(tour);
        }


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


        private List<DateRealization> GetDateAndTimes(List<TextBox> listDates, int tourId)
        {
            List<DateRealization> dates = new List<DateRealization>();

            foreach (TextBox textBox in listDates)
            {
                string dateString = textBox.Text;
                DateTime parsedDate;
                if (TryParseDate(dateString, out parsedDate))
                {
                    DateRealization dateRealization = CreateDateRealization(parsedDate, tourId);
                    SaveDateRealization(dateRealization);
                    dates.Add(dateRealization);
                }
                else
                {
                    ShowInvalidDateFormatMessage(dateString);
                }
            }

            return dates;
        }

        private bool TryParseDate(string dateString, out DateTime parsedDate)
        {
            return DateTime.TryParseExact(dateString, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
        }

        private DateRealization CreateDateRealization(DateTime parsedDate, int tourId)
        {
            return new DateRealization(parsedDate, tourId);
        }

        private void SaveDateRealization(DateRealization dateRealization)
        {
            dateRealizationRepository.Save(dateRealization);
        }

        private void ShowInvalidDateFormatMessage(string dateString)
        {
            MessageBox.Show($"Nije moguće parsirati datum: {dateString}");
        }


        private void SaveImages(List<string> pathImages, int tourId)
        {
            foreach (string pathImage in pathImages)
            {
                Model.Image image = new Model.Image(pathImage, -1, tourId);
                imageRepository.Save(image);
            }
        }
        private void ClearFields()
        {
            SetFieldDefaults(txtName, "Enter Name here..", Brushes.Gray);
            SetFieldDefaults(txtCity, "Enter City here..", Brushes.Gray);
            SetFieldDefaults(txtCountry, "Enter Country here..", Brushes.Gray);
            SetFieldDefaults(txtDescription, "Enter Description here..", Brushes.Gray);
            SetFieldDefaults(txtMaxGuests, "0", Brushes.Gray);
            SetFieldDefaults(txtStartCheckPoint, "Start Point", Brushes.Gray);
            SetFieldDefaults(txtOptionCheckPoint, "Additional Check Point", Brushes.Gray);
            SetFieldDefaults(txtEndCheckPoint, "End Point", Brushes.Gray);
            SetFieldDefaults(txtDates, "dd-MM-yyyy HH:mm", Brushes.Gray);
            SetFieldDefaults(txtDuration, "Hours.Minutes", Brushes.Gray);
            SetLanguageDefault();
        }

        private void SetFieldDefaults(TextBox textBox, string defaultText, Brush defaultBrush)
        {
            textBox.Text = defaultText;
            textBox.Foreground = defaultBrush;
        }

        private void SetLanguageDefault()
        {
            txtLanguage.SelectedIndex = 0;
            txtLanguage.Foreground = Brushes.Gray;
        }


        private void btnAddCheckPoint_Click(object sender, RoutedEventArgs e)
        {
            if (IsLastCheckpointEmpty())
            {
                MessageBox.Show("Poslednje polje za tacku nije popunjeno.", "Greska", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AddNewCheckpointTextBox();
        }

        private bool IsLastCheckpointEmpty()
        {
            TextBox lastBox = CheckPoints.LastOrDefault();
            return string.IsNullOrWhiteSpace(lastBox?.Text) || lastBox.Foreground == Brushes.Gray;
        }

        private void AddNewCheckpointTextBox()
        {
            TextBox newTextBox = CreateNewCheckpointTextBox();

            StackPanel stackPanel = btnAddCheckPoint.Parent as StackPanel;
            int secondIndex = stackPanel.Children.IndexOf(btnAddCheckPoint);

            stackPanel.Children.Insert(secondIndex, newTextBox);
            CheckPoints.Add(newTextBox);

            newTextBox.Focus();
        }

        private TextBox CreateNewCheckpointTextBox()
        {
            return new TextBox
            {
                Margin = new Thickness(19, 3, 18, 3),
                Text = "",
                Foreground = Brushes.Black,
                Height = 22
            };
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
                    PathImages.Add(item);
                    MessageBox.Show(item);
                }
            }
        }



        private void btnCancelTour_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }


        private void btnAddDate_Click(object sender, RoutedEventArgs e)
        {
            TextBox lastBox = Dates.Last();
            string pattern = @"^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-\d{4} (0\d|1\d|2[0-3]):([0-5]\d)$";

            if (IsLastDateEmpty(lastBox))
            {
                MessageBox.Show("Niste uneli datum.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!IsDateValid(lastBox.Text, pattern))
            {
                MessageBox.Show("Niste uneli ispravan format datuma.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AddNewDateTextBox();
        }

        private bool IsLastDateEmpty(TextBox lastBox)
        {
            return string.IsNullOrWhiteSpace(lastBox?.Text) || lastBox.Foreground == Brushes.Gray;
        }

        private bool IsDateValid(string dateText, string pattern)
        {
            return Regex.IsMatch(dateText, pattern);
        }

        private void AddNewDateTextBox()
        {
            TextBox newTextBox = CreateNewDateTextBox();

            Dates.Add(newTextBox);

            StackPanel stackPanel = btnAddDate.Parent as StackPanel;
            int secondIndex = stackPanel.Children.IndexOf(btnAddDate);

            stackPanel.Children.Insert(secondIndex, newTextBox);

            newTextBox.Focus();
        }

        private TextBox CreateNewDateTextBox()
        {
            return new TextBox
            {
                Margin = new Thickness(19, 3, 18, 0),
                Text = "",
                Foreground = Brushes.Black,
                Height = 22
            };
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
