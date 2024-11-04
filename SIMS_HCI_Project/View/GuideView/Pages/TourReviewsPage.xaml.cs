using BookingApp.Model;
using BookingApp.View.ViewModel.Guide;
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
using static BookingApp.View.ViewModel.Guide.TourReviewsViewModel;

namespace BookingApp.View.GuideView.Pages
{
    /// <summary>
    /// Interaction logic for TourReviews.xaml
    /// </summary>
    public partial class TourReviewsPage : Page
    {
        private readonly TourReviewsViewModel viewModel;
        public TourReviewsPage(User user)
        {
            viewModel = new TourReviewsViewModel(user);
            InitializeComponent();
            DataContext = viewModel;
        }

        private void btnTouristList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbTour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.LoadReviews(viewModel.SelectedTour.TourInstanceId);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.DataContext is TourReview tourReview)
            {
                int gradeTourId = tourReview.GradeTourId;
                viewModel.Report(gradeTourId);
            }
        }
    }
}
