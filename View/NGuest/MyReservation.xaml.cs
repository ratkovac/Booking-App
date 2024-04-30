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
using System.Windows.Shapes;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.View.ViewModel.Guest;

namespace BookingApp.View.NGuest
{
    /// <summary>
    /// Interaction logic for MyReservation.xaml
    /// </summary>
    public partial class MyReservation : Window
    {
        private MyReservationViewModel MyReservationViewModel { get; set; }
        public MyReservation(MyReservationViewModel myReservationViewModel)
        {
            InitializeComponent();
            this.DataContext = myReservationViewModel;
            MyReservationViewModel = myReservationViewModel;
        }

        private void OnClick_Decline(object sender, RoutedEventArgs e)
        {
            MyReservationViewModel.DeleteOnClick();
        }

        private void OnClick_Delay(object sender, RoutedEventArgs e)
        {
            MyReservationViewModel.OnClickDelay();
        }
    }
}
