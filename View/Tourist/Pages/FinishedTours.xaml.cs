using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.ViewModel.Tourist;
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

namespace BookingApp.View.Tourist.Pages
{
    public partial class FinishedTours : Page
    {
        public FinishedToursViewModel viewModel;
        public FinishedTours(FinishedToursViewModel finishedToursViewModel)
        {
            InitializeComponent();
            this.DataContext = finishedToursViewModel;
            viewModel = finishedToursViewModel;
            viewModel.NavigateToGradeTour = NavigateToGradeTour;
        }

        /*private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            var gradeTour = new GradeTourView(SelectedTourInstance, Tourist, tourReservationService);
            NavigationService.Navigate(gradeTour);
        }*/
        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            viewModel.GradeTour();
        }

        private void NavigateToGradeTour()
        {
            var gradeTour = new GradeTourView(viewModel.SelectedTourInstance, viewModel.Tourist, viewModel.tourReservationService);
            NavigationService.Navigate(gradeTour);
        }

        /*private void Home_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is FinishedToursViewModel viewModel)
            {
                navigationService.Navigate(new AvailableTours(viewModel.Tourist));
            }
        }*/
    }
}
