using BookingApp.DTO;
using BookingApp.View.ViewModel.Owner;
using System.Windows;

namespace BookingApp.View.Owner
{
    public partial class AccommodationStatistic : Window
    {
        public AccommodationDTO SelectedAccommodation { get; private set; }

        public RenovationDatesDTO SelectedDate { get; set; }

        public AccommodationStatistic(AccommodationStatisticViewModel accommodationStatisitcViewModel, AccommodationDTO selectedAccommodationDTO)
        {
            InitializeComponent();
            this.DataContext = accommodationStatisitcViewModel;
            //this.addRenovationViewModel = addRenovationViewModel;
            SelectedAccommodation = selectedAccommodationDTO;
        }
    }
}
