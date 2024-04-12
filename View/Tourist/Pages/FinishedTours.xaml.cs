using BookingApp.Model;
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
        public TourInstance SelectedTourInstance { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }
        public FinishedTours(BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            this.DataContext = new FinishedToursViewModel(tourist);
            Tourist = tourist;
        }

        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            var gradeTour = new GradeTourView(SelectedTourInstance, Tourist);
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
