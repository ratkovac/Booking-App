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
    /// Interaction logic for FilterAndSort.xaml
    /// </summary>
    public partial class FilterAndSort : Page
    {
        private FilterAndSortViewModel FilterAndSortViewModel { get; set; }
        public FilterAndSort(FilterAndSortViewModel filterAndSortViewModel)
        {
            InitializeComponent();
            DataContext = filterAndSortViewModel;
            FilterAndSortViewModel = filterAndSortViewModel;
        }

        private void OnClick_Back(object sender, RoutedEventArgs e)
        {
            FilterAndSortViewModel.OnClick_Back();
        }

        private void OnClick_Confirm(object sender, RoutedEventArgs e)
        {
            FilterAndSortViewModel.OnClick_Confirm();
        }

    }
}
