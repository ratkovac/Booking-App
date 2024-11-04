using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel.Tourist;
using System;
using System.Collections.Generic;
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
    public partial class TourTrackingView : Page
    {
        public List<TourGuest> TourGuests { get; set; }
        public BookingApp.Model.TourReservation TourReservation { get; set; }
        public TourGuestRepository tourGuestRepository;

        public TourTrackingView(BookingApp.Model.TourReservation tourReservation)
        {
            InitializeComponent();
            DataContext = this;
            TourReservation = tourReservation;
            tourGuestRepository = new TourGuestRepository();
            TourGuests = tourGuestRepository.GetAllByTourReservationId(TourReservation.Id);
        }
    }
}
