using BookingApp.Model;
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
    public partial class MyTours : Page
    {
        public User User { get; set; }

        public MyTours(User user)
        {
            InitializeComponent();
            User = user;
            MyToursViewModel viewModel = new MyToursViewModel(user);
            DataContext = viewModel;
        }

        private void MoreDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tour = button.Tag as TourInstanceViewModel;
            if (tour != null)
            {
                var detailsWindow = new TourTracking(tour);
                mainFrame.Navigate(detailsWindow);
            }
        }
    }
}
