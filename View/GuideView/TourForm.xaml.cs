using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;


namespace BookingApp.View.GuideView
{
    /// <summary>
    /// Interaction logic for TourForm.xaml
    /// </summary>
    public partial class TourForm : Window
    {
        public TourForm()
        {
            InitializeComponent();
        }

        private void CreateTourItem_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("View/GuideView/Pages/CreateTourPage.xaml", UriKind.RelativeOrAbsolute));
        }


        private void TrackTourLive_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("View/GuideView/Pages/TrackTourLivePage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
