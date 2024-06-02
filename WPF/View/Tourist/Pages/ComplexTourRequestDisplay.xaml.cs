using BookingApp.Model;
using BookingApp.Service;
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
    public partial class ComplexTourRequestDisplay : Page
    {
        public ComplexRequestDisplayViewModel ViewModel { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }
        public LocationService _locationService { get; set; }
        public LanguageService _languageService { get; set; }
        public TourRequestService _tourRequestService { get; set; }
        public TourRequestSegmentService _tourRequestSegmentService { get; set; }
        public TourRequestGuestService _tourRequestGuestService { get; set; }

        public ComplexTourRequestDisplay(ComplexRequestDisplayViewModel complexRequestDisplayViewModel, BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            this.DataContext = complexRequestDisplayViewModel;
            ViewModel = complexRequestDisplayViewModel;
            Tourist = tourist;
            _tourRequestService = new TourRequestService();
            _tourRequestSegmentService = new TourRequestSegmentService();
            _tourRequestGuestService = new TourRequestGuestService();
            _locationService = new LocationService();
            _languageService = new LanguageService();
        }

        private void ComplexRequestSegments_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            BookingApp.Model.TourRequest selectedTourRequest = (BookingApp.Model.TourRequest)button.DataContext;

            var complexRequestSegmentsView = new ComplexRequestSegmentsView(selectedTourRequest, Tourist);
            NavigationService.Navigate(complexRequestSegmentsView);
        }

        private void ComplexTourRequest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ComplexTourRequest(Tourist.User, _locationService, _languageService, _tourRequestService, _tourRequestSegmentService, _tourRequestGuestService));
        }
    }
}
