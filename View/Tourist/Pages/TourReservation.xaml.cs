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

        //public Tour Tour { get; set; }
        public int UserId { get; set; }
        public TourInstance SelectedTourInstance { get; set; }
        public Tour SelectedTour { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }

        private readonly TourRepository tourRepository;
        private readonly CheckPointRepository checkPointRepository = new CheckPointRepository();
        private readonly TourInstanceRepository _tourInstanceRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourGuestRepository _tourGuestRepository;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourReservation(Tour selectedTour, User user)
        {
            InitializeComponent();
            DataContext = this;
            UserId = user.Id;
            SelectedTour = selectedTour;

            _tourInstanceRepository = new TourInstanceRepository();
            _tourReservationRepository = new TourReservationRepository();
            _tourGuestRepository = new TourGuestRepository();

            NameTextBox.Text = selectedTour.Name;
            LocationTextBox.Text = selectedTour.Location.City;
            //LocationTextBox.Text = selectedTour.Location.Country;
            DescriptionTextBox.Text = selectedTour.Description;
            LanguageTextBox.Text = selectedTour.Language.Name;
            MaxGuestsTextBox.Text = selectedTour.MaxGuests.ToString();

            //DateStartTextBox.Text = TourTime.time.ToString("dd.MM.yyyy HH:mm");
            DurationTextBox.Text = selectedTour.Duration.ToString();

            CheckPoints = checkPointRepository.GetCheckPoints(selectedTour.Id);
            KeyPointTextBox.Text = string.Join(Environment.NewLine, CheckPoints.Select(cp => cp.PointText));

            GenerateDatesComboBox();
        }

        private void GenerateDatesComboBox()
        {
            var tourInstances = _tourInstanceRepository.GetAllById(SelectedTour.Id);
            StartTimeComboBox.ItemsSource = tourInstances.Select(t => t.StartTime.ToString("g")).ToList();
            RequiredSeatsComboBox.IsEnabled = false;
        }

        private void GenerateInputFields(int numberOfPeople)
        {
            InputGuestsStackPanel.Children.Clear();

            for (int i = 0; i < numberOfPeople; i++)
            {
                InputGuestsStackPanel.Children.Add(new TextBox { Margin = new Thickness(0, 0, 0, 10) });
                InputGuestsStackPanel.Children.Add(new TextBox { Margin = new Thickness(0, 0, 0, 20) });
            }
        }

        private void ChangedStartTimeComboBox(object sender, SelectionChangedEventArgs e)
        {
            if (!(StartTimeComboBox.SelectedItem is string selectedTime) || !DateTime.TryParse(selectedTime, out DateTime selectedDate))
            {
                MessageBox.Show("Date not valid.");
                return;
            }

            RequiredSeatsComboBox.IsEnabled = true;
            SelectedTourInstance = _tourInstanceRepository.GetByIdAndDate(SelectedTour.Id, selectedDate);
        }

        private void RequiredSeatsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(RequiredSeatsComboBox.SelectedItem is ComboBoxItem selectedItem) || !int.TryParse(selectedItem.Content.ToString(), out int requiredSeats))
            {
                return;
            }


            if (requiredSeats > SelectedTourInstance.AvailableSlots)
            {
                int reservedSeats = SelectedTour.MaxGuests - (SelectedTourInstance?.AvailableSlots ?? 0);
                int remainingSeats = SelectedTourInstance.AvailableSlots;
                MessageBox.Show($"Na ovoj turi nema dovoljan broj slobodnih mjesta za unijeti broj ljudi.\nBroj rezervisanih mjesta je: {reservedSeats}.\nBroj slobodnih mjesta je: {remainingSeats}.");
                RequiredSeatsComboBox.SelectedItem = null;
                return;
            }

            GenerateInputFields(requiredSeats);
        }

        private int GetSelectedNumberOfPeople(ComboBox comboBox)
        {
            if (comboBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Content.ToString(), out int numberOfPeople))
            {
                return numberOfPeople;
            }
            return 0;
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {

            var guests = GetGuestsFromInputFields();
            var numberOfPeople = GetSelectedNumberOfPeople(RequiredSeatsComboBox);


            UpdateTourInstanceCapacity((int)numberOfPeople);
            SaveTourReservation(guests);

            MessageBox.Show("Rezervacija uspjesna!");
        }

        private void UpdateTourInstanceCapacity(int requiredSeats)
        {
            SelectedTourInstance.AvailableSlots -= requiredSeats;
            _tourInstanceRepository.Update(SelectedTourInstance);
        }

        private void SaveTourReservation(List<TourGuest> tourGuests)
        {
            BookingApp.Model.TourReservation tourReservation = new BookingApp.Model.TourReservation(SelectedTourInstance.Id, UserId);
            _tourGuestRepository.SaveMultiple(tourGuests);
            _tourReservationRepository.Save(tourReservation);
        }

        private List<TourGuest> GetGuestsFromInputFields()
        {
            var guests = new List<TourGuest>();

            for (int i = 0; i < InputGuestsStackPanel.Children.Count; i += 2)
            {
                var firstNameBox = InputGuestsStackPanel.Children[i] as TextBox;
                var lastNameBox = InputGuestsStackPanel.Children[i + 1] as TextBox;

                if (firstNameBox != null && lastNameBox != null)
                {
                    string fullName = $"{firstNameBox.Text} {lastNameBox.Text}";
                    var tourGuest = new TourGuest(fullName, SelectedTourInstance.Id, UserId, 0);
                    guests.Add(tourGuest);
                }
            }

            return guests;
        }

        /*private void Reservation_Click(object sender, RoutedEventArgs e)
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
            }

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

        private void InputDialog_Click(object sender, RoutedEventArgs e)
        {
            InputDialog.IsOpen = false;
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
        }*/

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
