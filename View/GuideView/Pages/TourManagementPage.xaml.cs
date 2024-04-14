using System;
using System.Windows;

namespace BookingApp.View.GuideView.Pages
{
    /// <summary>
    /// Interaction logic for TourManagementWindow.xaml
    /// </summary>
    public partial class TourManagementWindow : Window
    {
        //private readonly ActionBar actionBar;

        public TourManagementWindow()
        {
            InitializeComponent();
            ActionBar actionBar = new ActionBar(MainFramee);
            actionBar.NavigationButtonClicked += ActionBar_NavigationButtonClicked;
        }

        private void ActionBar_NavigationButtonClicked(object sender, EventArgs e)
        {
            // Implementirajte logiku koja se izvršava kada se klikne dugme navigacije
            // Na primer, možete navigirati na drugu stranicu
            //NavigationService.Navigate(new Uri("PutanjaDoDrugaStranica.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnCreateTour_Click(object sender, RoutedEventArgs e)
        {
            MainFramee.Navigate(new Uri("View/GuideView/Pages/CreateTourPage.xaml", UriKind.RelativeOrAbsolute));
            actionBar.PageName = "Create Tour";
        }

        private void btnTrackTourLive_Click(object sender, RoutedEventArgs e)
        {
            MainFramee.Navigate(new Uri("View/GuideView/Pages/TrackTourLivePage.xaml", UriKind.RelativeOrAbsolute));
            actionBar.PageName = "Track Tour Live";
        }

        private void btnTourStatistics_Click(object sender, RoutedEventArgs e)
        {
            // Implementirajte logiku za dugme Tour Statistics
        }

        private void btnTourReviews_Click(object sender, RoutedEventArgs e)
        {
            // Implementirajte logiku za dugme Tour Reviews
        }

        private void btnCancelTour_Click(object sender, RoutedEventArgs e)
        {
            // Implementirajte logiku za dugme Cancel Tour
        }

        private void btnTourRequests_Click(object sender, RoutedEventArgs e)
        {
            // Implementirajte logiku za dugme Tour Requests
        }

        private void btnCustomizeTour_Click(object sender, RoutedEventArgs e)
        {
            // Implementirajte logiku za dugme Customize Tour
        }

        private void btnRequestsStatistics_Click(object sender, RoutedEventArgs e)
        {
            // Implementirajte logiku za dugme Requests Statistics
        }

        // public void btnNavigation_Click(object sender, RoutedEventArgs e)
        // {
        //     // Navigacija na TourManagementPage
        //     MainFramee.Navigate(new Uri("View/GuideView/Pages/TourManagementPage.xaml", UriKind.Relative));
        // }
    }
}
