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

            CheckPoints = checkPointRepository.GetCheckPoints(tourSelected.Id);

        }

        public TourReservation()
        {
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            int numberGuests = CheckNumberGuestTextBox(NumberGuestsTextBox.Text);
            if (numberGuests == -1) return;

            if (tour.AvailableSeats == 0)
            {
                // Pretpostavimo da imate TourRepository koji sadrži sve ture
                TourRepository tourRepository = new TourRepository();
                List<Tour> allTours = tourRepository.GetAll();

                // Filtrirajte listu da sadrži samo ture koje imaju dostupna mesta
                List<Tour> availableTours = allTours.Where(t => t.AvailableSeats > 0).ToList();

                // Postavite AlternativeToursGrid.ItemsSource na listu dostupnih tura
                AlternativeToursGrid.ItemsSource = availableTours;

                AlternativeTextBlock.Text = "Izabrana tura je u potpunosti rezervisana, neke od alternativnih tura su:";
                AlternativeToursGrid.Visibility = Visibility.Visible;
                return;
            }

            if (numberGuests > tour.AvailableSeats)
            {
                MessageBox.Show("Na ovoj turi nema dovoljan broj slobodnih mjesta za unijeti broj ljudi. " +
                    "\nBroj slobodnih mjesta je: " + tour.AvailableSeats + "!");
                return;
            }

            tour.AvailableSeats -= numberGuests;

            BookingApp.Model.TourReservation newReservation = new BookingApp.Model.TourReservation
            {
                TourId = tour.Id,
                Tour = tour,
                NumberGuest = numberGuests
            };

            // Sačuvajte novu rezervaciju u CSV fajl
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            tourReservationRepository.Save(newReservation);

            MessageBox.Show("Tura je uspešno rezervisana!");
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
