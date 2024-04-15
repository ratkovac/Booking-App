using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BookingApp.View.ViewModel.Owner;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using BookingApp.View.ViewModel.Owner;
using BookingApp.View.ViewModel.Guest;

namespace BookingApp.View.Owner
{
    public partial class AccommodationsGrades : Window
    {
        public AccommodationsGrades(AccommodationsGradesViewModel accommodationsGradesViewModel)
        {
            InitializeComponent();
            this.DataContext = accommodationsGradesViewModel;
            AccommodationGradesGrid.ItemsSource = accommodationsGradesViewModel.allGrades();
            Avarage.Content = accommodationsGradesViewModel.AvarageGrade();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
