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

namespace BookingApp.View.Tourist.Pages
{
    public partial class UseVoucher : Page
    {
        private UseVoucherViewModel viewModel;
        public UseVoucher(UseVoucherViewModel useVoucherViewModel)
        {
            InitializeComponent();
            this.DataContext = useVoucherViewModel;
            viewModel = useVoucherViewModel;
        }

        private void ReservationClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(viewModel.Reservation());
        }
    }
}
