using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using BookingApp.Repository;
using System.Collections.ObjectModel;
using BookingApp.ViewModel.Driver;
using BookingApp.Model;

namespace BookingApp.View.Driver
{
    /// <summary>
    /// Interaction logic for Example.xaml
    /// </summary>
    public partial class Example : Window
    {
        public User LoggedDriver {  get; set; }
        StatisticsViewModel viewModel { get; set; }
        public Example(User driver)
        {
            InitializeComponent();
            LoggedDriver = driver;
            viewModel = new StatisticsViewModel(LoggedDriver);
            DataContext = viewModel;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Back_Click();
            Window.GetWindow(this)?.Close();
        }
    }
}
       