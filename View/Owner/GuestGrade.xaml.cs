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

namespace BookingApp.View.Owner
{
    public partial class GuestGrade : Window, IObserver, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<AccommodationReservationDTO> Reservations { get; set; }
        private AccommodationReservationRepository accommodationReservationRepository { get; set; }
        private AccommodationRepository accommodationRepository { get; set; }
        private GradeGuestRepository gradeGuestRepository { get; set; }
        public GradeGuestDTO gradeGuestDTO { get; set; }
        public AccommodationReservationDTO selectedGuest { get; set; }

        ObservableCollection<GradeGuestDTO> grades;
        public User LoggedInUser { get; set; }

        public GuestGrade()
        {
            // Konstruktor bez parametara
        }
        public GuestGrade(User user) 
        {
            InitializeComponent();
            DataContext = this;
            this.grades = grades;
            Reservations = new ObservableCollection<AccommodationReservationDTO>();
            accommodationReservationRepository = new AccommodationReservationRepository();
            gradeGuestRepository = new GradeGuestRepository();
            gradeGuestDTO = new GradeGuestDTO();
            accommodationRepository = new AccommodationRepository();
            ShowOwnerGuests();
            LoggedInUser = user;
            Grade.IsEnabled = false;
        }

        private void ShowOwnerGuests()
        {
            List<AccommodationReservation> reservations = accommodationReservationRepository.GetAll();
            Reservations.Clear();

            foreach (AccommodationReservation reservation in reservations)
            {
                if(reservation.UserGrade == 0.0)
                {
                    Reservations.Add(new AccommodationReservationDTO
                    {
                        Id = reservation.Id,
                        UserName = reservation.User.Username,
                        AccommodationName = reservation.Accommodation.Name
                    });
                }
            }
            GuestsGrid.ItemsSource = Reservations;
        }
        public void Update()
        {
            throw new NotImplementedException();
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
            int accommodationId = gradeGuestDTO.AccommodationReservation.Accommodation.Id;
            GradeGuest gradeGuest = gradeGuestDTO.ToGradeGuest();
            gradeGuestRepository.Save(gradeGuest);
            selectedGuest.UserGrade = GetGuestGrade();
            selectedGuest.User = LoggedInUser;
            selectedGuest.Accommodation = accommodationRepository.GetByID(accommodationId);
            accommodationReservationRepository.Update(selectedGuest.ToAccommodationReservation());
            this.Close();
        }

        private void ChosenGuest_Click(object sender, RoutedEventArgs e)
        {
            if (GuestsGrid.SelectedItem != null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to grade this guest?", "Confirmation", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes)
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
    }
}
