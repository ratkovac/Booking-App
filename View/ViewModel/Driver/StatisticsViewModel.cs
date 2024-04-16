using LiveCharts;
using System;
using System.Collections.ObjectModel;
using BookingApp.Repository;
using BookingApp.ViewModel;
using LiveCharts.Wpf;
using BookingApp.Model;
using System.Windows;
using BookingApp.View.Driver;

namespace BookingApp.ViewModel.Driver
{
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly SuccessfulDrivesRepository _successfulDrivesRepository;
        private readonly DrivesDrivenRepository _drivesDrivenRepository;
        private readonly DriverStatsRepository _driverStatsRepository;

        public SeriesCollection SeriesCollection1 { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection3 { get; set; }

        public User LoggedDriver {  get; set; }
        public int FastDrivesValue { get; set; }
        public int BonusPointsValue { get; set; }
        public int CancelledDrivesValue { get; set; }

        // Dodatni propertiji za maksimalne vrednosti ProgressBar-ova
        public int MaxFastDrivesValue { get; set; } = 15;
        public int MaxBonusPointsValue { get; set; } = 50;
        public int MaxCancelledDrivesValue { get; set; } = 15;
        public event EventHandler NazadClicked;

        public StatisticsViewModel(User driver)
        {
            LoggedDriver = driver;
            _successfulDrivesRepository = new SuccessfulDrivesRepository();
            _drivesDrivenRepository = new DrivesDrivenRepository();
            _driverStatsRepository = new DriverStatsRepository();   

            SeriesCollection1 = new SeriesCollection();
            SeriesCollection2 = new SeriesCollection();
            SeriesCollection3 = new SeriesCollection();

            InitializeCharts();
        }

        private void InitializeCharts()
        {
            DriverStats driverStats = _driverStatsRepository.GetByDriverId(LoggedDriver.Id);
            FastDrivesValue = driverStats.FastDrives;
            BonusPointsValue = driverStats.BonusPoints;
            CancelledDrivesValue = driverStats.CancelledDrives;
            // Kreiranje podataka za grafikon 1
            ChartValues<double> avgPrices = new ChartValues<double>();
            for (int i = 1; i <= 12; i++)
            {
                var avgPrice = FindAvgPrice(i);
                avgPrices.Add(avgPrice);
            }

            LineSeries seriesLine1 = new LineSeries
            {
                Title = "Average Price",
                Values = avgPrices
            };

            SeriesCollection1.Add(seriesLine1);

            // Kreiranje podataka za grafikon 2
            ChartValues<double> avgDurations = new ChartValues<double>();
            for (int i = 1; i <= 12; i++)
            {
                var avgLength = FindAvgDuration(i);
                avgDurations.Add(avgLength);
            }

            LineSeries seriesLine2 = new LineSeries
            {
                Title = "Average Duration",
                Values = avgDurations
            };

            SeriesCollection2.Add(seriesLine2);

            // Kreiranje podataka za grafikon 3
            ChartValues<double> Counts = new ChartValues<double>();
            for (int i = 1; i <= 12; i++)
            {
                var Count = FindNumberOfDrivesForMonth(i);
                Counts.Add(Count);
            }

            LineSeries seriesLine3 = new LineSeries
            {
                Title = "Number of Drives",
                Values = Counts
            };

            SeriesCollection3.Add(seriesLine3);
        }

        private double FindAvgPrice(int month)
        {
            ObservableCollection<int> idsPerMonth = _successfulDrivesRepository.GetDriveIdsByMonthAndYear(month, 2024, LoggedDriver.Id);
            return _drivesDrivenRepository.CalculateAveragePriceForDrives(idsPerMonth);
        }

        private double FindAvgDuration(int month)
        {
            ObservableCollection<int> idsPerMonth = _successfulDrivesRepository.GetDriveIdsByMonthAndYear(month, 2024, LoggedDriver.Id);
            return _drivesDrivenRepository.CalculateAverageDurationForDrives(idsPerMonth);
        }

        private int FindNumberOfDrivesForMonth(int month)
        {
            return _successfulDrivesRepository.GetNumberOfDrivesByMonthAndYear(month, 2024, LoggedDriver.Id);
        }

        internal void Back_Click()
        {
            DriverFrontPage driverFrontPage = new DriverFrontPage(LoggedDriver);
            driverFrontPage.Show();
        }
    }
}
