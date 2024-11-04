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
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Owner
{
    public partial class GuestGrade : Window, IObserver, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<AccommodationReservationDTO> Reservations { get; set; }
        private AccommodationReservationRepository accommodationReservationRepository { get; set; }
        private GradeGuestRepository gradeGuestRepository { get; set; }
        public GradeGuestDTO gradeGuestDTO { get; set; }
        public AccommodationReservationDTO selectedGuest { get; set; }
        private Border SelectedBorder { get; set; }
        private ImageRepository imageRepository;

        public User LoggedInUser;

        public GuestGrade(User user)
        {
            InitializeComponent();
            DataContext = this;
            Reservations = new ObservableCollection<AccommodationReservationDTO>();
            accommodationReservationRepository = new AccommodationReservationRepository();
            gradeGuestRepository = new GradeGuestRepository();
            gradeGuestDTO = new GradeGuestDTO();
            imageRepository = new ImageRepository();
            LoggedInUser = user;
            ShowOwnerGuests();
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
                    string imagePath;
                    Image frontImage = imageRepository.GetByAccommodationId(reservation.Accommodation.Id);
                    if (frontImage != null)
                    {
                        imagePath = frontImage.Path;
                    }
                    else
                    {
                        imagePath = "/View/Owner/noimage.png";
                    }
                    Reservations.Add(new AccommodationReservationDTO
                    {
                        Id = reservation.Id,
                        UserName = reservation.User.Username,
                        AccommodationName = reservation.Accommodation.Name,
                        StartDate = reservation.StartDate,
                        EndDate = reservation.EndDate,
                        DaysToRating = DaysUntilGuestRating(reservation),
                        FrontImagePath = imagePath
                    });
                }
            }
            
        }
        private void CleanilnessGrade(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image clickedStar = sender as System.Windows.Controls.Image;
            int clickedStarIndex = CleanlinessStackPanel.Children.IndexOf(clickedStar);

            for (int i = 0; i < CleanlinessStackPanel.Children.Count; i++)
            {
                System.Windows.Controls.Image star = CleanlinessStackPanel.Children[i] as System.Windows.Controls.Image;
                if (i <= clickedStarIndex)
                {
                    star.Source = new BitmapImage(new Uri("/View/Owner/star_filled.png", UriKind.Relative));
                }
                else
                {
                    star.Source = new BitmapImage(new Uri("/View/Owner/star_empty.png", UriKind.Relative));
                }
            }
            gradeGuestDTO.Cleanliness = clickedStarIndex + 1;
        }
        private void RulesFollowingGrade(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image clickedStar = sender as System.Windows.Controls.Image;
            int clickedStarIndex = RulesFollowingStackPanel.Children.IndexOf(clickedStar);

            // Prolazi kroz sve slike zvezdica i ažurira njihov izgled na osnovu indeksa kliknute zvezdice
            for (int i = 0; i < RulesFollowingStackPanel.Children.Count; i++)
            {
                System.Windows.Controls.Image star = RulesFollowingStackPanel.Children[i] as System.Windows.Controls.Image;
                if (i <= clickedStarIndex)
                {
                    star.Source = new BitmapImage(new Uri("/View/Owner/star_filled.png", UriKind.Relative));
                }
                else
                {
                    star.Source = new BitmapImage(new Uri("/View/Owner/star_empty.png", UriKind.Relative));
                }
            }
            gradeGuestDTO.RulesFollowing = clickedStarIndex + 1;
        }
        private void ResetCleanlinessStars()
        {
            foreach (var child in CleanlinessStackPanel.Children)
            {
                if (child is System.Windows.Controls.Image star)
                {
                    star.Source = new BitmapImage(new Uri("/View/Owner/star_empty.png", UriKind.Relative));
                }
            }
        }
        private void ResetRulesFollowingStars()
        {
            foreach (var child in RulesFollowingStackPanel.Children)
            {
                if (child is System.Windows.Controls.Image star)
                {
                    star.Source = new BitmapImage(new Uri("/View/Owner/star_empty.png", UriKind.Relative));
                }
            }
        }
        public int UnratedGuestsNumber()
        {
            return Reservations.Count;
        }
        

        private void Grade_Click(object sender, RoutedEventArgs e)
        {
            gradeGuestDTO.AccommodationReservation = accommodationReservationRepository.GetByID(selectedGuest.Id);
            GradeGuest gradeGuest = gradeGuestDTO.ToGradeGuest();
            gradeGuestRepository.Save(gradeGuest);

            AccommodationReservation oldReservation = gradeGuestDTO.AccommodationReservation;
            oldReservation.UserGrade = GetGuestGrade();
            accommodationReservationRepository.Update(oldReservation);
            MessageBox.Show("Chosen guest is rated!");
            ShowGradeGuestPage();
            ResetCleanlinessStars();
            ResetRulesFollowingStars();
        }

        public void ShowGradeGuestPage()
        {
            ShowOwnerGuests();
            NewComment.Text = "";
            Grade.IsEnabled = false;
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
            cleanlinessGrade = gradeGuestDTO.Cleanliness;
            rulesFollowingGrade = gradeGuestDTO.RulesFollowing;
            grade = GradeCalculation(cleanlinessGrade, rulesFollowingGrade);
            return grade;
        }

        public void Update()
        {
            ShowOwnerGuests();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Guest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NewComment.Text = "";
            ResetCleanlinessStars();
            ResetRulesFollowingStars();

            Border border = sender as Border;
            if (border != null)
            {
                AccommodationReservationDTO selectedReservation = border.DataContext as AccommodationReservationDTO;
                if (selectedReservation != null)
                {
                    // Proverite da li je kliknuta kartica već selektovana
                    if (SelectedBorder != null && SelectedBorder == border)
                    {
                        // Ako jeste, deselektujte je
                        SelectedBorder.BorderBrush = Brushes.White;
                        SelectedBorder = null;
                        selectedGuest = null;
                        // Onemogućite dugme za ocenjivanje
                        Grade.IsEnabled = false;
                    }
                    else
                    {
                        if (SelectedBorder != null)
                        {
                            SelectedBorder.BorderBrush = Brushes.White;
                        }

                        SelectedBorder = border;
                        selectedGuest = selectedReservation;
                        border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ecf007"));
                        Grade.IsEnabled = true;
                    }
                }
            }
            else
            {
                Grade.IsEnabled = false;
            }
        }

       
    }
}