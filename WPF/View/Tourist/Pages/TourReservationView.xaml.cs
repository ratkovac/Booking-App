using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.WPF.ViewModel.Tourist;
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
        public int UserId { get; set; }
        public TourInstance SelectedTourInstance { get; set; }
        public Tour SelectedTour { get; set; }
        public List<CheckPoint> CheckPoints { get; set; }
        public Voucher SelectedVoucher { get; set; }
        public ObservableCollection<Voucher> ListVoucher { get; set; }
        public ObservableCollection<TourGuestViewModel> TourGuestInputs { get; } = new ObservableCollection<TourGuestViewModel>();
        private ObservableCollection<int> _numberOfPeopleSelection = new ObservableCollection<int>();

        private VoucherService voucherService;
        private readonly TourInstanceRepository _tourInstanceRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly TourGuestRepository _tourGuestRepository;

        public ObservableCollection<int> NumberOfPeopleSelection
        {
            get => _numberOfPeopleSelection;
            private set
            {
                _numberOfPeopleSelection = value;
                OnPropertyChanged(nameof(NumberOfPeopleSelection));
            }
        }

        public int NumberOfPeople
        {
            get => _numberOfPeople;
            set
            {
                if (_numberOfPeople != value)
                {
                    _numberOfPeople = value;
                    GenerateGuests(value);
                    OnPropertyChanged(nameof(NumberOfPeople));
                }
            }
        }

        public TourReservationView(Tour selectedTour, BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            UserId = tourist.Id;
            SelectedTour = selectedTour;
            NumberOfPeopleOptions();

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
        }

        private void NumberOfPeopleOptions()
        {
            NumberOfPeopleSelection.Clear();
            NumberOfPeopleSelection.Add(1);
            NumberOfPeopleSelection.Add(2);
            NumberOfPeopleSelection.Add(3);
            NumberOfPeopleSelection.Add(4);
            NumberOfPeopleSelection.Add(5);
        }

        private void GenerateGuests(int numberOfPeople)
        {
            TourGuestInputs.Clear();
            if (numberOfPeople > 0)
            {
                for (int i = 0; i < numberOfPeople; i++)
                {
                    TourGuestInputs.Add(new TourGuestViewModel());
                }
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
