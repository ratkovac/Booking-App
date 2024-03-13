using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace BookingApp.View.Tourist.Pages
{
    /// <summary>
    /// Interaction logic for AvailableTours.xaml
    /// </summary>
    public partial class AvailableTours : Page, INotifyPropertyChanged
    {

        private TourRepository tourRepository;
        public ObservableCollection<Tour> ListTour { get; set; }

        private int _numberGuest;
        public int NumberGuests
        {
            get => _numberGuest;
            set
            {
                if (value != _numberGuest)
                {
                    _numberGuest = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AvailableTours()
        {
            InitializeComponent();
            DataContext = this;

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

        public void Update()
        {
            UpdateTourList();
        }
    }
}
