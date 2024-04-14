using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Model;
using System.Windows.Controls;
using System.Globalization;

namespace BookingApp.View.Owner
{
    public partial class GuestGrade : Window, IObserver, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<AccommodationReservationDTO> Reservations { get; set; }
        private AccommodationReservationRepository accommodationReservationRepository { get; set; }
        private GradeGuestRepository gradeGuestRepository { get; set; }
        public GradeGuestDTO gradeGuestDTO { get; set; }
        public AccommodationReservationDTO selectedGuest { get; set; }

        public User LoggedInUser;

        public GuestGrade(User user)
        {
            InitializeComponent();
            DataContext = this;
            Reservations = new ObservableCollection<AccommodationReservationDTO>();
            accommodationReservationRepository = new AccommodationReservationRepository();
            gradeGuestRepository = new GradeGuestRepository();
            gradeGuestDTO = new GradeGuestDTO();
            LoggedInUser = user;
            ShowOwnerGuests();
            UnratedGuests();
            Grade.IsEnabled = false;
        }

        public int DaysUntilGuestRating(AccommodationReservation reservation)
        {
            DateTime today = DateTime.Now;
            DateTime endDate = DateTime.Parse(reservation.EndDate.ToString(), CultureInfo.InvariantCulture);
            DateTime endDateRating = endDate.AddDays(5);
            TimeSpan difference = endDateRating - today;
            return difference.Days;
        }

        private bool IsValidForRating(AccommodationReservation reservation)
        {
            return reservation.UserGrade == 0.0 && reservation.Accommodation.User.Id == LoggedInUser.Id
                     && DaysUntilGuestRating(reservation) >= 0 && DaysUntilGuestRating(reservation) < 5;
        }
        private void ShowOwnerGuests()
        {
            List<AccommodationReservation> reservations = accommodationReservationRepository.GetAll();
            Reservations.Clear();

            foreach (AccommodationReservation reservation in reservations)
            {
                if (IsValidForRating(reservation))
                {
                    Reservations.Add(new AccommodationReservationDTO
                    {
                        Id = reservation.Id,
                        UserName = reservation.User.Username,
                        AccommodationName = reservation.Accommodation.Name,
                        DaysToRating = DaysUntilGuestRating(reservation)
                    });
                }
            }
            GuestsGrid.ItemsSource = Reservations;
        }

        public int UnratedGuestsNumber()
        {
            return Reservations.Count;
        }
        private void UnratedGuests()
        {
            if (UnratedGuestsNumber() > 0)
                MessageBox.Show("Still, you have unrated guests!\nYou have their details in the table.", "Rate guest!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void SliderValues()
        {
            gradeGuestDTO.Cleanliness = Convert.ToInt32(CleanlinessValue.Text);
            gradeGuestDTO.RulesFollowing = Convert.ToInt32(RulesValue.Text);
        }

        private void Grade_Click(object sender, RoutedEventArgs e)
        {
            SliderValues();
            gradeGuestDTO.AccommodationReservation = accommodationReservationRepository.GetByID(selectedGuest.Id);
            GradeGuest gradeGuest = gradeGuestDTO.ToGradeGuest();
            gradeGuestRepository.Save(gradeGuest);

            AccommodationReservation oldReservation = gradeGuestDTO.AccommodationReservation;
            oldReservation.UserGrade = GetGuestGrade();
            accommodationReservationRepository.Update(oldReservation);
            MessageBox.Show("Chosen guest is rated!");
            ShowGradeGuestPage();
        }

        public void ShowGradeGuestPage()
        {
            ShowOwnerGuests();
            comment.Text = "";
            cleanliness.SetValue(Slider.ValueProperty, cleanliness.Minimum);
            rules.SetValue(Slider.ValueProperty, rules.Minimum);
            Grade.IsEnabled = false;
        }
        private void ChosenGuest_Click(object sender, RoutedEventArgs e)
        {
            if (GuestsGrid.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to grade this guest?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    selectedGuest = (AccommodationReservationDTO)GuestsGrid.SelectedItem;
                    GuestsGrid.IsEnabled = false;
                    ChosenGuest.IsEnabled = false;
                    Grade.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("No guest selected.");
            }
        }
        public double GradeCalculation(int cleanMark, int ruleMark)
        {
            int sum = cleanMark + ruleMark;
            return (double)sum / 2;
        }
        private double GetGuestGrade()
        {
            double grade;
            int cleanlinessGrade, rulesFollowingGrade;
            SliderValues();
            cleanlinessGrade = gradeGuestDTO.Cleanliness;
            rulesFollowingGrade = gradeGuestDTO.RulesFollowing;
            grade = GradeCalculation(cleanlinessGrade, rulesFollowingGrade);
            return grade;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}