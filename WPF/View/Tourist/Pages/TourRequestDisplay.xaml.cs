using BookingApp.Model;
using BookingApp.Service;
using BookingApp.WPF.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class TourRequestDisplay : Page
    {
        public TourRequestDisplayViewModel ViewModel { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }

        public TourRequestDisplay(TourRequestDisplayViewModel tourRequestDisplayViewModel)
        {
            InitializeComponent();
            this.DataContext = tourRequestDisplayViewModel;
            ViewModel = tourRequestDisplayViewModel;
        }

        private void TourRequestDescription_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            TourRequestSegment selectedSegment = (TourRequestSegment)button.DataContext;

            var tourRequestDescription = new TourRequestDescription(selectedSegment, Tourist);
            NavigationService.Navigate(tourRequestDescription);
        }
    }
}
