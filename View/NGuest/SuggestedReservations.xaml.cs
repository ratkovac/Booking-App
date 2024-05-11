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
using BookingApp.View.ViewModel.Guest;

namespace BookingApp.View.NGuest
{
    /// <summary>
    /// Interaction logic for SuggestedReservations.xaml
    /// </summary>
    public partial class SuggestedReservations : Page
    {
        public SuggestedReservations(SuggestedReservationsViewModel suggestedReservationViewModel)
        {
            InitializeComponent();
            this.DataContext = suggestedReservationViewModel;
        }

        private void ContentControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnClick_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
