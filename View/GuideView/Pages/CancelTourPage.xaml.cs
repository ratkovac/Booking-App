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
using BookingApp.Model;
using BookingApp.View.ViewModel;
using BookingApp.View.ViewModel.Guide;

namespace BookingApp.View.GuideView.Pages
{
    /// <summary>
    /// Interaction logic for CancelTourPage.xaml
    /// </summary>
    public partial class CancelTourPage : Page
    {

        private CancelTourViewModel viewModel;



        public CancelTourPage(User user)
        {
            viewModel = new CancelTourViewModel(user);
            InitializeComponent();
            DataContext = viewModel;
        }



        private void btnCancelTour_Click(object sender, RoutedEventArgs e)
        {
            var selectedInstance = (TourInstance)ListBoxTourInstances.SelectedItem;
            var result = MessageBox.Show("Da li želite da otkažete turu sa ID: " + selectedInstance.Id.ToString() + "?", "Potvrda", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                if (selectedInstance != null)
                {
                    viewModel.CancelTour(selectedInstance);
                }
            }
            MessageBox.Show("Uspesno otkazana tura sa ID:" + selectedInstance.Id.ToString());

        }

        private void btnCancelAllTours_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
