using LiveCharts;
using System;
using System.Collections.ObjectModel;
using BookingApp.Repository;
using BookingApp.ViewModel;
using LiveCharts.Wpf;
using BookingApp.Model;
using System.Windows;
using BookingApp.View.Driver;
using System.ComponentModel;
using System.Windows.Media;
using BookingApp.Service;
using System.Collections.Generic;

namespace BookingApp.ViewModel.Driver
{
    public class StatisticsViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DriverStatsService driverStatsService;

        public SeriesCollection SeriesCollection1 { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection3 { get; set; }

        public User LoggedDriver {  get; set; }
        public int FastDrivesValue { get; set; }
        public int BonusPointsValue { get; set; }
        public int CancelledDrivesValue { get; set; }

        public int MaxFastDrivesValue { get; set; } = 15;
        public int MaxBonusPointsValue { get; set; } = 50;
        public int MaxCancelledDrivesValue { get; set; } = 15;
        public event EventHandler NazadClicked;

        private ObservableCollection<string> comboBoxItems;
        public ObservableCollection<string> ComboBoxItems
        {
            get { return comboBoxItems; }
            set
            {
                comboBoxItems = value;
                OnPropertyChanged("ComboBoxItems");
            }
        }
        private string _year;
        public string Year
        {
            get { return _year; }
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged(nameof(Year));
                    SeriesCollection1.Clear();
                    SeriesCollection2.Clear();
                    SeriesCollection3.Clear();
                    InitializeCharts();
                }
            }
        }

        public StatisticsViewModel(User driver)
        {
            SeriesCollection1 = new SeriesCollection();
            SeriesCollection2 = new SeriesCollection();
            SeriesCollection3 = new SeriesCollection();
            LoggedDriver = driver;
            driverStatsService = new DriverStatsService();
            ComboBoxItems = new ObservableCollection<string>(driverStatsService.GetYears());
            Year = "2024";

            InitializeCharts();
        }

        private void InitializeCharts()
        {

            DriverStats driverStats = driverStatsService.GetStatsByDriverId(LoggedDriver.Id);
            FastDrivesValue = driverStats.FastDrives;
            BonusPointsValue = driverStats.BonusPoints;
            CancelledDrivesValue = driverStats.CancelledDrives;
            ChartValues<double> avgPrices = new ChartValues<double>();
            for (int i = 1; i <= 12; i++)
            {
                var avgPrice = FindAvgPrice(i);
                avgPrices.Add(avgPrice);
            }

            LineSeries seriesLine1 = new LineSeries
            {
                Title = "Average Price",
                Values = avgPrices,
                Stroke = Brushes.Black,
                PointForeground = Brushes.Black,
                StrokeThickness = 2
            };

            SeriesCollection1.Add(seriesLine1);

            ChartValues<double> avgDurations = new ChartValues<double>();
            for (int i = 1; i <= 12; i++)
            {
                var avgLength = FindAvgDuration(i);
                avgDurations.Add(avgLength);
            }

            LineSeries seriesLine2 = new LineSeries
            {
                Title = "Average Duration",
                Values = avgDurations,
                Stroke = Brushes.Black,
                PointForeground = Brushes.Black,
                StrokeThickness = 2
            };

            SeriesCollection2.Add(seriesLine2);

            ChartValues<double> Counts = new ChartValues<double>();
            for (int i = 1; i <= 12; i++)
            {
                var Count = FindNumberOfDrivesForMonth(i);
                Counts.Add(Count);
            }

            LineSeries seriesLine3 = new LineSeries
            {
                Title = "Number of Drives",
                Values = Counts,
                Stroke = Brushes.Black,
                PointForeground = Brushes.Black,
                StrokeThickness = 2
            };

            SeriesCollection3.Add(seriesLine3);
        }

        private double FindAvgPrice(int month)
        {
            List<int> idsPerMonth = new List<int>(driverStatsService.GetDrivesByMonthAndYear(month, int.Parse(Year), LoggedDriver.Id));
            return driverStatsService.GetAveragePriceForDrives(idsPerMonth);
        }

        private double FindAvgDuration(int month)
        {
            List<int> idsPerMonth = new List<int>(driverStatsService.GetDrivesByMonthAndYear(month, int.Parse(Year), LoggedDriver.Id));
            return driverStatsService.GetAverageDuration(idsPerMonth);
        }

        private int FindNumberOfDrivesForMonth(int month)
        {
            return driverStatsService.GetNumberOfDrives(month, int.Parse(Year), LoggedDriver.Id);
        }

        internal void Back_Click()
        {
            DriverFrontPage driverFrontPage = new DriverFrontPage(LoggedDriver);
            driverFrontPage.Show();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
