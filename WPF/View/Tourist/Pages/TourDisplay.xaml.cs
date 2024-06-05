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

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class TourDisplay : Page, INotifyPropertyChanged
    {

        private TourRepository tourRepository;
        public ObservableCollection<Tour> ListTour { get; set; }
        public Tour SelectedTour { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }

        private int _maxNumberGuest;

        public int MaxNumberGuests
        {
            get => _maxNumberGuest;
            set
            {
                if (value != _maxNumberGuest)
                {
                    _maxNumberGuest = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourDisplay(BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            Tourist = tourist;

            tourRepository = new TourRepository();
            ListTour = new ObservableCollection<Tour>(tourRepository.GetAll());
        }

        private void UpdateTourList()
        {
            ListTour.Clear();
            foreach (var tour in tourRepository.GetAll())
            {
                ListTour.Add(tour);
            }
        }

        private void Tour_Reservation(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                var tour_reservation = new TourReservationView(SelectedTour, Tourist);
                NavigationService.Navigate(tour_reservation);
            }
            else
            {
                if (App.CurrentLanguage == "en-US")
                {
                    MessageBox.Show("You have to select a tour in order to book it!");
                }
                else
                {
                    MessageBox.Show("Moraš izabrati turu kako bi je rezervisao!");
                }
            }
        }

        private void TourDescription_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Tour selectedTour = (Tour)button.DataContext;

            var tourDescription = new TourDescription(selectedTour, Tourist);
            NavigationService.Navigate(tourDescription);
        }

        public void Update()
        {
            UpdateTourList();
        }
    }
}
