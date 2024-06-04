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
    public partial class TourRequest : Page
    {
        private TourRequestViewModel ViewModel;
        public TourRequest(User tourist, LocationService locationService, LanguageService languageService, TourRequestService requestService, TourRequestSegmentService segmentService, TourRequestGuestService tourRequestGuestService)
        {
            InitializeComponent();
            ViewModel = new TourRequestViewModel(tourist, locationService, languageService, requestService, segmentService, tourRequestGuestService);
            DataContext = ViewModel;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateTourRequest();
            MessageBox.Show("Tour request added successfully!");
        }
    }
}
