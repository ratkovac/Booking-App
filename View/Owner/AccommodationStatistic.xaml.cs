using BookingApp.DTO;
using BookingApp.View.ViewModel.Owner;
using System.Windows;

namespace BookingApp.View.Owner
{
    public partial class AccommodationStatistic : Window
    {
        public AccommodationDTO SelectedAccommodation { get; private set; }

        public RenovationDatesDTO SelectedDate { get; set; }

        public AccommodationStatistic(AccommodationDTO selectedAccommodationDTO)
        {
            //DataContext nije pravilno postavljen- pa sam morao preko itemsSource
            InitializeComponent();
            AccommodationStatisticViewModel accommodationStatisticViewModel = new AccommodationStatisticViewModel(selectedAccommodationDTO); 
            this.DataContext = accommodationStatisticViewModel;
            Stats.ItemsSource = accommodationStatisticViewModel.yearStatistic;
            SelectedAccommodation = selectedAccommodationDTO;
        }
    }
}
