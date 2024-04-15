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
using BookingApp.DTO;
using BookingApp.View.ViewModel.Guest;

namespace BookingApp.View.NGuest
{
    /// <summary>
    /// Interaction logic for DelayReservations.xaml
    /// </summary>
    public partial class DelayReservations : Window
    {
        private DelayReservationViewModel delayReservationViewModel;
        public DelayReservations(DelayReservationViewModel delayReservationViewModel)
        {
            InitializeComponent();
            this.DataContext = delayReservationViewModel;
            this.delayReservationViewModel = delayReservationViewModel;
        }

        private void Delay_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            delayReservationViewModel.CreateNewDelayReservations();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            delayReservationViewModel.FindAllFreeReservation();
        }
    }
}
