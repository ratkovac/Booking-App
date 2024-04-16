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
    public partial class FastDrivePage : Page
    {
        private FastDriveViewModel viewModel;
        public FastDrivePage(FastDriveViewModel fastDriveViewModel)
        {
            InitializeComponent();
            this.DataContext = fastDriveViewModel;
            viewModel = fastDriveViewModel;
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(viewModel.Reservation());
        }
    }
}
