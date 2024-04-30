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
using Microsoft.Win32;

namespace BookingApp.View.NGuest
{
    /// <summary>
    /// Interaction logic for Rate.xaml
    /// </summary>
    public partial class Rate : Window
    {
        private RateViewModel rateViewModel;
        public Rate(RateViewModel rateViewModel)
        {
            InitializeComponent();
            this.DataContext = rateViewModel;
            this.rateViewModel = rateViewModel;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            rateViewModel.AddGrade();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            rateViewModel.EnterPictures();
        }

    }
}
