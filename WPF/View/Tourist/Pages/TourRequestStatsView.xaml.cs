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
    public partial class TourRequestStatsView : Page
    {
        public TourRequestStatisticsViewModel ViewModel { get; }
        public TourRequestStatsView(BookingApp.Model.Tourist tourist, TourRequestService requestService, TourRequestSegmentService segmentService)
        {
            InitializeComponent();
            ViewModel = new TourRequestStatisticsViewModel(tourist.Id, requestService, segmentService);
            DataContext = ViewModel;
        }
    }
}
