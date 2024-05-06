using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class TourDescription : Page
    {

        public int UserId { get; set; }
        public TourInstance SelectedTourInstance { get; set; }
        public Tour SelectedTour { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }

        private readonly CheckPointRepository _checkPointRepository;
        private readonly TourInstanceRepository _tourInstanceRepository;

        public TourDescription(Tour selectedTour, BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            UserId = tourist.Id;
            SelectedTour = selectedTour;

            _checkPointRepository = new CheckPointRepository();
            _tourInstanceRepository = new TourInstanceRepository();

            NameTextBox.Text = selectedTour.Name;
            CityTextBox.Text = selectedTour.Location.City;
            CountryTextBox.Text = selectedTour.Location.Country;
            DescriptionTextBox.Text = selectedTour.Description;
            LanguageTextBox.Text = selectedTour.Language.Name;
            MaxGuestsTextBox.Text = selectedTour.MaxGuests.ToString();
            DurationTextBox.Text = selectedTour.Duration.ToString();
            CheckPoints = _checkPointRepository.GetCheckPoints(selectedTour.Id);
            CheckPointTextBox.Text = string.Join(Environment.NewLine, CheckPoints.Select(cp => "\u2022" + " " + cp.PointText));

            GenerateStartTimesTextBox();
        }

        private void GenerateStartTimesTextBox()
        {
            var tourInstances = _tourInstanceRepository.GetAllById(SelectedTour.Id)
                .Where(t => t.AvailableSlots > 0 && t.State != TourInstanceState.Finished && t.State != TourInstanceState.Cancelled);

            StringBuilder startTimes = new StringBuilder();
            foreach (var instance in tourInstances)
            {
                startTimes.AppendLine(instance.StartTime.ToString("g"));
            }
            StartTimesTextBox.Text = startTimes.ToString();
        }
    }
}
