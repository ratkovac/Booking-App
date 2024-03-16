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

        public Tour Tour { get; set; }
        public ObservableCollection<Tour> TourList { get; set; }
        public Tour SelectedTour { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        private readonly TourRepository tourRepository = new TourRepository();

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
            Tour = tourSelected;

            NameTextBox.Text = tourSelected.Name;
            LocationTextBox.Text = tourSelected.Location.City;
            LocationTextBox.Text = tourSelected.Location.Country;
            DescriptionTextBox.Text = tourSelected.Description;
            LanguageTextBox.Text = tourSelected.Language.Name;
            MaxGuestsTextBox.Text = tourSelected.MaxGuests.ToString();

            //DateStartTextBox.Text = TourTime.time.ToString("dd.MM.yyyy HH:mm");
            DurationTextBox.Text = tourSelected.Duration.ToString();

            CheckPoints = checkPointRepository.GetCheckPoints(tourSelected.Id);
            KeyPointTextBox.Text = string.Join(Environment.NewLine, CheckPoints.Select(cp => cp.PointText));

        }

        public TourReservation()
        {
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            int numberGuests = CheckNumberGuestTextBox(NumberGuestsTextBox.Text);
            if (numberGuests == -1) return;

            if (Tour.AvailableSeats == 0)
            {
                List<Tour> allTours = tourRepository.GetAll();

                List<Tour> availableTours = allTours.Where(t => t.AvailableSeats > 0).ToList();

                AlternativeToursGrid.ItemsSource = availableTours;

                AlternativeTextBlock.Text = "Izabrana tura je u potpunosti rezervisana, neke od alternativnih tura su:";
                AlternativeToursGrid.Visibility = Visibility.Visible;
                return;
            }

            if (numberGuests > Tour.AvailableSeats)
            {
                MessageBox.Show("Na ovoj turi nema dovoljan broj slobodnih mjesta za unijeti broj ljudi. " +
                    "\nBroj slobodnih mjesta je: " + Tour.AvailableSeats + "!");
                return;
            }

            Tour.AvailableSeats -= numberGuests;
            //tourRepository.Update(Tour);

            /*List<string> guestNames = new List<string>();
            for (int i = 0; i < numberGuests; i++)
            {
                InputDialog.IsOpen = true;
                // Čekanje na korisnički unos
                while (InputDialog.IsOpen)
                {
                    // Čekanje...
                }
                guestNames.Add(GuestNameTextBox.Text);
            }*/

            BookingApp.Model.TourReservation newReservation = new BookingApp.Model.TourReservation
            {
                TourId = Tour.Id,
                Tour = Tour,
                NumberGuest = numberGuests,
                Name = Tour.Name,
                //GuestNames = guestNames
            };

            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            tourReservationRepository.Save(newReservation);

            MessageBox.Show("Tura je uspešno rezervisana!");
        }

        /*private void InputDialog_Click(object sender, RoutedEventArgs e)
        {
            InputDialog.IsOpen = false;
        }*/


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
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Nema prethodne stranice!");
            }
        }
    }
}
