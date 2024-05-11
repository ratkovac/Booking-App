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
    public partial class GroupDrivePage : Page
    {
        private GroupDriveViewModel viewModel;
        public GroupDrivePage(GroupDriveViewModel groupDriveViewModel)
        {
            InitializeComponent();
            this.DataContext = groupDriveViewModel;
            viewModel = groupDriveViewModel;
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Nema prethodne stranice!");
            }
        }

        private void Reservation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(viewModel.Reservation());
        }
    }
}
