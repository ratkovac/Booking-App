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

namespace BookingApp.View.GuideView.Pages
{
    /// <summary>
    /// Interaction logic for TouristListPage.xaml
    /// </summary>
    public partial class TouristListPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<TourReservation> _tourReservations;

        private ObservableCollection<TourGuest> _tourGuests;
        public ObservableCollection<TourGuest> TourGuests
        {
            get { return _tourGuests; }
            set
            {
                _tourGuests = value;
                OnPropertyChanged();
            }
        }

        private List<TourGuest> _selectedTourGuests;
        public List<TourGuest> SelectedTourGuest
        {
            get { return _selectedTourGuests; }
            set
            {
                _selectedTourGuests = value;
                OnPropertyChanged();
            }
        }

        private TourReservationRepository tourReservationRepository = new TourReservationRepository();
        private TourGuestRepository tourGuestRepository = new TourGuestRepository();

        private int checkPointId;
        public TouristListPage(int tourId, int _checkPointId)
        {
            InitializeComponent();
            DataContext = this;
            _tourReservations = new List<TourReservation>(tourReservationRepository.GetAllByTourInstanceId(tourId));
            TourGuests = new ObservableCollection<TourGuest>(GetTourGuests(_tourReservations));
            checkPointId = _checkPointId;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Pronađi TourGuest objekat na osnovu selektovanog checkbox-a
            var checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.DataContext is TourGuest)
            {
                var tourGuest = checkBox.DataContext as TourGuest;

                // Ažuriraj CheckPointId
                tourGuest.CheckpointId = checkPointId; // checkPointId je promenljiva koju imate dostupnu u klasi
                tourGuestRepository.Update(tourGuest);
                // Obavesti interfejs o promeni
                OnPropertyChanged(nameof(TourGuests));
            }
        }
        private List<TourGuest> GetTourGuests(List<TourReservation> tourReservations)
        {
            List<TourGuest> tourGuests = new List<TourGuest>();

            foreach(TourReservation tourReservation in tourReservations)
            {
                tourGuests = tourGuestRepository.GetAllByTourReservationId(tourReservation.Id);               
            }

            return tourGuests;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this); // Dobijanje referenca na roditeljski prozor stranice

            if (window != null)
            {
                window.Close(); // Zatvaranje roditeljskog prozora
            }
            else
            {
                MessageBox.Show("Nije moguće zatvoriti stranicu."); // Ako prozor nije pronađen, prikaži poruku
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
