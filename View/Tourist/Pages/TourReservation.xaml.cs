using BookingApp.Model;
using BookingApp.Repository;
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

namespace BookingApp.View.Tourist.Pages
{

    public partial class TourReservation : Page, INotifyPropertyChanged
    {

        public Tour tour { get; set; }
        public ObservableCollection<Tour> TourList { get; set; }
        public Tour SelectedTour { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }

        CheckPointRepository checkPointRepository = new CheckPointRepository();

        private int _numberGuest;
        public int NumberGuests
        {
            get => _numberGuest;
            set
            {
                if (value != _numberGuest)
                {
                    _numberGuest = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourReservation(Tour tourSelected)
        {
            InitializeComponent();
            DataContext = this;
            tour = tourSelected;

            NameTextBox.Text = tourSelected.Name;
            LocationTextBox.Text = tourSelected.Location.City;
            LocationTextBox.Text = tourSelected.Location.Country;
            DescriptionTextBox.Text = tourSelected.Description;
            LanguageTextBox.Text = tourSelected.Language.Name;
            MaxGuestsTextBox.Text = tourSelected.MaxGuests.ToString();

            //DateStartTextBox.Text = TourTime.time.ToString("dd.MM.yyyy HH:mm");
            DurationTextBox.Text = tourSelected.Duration.ToString();

            //CheckPoints = checkPointRepository.GetCheckPoints(tourSelected.Id);

        }

        public TourReservation()
        {
        }

        private int CheckNumberGuestTextBox(string text)
        {
            uint numberGuests;

            if (!uint.TryParse(text, out numberGuests))
            {
                MessageBox.Show("Wrong input! Number people on tour must be a positive number!");
                return -1;
            }

            if (numberGuests == 0)
            {
                MessageBox.Show("Wrong input! Number people on tour can't be a zero!");
                return -1;
            }
            return (int)numberGuests;
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
