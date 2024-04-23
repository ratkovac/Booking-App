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
using System.Windows.Shapes;
using BookingApp.View.ViewModel;
using BookingApp.View.ViewModel.Guest;

namespace BookingApp.View.NGuest
{
    /// <summary>
    /// Interaction logic for RateAccommodations.xaml
    /// </summary>
    public partial class RateAccommodations : Window
    {
        private RateAcciommodationViewModel rateAccommodationViewModel;
        public RateAccommodations(RateAcciommodationViewModel rateAccommodationViewModel)
        {
            InitializeComponent();
            this.DataContext = rateAccommodationViewModel;
            this.rateAccommodationViewModel = rateAccommodationViewModel;
        }

        private void OnClick_Rate(object sender, RoutedEventArgs e)
        {
            rateAccommodationViewModel.OnClickRate();
        }
    }
}
