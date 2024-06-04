using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.WPF.ViewModel.Tourist;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class TourReservationView : Page, INotifyPropertyChanged
    {
        private int _numberOfPeople;
        private bool _canBookNow;
        private string _selectedStartTime;

        public int UserId { get; set; }
        public TourInstance SelectedTourInstance { get; set; }
        public Tour SelectedTour { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public Voucher SelectedVoucher { get; set; }
        public ObservableCollection<Voucher> ListVoucher { get; set; }
        public ObservableCollection<TourGuestViewModel> TourGuestInputs { get; } = new ObservableCollection<TourGuestViewModel>();
        public BookingApp.Model.Tourist Tourist { get; set; }

        private VoucherService voucherService;
        private readonly TourInstanceRepository _tourInstanceRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourGuestRepository _tourGuestRepository;

        public int NumberOfPeople
        {
            get => _numberOfPeople;
            set
            {
                if (_numberOfPeople != value)
                {
                    _numberOfPeople = value;
                    OnPropertyChanged(nameof(NumberOfPeople));
                    if (CheckAvailableSeats(_numberOfPeople))
                    {
                        GenerateGuests(value);
                        UpdateCanBookNow();
                    }
                }
            }
        }

        public string SelectedStartTime
        {
            get => _selectedStartTime;
            set
            {
                if (_selectedStartTime != value)
                {
                    _selectedStartTime = value;
                    OnPropertyChanged(nameof(SelectedStartTime));
                }
            }
        }

        public bool CanBookNow
        {
            get => _canBookNow;
            private set
            {
                if (_canBookNow != value)
                {
                    _canBookNow = value;
                    OnPropertyChanged(nameof(CanBookNow));
                }
            }
        }

        public TourReservationView(Tour selectedTour, BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            UserId = tourist.Id;
            SelectedTour = selectedTour;
            Tourist = tourist;

            voucherService = new VoucherService();
            _tourInstanceRepository = new TourInstanceRepository();
            _tourReservationRepository = new TourReservationRepository();
            _tourGuestRepository = new TourGuestRepository();
            ListVoucher = new ObservableCollection<Voucher>(voucherService.GetActiveVouchers(tourist.VoucherIds));
            GenerateDatesComboBox();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CheckIfTourInstancesExist()
        {
            var tourInstances = _tourInstanceRepository.GetAllById(SelectedTour.Id)
                .Where(t => t.AvailableSlots > 0 && t.State != TourInstanceState.Finished && t.State != TourInstanceState.Cancelled);

            return tourInstances.Any();
        }

        private void GenerateDatesComboBox()
        {
            var tourInstances = _tourInstanceRepository.GetAllById(SelectedTour.Id)
                .Where(t => t.AvailableSlots > 0 && t.State != TourInstanceState.Finished && t.State != TourInstanceState.Cancelled);

            StartTimeComboBox.ItemsSource = tourInstances.Select(t => t.StartTime.ToString("g")).ToList();
        }

        private void ChangedStartTimeComboBox(object sender, SelectionChangedEventArgs e)
        {
            if (!(StartTimeComboBox.SelectedItem is string selectedTime) || !DateTime.TryParse(selectedTime, out DateTime selectedDate))
            {
                MessageBox.Show("Date not valid.");
                return;
            }

            SelectedTourInstance = _tourInstanceRepository.GetByIdAndDate(SelectedTour.Id, selectedDate);
            SelectedStartTime = selectedTime;
        }

        private void GenerateGuests(int numberOfPeople)
        {
            TourGuestInputs.Clear();
            if (numberOfPeople > 0)
            {
                for (int i = 0; i < numberOfPeople; i++)
                {
                    var guest = new TourGuestViewModel();
                    guest.PropertyChanged += (sender, e) => UpdateCanBookNow();
                    TourGuestInputs.Add(guest);
                }
            }
            UpdateCanBookNow();
        }

        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumberOfPeopleTextBox.Text, out int numberOfPeople))
            {
                if (numberOfPeople < 1 || numberOfPeople > 120)
                {
                    MessageBox.Show("Number of people must be between 1 and 120.");
                    NumberOfPeopleTextBox.Clear();
                }
                else
                {
                    NumberOfPeople = numberOfPeople;
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
                NumberOfPeopleTextBox.Clear();
            }
        }

        private void CheckStartingTimes(object sender, EventArgs e)
        {
            if (!CheckIfTourInstancesExist())
            {
                MessageBox.Show("Nema dostupnih termina za ovu turu.");
                StartTimeComboBox.IsDropDownOpen = false;
                NavigationService.GoBack();
                return;
            }
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            var numberOfPeople = NumberOfPeople;
            var guests = new List<TourGuest>();

            foreach (var guest in TourGuestInputs)
            {
                var guestName = $"{guest.FirstName} {guest.LastName}";

                var tourGuest = new TourGuest(guestName, guest.Age.ToString(), SelectedTourInstance.Id, UserId, 0);
                guests.Add(tourGuest);
            }

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
                NumberOfPeople = 0;
                NumberOfPeopleTextBox.Clear();
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

        private void UpdateCanBookNow()
        {
            bool allGuestsValid = TourGuestInputs.All(guest =>
                !string.IsNullOrWhiteSpace(guest.FirstName) &&
                !string.IsNullOrWhiteSpace(guest.LastName) &&
                guest.Age > 0 && guest.Age <= 120);

            CanBookNow = !string.IsNullOrEmpty(SelectedStartTime) && allGuestsValid;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            TourGuestInputs.Clear();
            NumberOfPeople = 0;
            SelectedTourInstance = null;
            SelectedVoucher = null;
            var activeVouchers = voucherService.GetActiveVouchers(Tourist.VoucherIds);
            ListVoucher.Clear();
            foreach (var voucher in activeVouchers)
            {
                ListVoucher.Add(voucher);
            }
            StartTimeComboBox.SelectedItem = null;
            NumberOfPeopleTextBox.Clear();
        }
    }

}
