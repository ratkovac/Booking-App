using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookingApp.GUI_Elements;
using BookingApp.Service;
using BookingApp.View.NGuest;
using BookingApp.View.ViewModel.Guest;
using GalaSoft.MvvmLight.Views;
using static BookingApp.Model.AccommodationTypeEnum;
using Menu = BookingApp.View.NGuest.Menu;
using System.Windows.Navigation;


namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Guest.xaml
    /// </summary>
    public partial class Guest : Page
    {

        private AccommodationRepository AccommodationRepository { get; set; }
        public AccommodationDTO? SelectedAccommodation { get; set; }
        public ObservableCollection<AccommodationDTO> Accommodations { get; set; }
        public ObservableCollection<string> Locations { get; set; }
        public ICollectionView FilteredAccommodations { get; set; }

        private DelayReservationService delayReservationService { get; set; }
        private ObservableCollection<DelayReservation> DelayReservations { get; set; }

        private Cache.Cache cache;
        public User LoggedInUser { get; set; }

        public Guest(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;

            Accommodations = new ObservableCollection<AccommodationDTO>();
            AccommodationRepository = new AccommodationRepository();

            Locations = new ObservableCollection<string>();

            FilteredAccommodations = CollectionViewSource.GetDefaultView(Accommodations);

            SelectedAccommodation = new AccommodationDTO();

            delayReservationService = new DelayReservationService();
            DelayReservations = new ObservableCollection<DelayReservation>();

            cache = new Cache.Cache();
            Update();
            LoggedInUser = loggedInUser;

        }

        

        private User user;
        public User User
        {
            get
            {
                return user;
            }
            set
            {
                if (user != value)
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }

        public void Update()
        {
            Accommodations.Clear();
            var allLocations = new HashSet<Location>();
            foreach(Accommodation accommodation in AccommodationRepository.GetAll())
            {
                Accommodations.Add(new AccommodationDTO(accommodation));
                allLocations.Add(accommodation.Location);
            }

            Locations.Clear();
            Locations.Add(" ");
            foreach (Location location in allLocations)
            {
                Locations.Add(location.ToString());
            }

            DelayReservations.Clear();
            foreach (DelayReservation delayReservation in delayReservationService.GetAll())
            {
                DelayReservations.Add(delayReservation);
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void MyReservations_Click(object sender, RoutedEventArgs e)
        {
            MyReservationViewModel myReservationViewModel = new MyReservationViewModel(LoggedInUser);
            MyReservation myReservation = new MyReservation(myReservationViewModel);
            myReservation.Show();
        }

        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            RateAcciommodationViewModel rateAcciommodationViewModel = new RateAcciommodationViewModel(LoggedInUser, NavigationService);
            RateAccommodations rateAccommodations = new RateAccommodations(rateAcciommodationViewModel);
            rateAccommodations.Show();
        }

        private void ContentControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var contentControl = sender as ContentControl;
            if (contentControl != null)
            {
                var accommodation = contentControl.DataContext as AccommodationDTO;
                if (accommodation != null)
                {
                    ItemsControlExtensions.SetSelectedItem(contentControl, accommodation);
                    Reservation reservationPage = new Reservation(accommodation, LoggedInUser);
                    NavigationService.Navigate(reservationPage);
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuViewModel menuViewModel = new MenuViewModel("Home", NavigationService, LoggedInUser);
            NavigationService.Navigate(new Menu(menuViewModel));
        }

        private void OnClick_Filter_Sort(object sender, RoutedEventArgs e)
        {
            FilterAndSortViewModel filterAndSortViewModel = new FilterAndSortViewModel(FilteredAccommodations, Locations, NavigationService, cache);
            NavigationService.Navigate(new FilterAndSort(filterAndSortViewModel));
        }

        private void OnClick_Review(object sender, RoutedEventArgs e)
        {
            ReviewViewModel reviewViewModel = new ReviewViewModel(LoggedInUser.Id);
            Review review = new Review(reviewViewModel);
            NavigationService.Navigate(review);
        }
    }
}