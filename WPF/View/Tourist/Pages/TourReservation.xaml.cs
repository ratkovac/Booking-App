using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
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

namespace BookingApp.WPF.View.Tourist.Pages
{

    public partial class TourReservation : Page
    {

        public int UserId { get; set; }
        public TourInstance SelectedTourInstance { get; set; }
        public Tour SelectedTour { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public Voucher SelectedVoucher { get; set; }
        public ObservableCollection<Voucher> ListVoucher { get; set; }

        private VoucherService voucherService;
        private readonly CheckPointRepository _checkPointRepository;
        private readonly TourInstanceRepository _tourInstanceRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourGuestRepository _tourGuestRepository;

        public TourReservation(Tour selectedTour, BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            UserId = tourist.Id;
            SelectedTour = selectedTour;

            voucherService = new VoucherService();
            _checkPointRepository = new CheckPointRepository();
            _tourInstanceRepository = new TourInstanceRepository();
            _tourReservationRepository = new TourReservationRepository();
            _tourGuestRepository = new TourGuestRepository();
            ListVoucher = new ObservableCollection<Voucher>(voucherService.GetActiveVouchers(tourist.VoucherIds));

            NameTextBox.Text = selectedTour.Name;
            LocationTextBox.Text = selectedTour.Location.City;
            //LocationTextBox.Text = selectedTour.Location.Country;
            DescriptionTextBox.Text = selectedTour.Description;
            LanguageTextBox.Text = selectedTour.Language.Name;
            MaxGuestsTextBox.Text = selectedTour.MaxGuests.ToString();
            //DateStartTextBox.Text = TourTime.time.ToString("dd.MM.yyyy HH:mm");
            DurationTextBox.Text = selectedTour.Duration.ToString();
            CheckPoints = _checkPointRepository.GetCheckPoints(selectedTour.Id);
            CheckPointTextBox.Text = string.Join(Environment.NewLine, CheckPoints.Select(cp => cp.PointText));

            GenerateDatesComboBox();
        }

        private void GenerateDatesComboBox()
        {
            var tourInstances = _tourInstanceRepository.GetAllById(SelectedTour.Id)
                .Where(t => t.AvailableSlots > 0 && t.State != TourInstanceState.Finished && t.State != TourInstanceState.Cancelled);

            StartTimeComboBox.ItemsSource = tourInstances.Select(t => t.StartTime.ToString("g")).ToList();
            NumberOfPeopleTextBox.IsEnabled = false;
        }

        private void GenerateInputFields(int numberOfPeople)
        {
            InputGuestsStackPanel.Children.Clear();

            for (int i = 0; i < numberOfPeople; i++)
            {
                Grid guestGrid = new Grid { Margin = new Thickness(200, 0, -50, 10) };

                ColumnDefinition column1 = new ColumnDefinition();
                ColumnDefinition column2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
                guestGrid.ColumnDefinitions.Add(column1);
                guestGrid.ColumnDefinitions.Add(column2);

                TextBox nameTextBox = new TextBox { Height = 30, Width = 200, Text = "Ime i prezime", Margin = new Thickness(0, 0, 10, 0) };
                nameTextBox.GotFocus += (s, e) => { if (nameTextBox.Text == "Ime i prezime") nameTextBox.Text = ""; };
                nameTextBox.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(nameTextBox.Text)) nameTextBox.Text = "Ime i prezime"; };
                Grid.SetColumn(nameTextBox, 0);

                TextBox ageTextBox = new TextBox { Height = 30, Width = 50, Text = "Godine" };
                ageTextBox.GotFocus += (s, e) => { if (ageTextBox.Text == "Godine") ageTextBox.Text = ""; };
                ageTextBox.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(ageTextBox.Text)) ageTextBox.Text = "Godine"; };
                Grid.SetColumn(ageTextBox, 1);

                guestGrid.Children.Add(nameTextBox);
                guestGrid.Children.Add(ageTextBox);

