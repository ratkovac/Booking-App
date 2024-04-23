using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.GuideView.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Xml.Linq;


namespace BookingApp.View.GuideView

{
    /// <summary>
    /// Interaction logic for TourForm.xaml
    /// </summary>
    public partial class TourForm : Window, INotifyPropertyChanged
    {
        private User user;

        private string _pageName;
        public string PageName
        {
            get { return _pageName; }
            set
            {
                if (_pageName != value)
                {
                    _pageName = value;
                    OnPropertyChanged(nameof(PageName));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TourForm(User user)
        {
            InitializeComponent();
            ActionBar actionBar = new ActionBar(MainFrame);
            DataContext = this;
            this.user = user;
        }

        public void btnNavigation_Click(object sender, RoutedEventArgs e)
        {
            // Provera da li postoji referenca na glavni okvir (mainFrame)
            MainFrame.Content = Resources["MainFrameContent"];
        }
        private void ActionBar_NavigationButtonClicked(object sender, EventArgs e)
        {
            // Implementirajte logiku koja se izvršava kada se klikne dugme navigacije
            // Na primer, možete navigirati na drugu stranicu
            MainFrame.Navigate(new Uri("View/GuideView/Pages/TourManagmentPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnCreateTour_Click(object sender, RoutedEventArgs e)
        {
            CreateTourPage createTourPage = new CreateTourPage(user);
            MainFrame.Navigate(createTourPage);
            //MainFrame.Navigate(new Uri("View/GuideView/Pages/CreateTourPage.xaml", UriKind.RelativeOrAbsolute));
            PageName = "Create Tour";
        }

        private void btnTrackTourLive_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("View/GuideView/Pages/TrackTourLivePage.xaml", UriKind.RelativeOrAbsolute));
            PageName = "Track Tour Live";
        }

        private void btnTourStatistics_Click(object sender, RoutedEventArgs e)
        {
            TourStatisticPage tourStatisticPage = new TourStatisticPage(user);
            // Implementirajte logiku za dugme Tour Statistics
            MainFrame.Navigate(tourStatisticPage);
            PageName = "Tour Statistic";
        }

        private void btnTourReviews_Click(object sender, RoutedEventArgs e)
        {
            TourReviewsPage tourReviewsPage = new TourReviewsPage(user);
            // Implementirajte logiku za dugme Tour Statistics
            MainFrame.Navigate(tourReviewsPage);
            PageName = "Tour Reviews";
        }

        private void btnCancelTour_Click(object sender, RoutedEventArgs e)
        {
            CancelTourPage cancelTourPage = new CancelTourPage(user);
            // Implementirajte logiku za dugme Tour Statistics
            MainFrame.Navigate(cancelTourPage);
            PageName = "Cancel Tour";
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

        private void actionBar_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        // public void btnNavigation_Click(object sender, RoutedEventArgs e)
        // {
        //     // Navigacija na TourManagementPage
        //     MainFramee.Navigate(new Uri("View/GuideView/Pages/TourManagementPage.xaml", UriKind.Relative));
        // }

    }
}
