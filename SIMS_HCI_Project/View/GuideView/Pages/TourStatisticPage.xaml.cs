using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using BookingApp.View.ViewModel;
using BookingApp.View.ViewModel.Guide;

namespace BookingApp.View.GuideView.Pages
{
    /// <summary>
    /// Interaction logic for TourStatisticPage.xaml
    /// </summary>
    public partial class TourStatisticPage : Page
    {
        private TourStatisticViewModel viewModel;


        public TourStatisticPage(User user)
        {
            viewModel = new TourStatisticViewModel(user);
            InitializeComponent();
            DataContext = viewModel;

  

        }

        private void cbOverall_Checked(object sender, RoutedEventArgs e)
        {
            if (cbSpecificYear != null)
            {
                cbSpecificYear.IsChecked = false;
                UpdateTourStatistic();
                txtYear.Text = "";
            }
        }


        private void cbSpecificYear_Checked(object sender, RoutedEventArgs e)
        {
            if (txtYear.Text == "")
            {
                MessageBox.Show("Please enter a valid year.");
                cbSpecificYear.IsChecked = false;
                return;
            }
            if (cbOverall != null)
            {
                cbOverall.IsChecked = false;
                UpdateTourStatistic();
            }
        }

        private void txtYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTourStatistic();
        }

        private void UpdateTourStatistic()
        {
            if (cbOverall.IsChecked == true)
            {
                viewModel.MostVistedTour();
            }
            else if (cbSpecificYear.IsChecked == true)
            {
                if (int.TryParse(txtYear.Text, out int year))
                {
                    viewModel.MostVistedTourByYear(year);
                }
                else
                {
                    MessageBox.Show("Please enter a valid year.");
                }
            }
        }
        private void cbTour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.GetAgeStatistics(viewModel.SelectedTourInstance.Id);
        }

        
    }
}
