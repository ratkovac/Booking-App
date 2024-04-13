using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
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

namespace BookingApp.View.Tourist.Pages
{
    public partial class GradeTourView : Page
    {
        public BookingApp.Model.Tourist Tourist { get; set; }
        public BookingApp.Model.TourReservation TourReservation { get; set; }
        private GradeTourService gradeTourService { get; set; }
        private TourReservationService tourReservationService { get; set; }
        private TourGuestRepository tourGuestRepository { get; set; }

        public GradeTourView(BookingApp.Model.TourReservation tourReservation, BookingApp.Model.Tourist t, TourReservationService trs)
        {
            InitializeComponent();
            DataContext = this;

            Tourist = t;
            TourReservation = tourReservation;
            gradeTourService = new GradeTourService();
            tourGuestRepository = new TourGuestRepository();
            tourReservationService = trs;
        }

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            int tourGrade = FindTourGrade();
            if (tourGrade == 0)
            {
                MessageBox.Show("Morate ocijeniti znanje vodica!");
                return;
            }

            List<string> imageList = new List<string>();
            if (!string.IsNullOrEmpty(ImagesBox.Text))
            {
                string images = ImagesBox.Text.Remove(ImagesBox.Text.Length - 1, 1);
                foreach (string image in images.Split(','))
                {
                    imageList.Add(image);
                }
            }
            else
            {
                imageList.Add("");
            }
            GradeTour gradeTour = new GradeTour(Tourist.Id, Tourist, TourReservation.Id, tourGrade, AddedComentBox.Text, imageList);
            gradeTourService.Create(gradeTour);
            //BookingApp.Model.TourReservation reservation = tourReservationService.GetReservationByTouristAndTourInstance(TourReservation, Tourist);
            TourReservation.RatedTour = true;
            tourReservationService.Update(TourReservation);
        }
        private int FindTourGrade()
        {
            ComboBoxItem selectedItem = (ComboBoxItem)NumberPicker.SelectedItem;
            int selectedGrade;
            if (selectedItem != null && int.TryParse(selectedItem.Content.ToString(), out selectedGrade))
            {
                return selectedGrade;
            }
            return 0;
        }
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openFileDlg.ShowDialog();

            string apsolutePath = "";
            if (result == true)
            {
                apsolutePath = openFileDlg.FileName;
            }
            else
            {
                return;
            }
            ImagesBox.Text += GetRelativePath(apsolutePath) + ",";
        }
        private string GetRelativePath(string apsolutePath)
        {
            string[] helpString = apsolutePath.Split('\\');
            string nameFile = helpString.Last();
            return "/Resources/Images/" + nameFile;
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
