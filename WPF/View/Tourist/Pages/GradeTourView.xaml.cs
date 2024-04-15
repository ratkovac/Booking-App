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

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class GradeTourView : Page
    {
        public BookingApp.Model.Tourist Tourist { get; set; }
        public BookingApp.Model.TourReservation TourReservation { get; set; }
        public List<string> tourGrades = new List<string>();
        public List<string> comments = new List<string>();
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

        private void GuestRatingsButton_Click(object sender, RoutedEventArgs e)
        {
            tourGrades = FindTourGradesForAllGuests();
        }

        private void Rating_Click(object sender, RoutedEventArgs e)
        {
            string touristGrade = FindTourGrade();
            tourGrades.Add(touristGrade);

            if (tourGrades.Count == 0)
            {
                MessageBox.Show("Morate ocijeniti turu!");
                return;
            }

            List<string> imageList = GetImageList();
            GradeTour gradeTour = new GradeTour(Tourist.Id, Tourist, TourReservation.Id, tourGrades, AddedComentBox.Text, imageList);
            gradeTourService.Create(gradeTour);
            TourReservation.RatedTour = true;
            tourReservationService.Update(TourReservation);
        }

        private List<string> FindTourGradesForAllGuests()
        {
            List<TourGuest> presentGuests = tourGuestRepository.GetAllPresentByTourReservationId(TourReservation.Id);
            List<string> tourGrades = presentGuests.Select(guest =>
            {
                GradeDialog gradeDialog = new GradeDialog();
                return gradeDialog.ShowDialog() == true ? gradeDialog.GetTourGrade() : "0";
            }).ToList();

            return tourGrades;
        }

        private string FindTourGrade()
        {
            ComboBoxItem selectedItem = (ComboBoxItem)NumberPicker.SelectedItem;
            if (selectedItem != null)
            {
                return selectedItem.Content.ToString();
            }
            return "0";
        }

        private List<string> GetImageList()
        {
            List<string> imageList = new List<string>();
            if (!string.IsNullOrEmpty(ImagesBox.Text))
            {
                string images = ImagesBox.Text.Remove(ImagesBox.Text.Length - 1, 1);
                imageList.AddRange(images.Split(','));
            }
            else
            {
                imageList.Add("");
            }
            return imageList;
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
