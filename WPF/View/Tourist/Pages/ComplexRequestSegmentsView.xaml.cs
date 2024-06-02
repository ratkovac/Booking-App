using BookingApp.Model;
using BookingApp.Service;
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
    public partial class ComplexRequestSegmentsView : Page
    {
        public List<TourRequestSegment> ListTourSegments { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }
        public TourRequestSegmentService _tourRequestSegmentService;
        public BookingApp.Model.TourRequest TourRequest { get; set; }

        public ComplexRequestSegmentsView(BookingApp.Model.TourRequest tourRequest, BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            _tourRequestSegmentService = new TourRequestSegmentService();
            TourRequest = tourRequest;
            Tourist = tourist;
            ListTourSegments = _tourRequestSegmentService.GetAllComplexSegmentsByComplexTourRequestId(TourRequest.Id);
        }

        private void TourRequestDescription_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TourRequestSegment selectedSegment = (TourRequestSegment)button.DataContext;

            var tourRequestDescription = new TourRequestDescription(selectedSegment, Tourist);
            NavigationService.Navigate(tourRequestDescription);
        }
    }
}
