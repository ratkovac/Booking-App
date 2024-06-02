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
    public partial class MyProfile : Page
    {
        public BookingApp.Model.Tourist Tourist { get; set; }
        public TourRequestService _tourRequestService;
        public TourRequestSegmentService _tourRequestSegmentService;

        public MyProfile(BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            Tourist = tourist;
            _tourRequestService = new TourRequestService();
            _tourRequestSegmentService = new TourRequestSegmentService();
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TourRequestStatsView(Tourist, _tourRequestService, _tourRequestSegmentService));
        }
    }
}