                InputGuestsStackPanel.Children.Add(guestGrid);
            }
        }

        private void ChangedStartTimeComboBox(object sender, SelectionChangedEventArgs e)
        {
            if (!(StartTimeComboBox.SelectedItem is string selectedTime) || !DateTime.TryParse(selectedTime, out DateTime selectedDate))
            {
                MessageBox.Show("Date not valid.");
                return;
            }

            NumberOfPeopleTextBox.IsEnabled = true;
            SelectedTourInstance = _tourInstanceRepository.GetByIdAndDate(SelectedTour.Id, selectedDate);
        }

        private int GetNumberOfPeople()
        {
            if (int.TryParse(NumberOfPeopleTextBox.Text, out int numberOfPeople))
            {
                return numberOfPeople;
            }
            return 0;
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            var guests = GetGuestsFromInputFields();
            var numberOfPeople = GetNumberOfPeople();

            if (guests == null || guests.Count == 0 || numberOfPeople == 0)
            {
                return;
            }

            if (!CheckAvailableSeats(numberOfPeople))
            {
                return;
            }

            UpdateTourInstanceCapacity(numberOfPeople);
            bool isReservationSaved = SaveTourReservation(guests);

            if (isReservationSaved)
            {
                MessageBox.Show("Rezervacija uspješna!");
            }
            else
            {
                MessageBox.Show("Rezervacija nije uspješna. Molimo pokušajte ponovo.");
            }
        }

        private bool CheckAvailableSeats(int numberOfPeople)
        {
            int requiredSeats = numberOfPeople;
            if (requiredSeats > SelectedTourInstance.AvailableSlots)
            {
                int reservedSeats = SelectedTour.MaxGuests - (SelectedTourInstance?.AvailableSlots ?? 0);
                int remainingSeats = SelectedTourInstance.AvailableSlots;
                MessageBox.Show($"Na ovoj turi nema dovoljan broj slobodnih mjesta za unijeti broj ljudi.\nBroj rezervisanih mjesta je: {reservedSeats}.\nBroj slobodnih mjesta je: {remainingSeats}.");
                return false;
            }
            return true;
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
                if (SelectedVoucher != null)
                {
                    SelectedVoucher.Used = true;
                    SelectedVoucher.ValidVoucher = false;
                    voucherService.Update(SelectedVoucher);
                    BookingApp.Model.TourReservation tourReservation = new BookingApp.Model.TourReservation(SelectedTourInstance.Id, UserId, true, false);
                    _tourGuestRepository.SaveMultiple(tourGuests);
                    _tourReservationRepository.Save(tourReservation);
                    return true;
                }
                else
                {
                    BookingApp.Model.TourReservation tourReservation = new BookingApp.Model.TourReservation(SelectedTourInstance.Id, UserId, false, false);
                    _tourGuestRepository.SaveMultiple(tourGuests);
                    _tourReservationRepository.Save(tourReservation);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private List<TourGuest> GetGuestsFromInputFields()
        {
            var guests = new List<TourGuest>();

            foreach (Grid guestGrid in InputGuestsStackPanel.Children)
            {
                var nameTextBox = guestGrid.Children.OfType<TextBox>().FirstOrDefault(t => Grid.GetColumn(t) == 0);
                var ageTextBox = guestGrid.Children.OfType<TextBox>().FirstOrDefault(t => Grid.GetColumn(t) == 1);

                if (nameTextBox != null && ageTextBox != null &&
                    nameTextBox.Text != "Ime i prezime" && ageTextBox.Text != "Godine")
                {
                    string fullName = nameTextBox.Text;
                    string age = ageTextBox.Text;

                    var tourGuest = new TourGuest(fullName, age, SelectedTourInstance.Id, UserId, 0);
                    guests.Add(tourGuest);
                }
            }

            if (guests.Count < InputGuestsStackPanel.Children.Count)
            {
                return new List<TourGuest>();
            }

            return guests;
        }

        private void NumberOfGuestsTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (int.TryParse(NumberOfPeopleTextBox.Text, out int numberOfGuests) && numberOfGuests > 0)
                {
                    GenerateInputFields(numberOfGuests);
                }
                else
                {
                    MessageBox.Show("Unesite validan broj gostiju.");
                }
            }
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
