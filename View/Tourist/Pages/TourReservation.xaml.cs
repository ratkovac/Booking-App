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
                TextBox firstName = new TextBox { Height = 30, Text = "Ime", Margin = new Thickness(0, 0, 0, 10) };
                firstName.GotFocus += (s, e) => { if (firstName.Text == "Ime") firstName.Text = ""; };
                firstName.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(firstName.Text)) firstName.Text = "Ime"; };

                TextBox lastName = new TextBox { Height = 30, Text = "Prezime", Margin = new Thickness(0, 0, 0, 20) };
                lastName.GotFocus += (s, e) => { if (lastName.Text == "Prezime") lastName.Text = ""; };
                lastName.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(lastName.Text)) lastName.Text = "Prezime"; };

                InputGuestsStackPanel.Children.Add(firstName);
                InputGuestsStackPanel.Children.Add(lastName);
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

            if (guests == null || guests.Count == 0 || numberOfPeople == 0)
            {
                return;
            }

            UpdateTourInstanceCapacity((int)numberOfPeople);
            bool isReservationSaved = SaveTourReservation(guests);

            if (isReservationSaved)
            {
                MessageBox.Show("Rezervacija uspjesna!");
            }
            else
            {
                MessageBox.Show("Rezervacija nije uspjesna. Molimo pokušajte ponovo.");
            }
        }

        private void UpdateTourInstanceCapacity(int requiredSeats)
        {
            SelectedTourInstance.AvailableSlots -= requiredSeats;
            _tourInstanceRepository.Update(SelectedTourInstance);
        }

        private bool SaveTourReservation(List<TourGuest> tourGuests)
        {
            try
            {
                BookingApp.Model.TourReservation tourReservation = new BookingApp.Model.TourReservation(SelectedTourInstance.Id, UserId);
                _tourGuestRepository.SaveMultiple(tourGuests);
                _tourReservationRepository.Save(tourReservation);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private List<TourGuest> GetGuestsFromInputFields()
        {
            var guests = new List<TourGuest>();

            for (int i = 0; i < InputGuestsStackPanel.Children.Count; i += 2)
            {
                var firstNameBox = InputGuestsStackPanel.Children[i] as TextBox;
                var lastNameBox = InputGuestsStackPanel.Children[i + 1] as TextBox;

                if (firstNameBox != null && lastNameBox != null &&
                    firstNameBox.Text != "Ime" && lastNameBox.Text != "Prezime")
                {
                    string fullName = $"{firstNameBox.Text} {lastNameBox.Text}";
                    var tourGuest = new TourGuest(fullName, SelectedTourInstance.Id, UserId, 0);
                    guests.Add(tourGuest);
                }
            }

            // Ako nisu sva polja popunjena validnim imenima i prezimenima, vrati praznu listu
            if (guests.Count < InputGuestsStackPanel.Children.Count / 2)
            {
                return new List<TourGuest>();
            }

            return guests;
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
