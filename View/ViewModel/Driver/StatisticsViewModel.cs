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

namespace BookingApp.ViewModel.Driver
{
    public class StatisticsViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
            _successfulDrivesRepository = new SuccessfulDrivesRepository();
            _drivesDrivenRepository = new DrivesDrivenRepository();
            _driverStatsRepository = new DriverStatsRepository();
            ComboBoxItems = _successfulDrivesRepository.GetYears();
            Year = "2024";

            InitializeCharts();
        }

        private void InitializeCharts()
        {

            DriverStats driverStats = _driverStatsRepository.GetByDriverId(LoggedDriver.Id);
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
                Values = avgPrices
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
                Values = avgDurations
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
                Values = Counts
            };

            SeriesCollection3.Add(seriesLine3);
        }

        private double FindAvgPrice(int month)
        {
            ObservableCollection<int> idsPerMonth = _successfulDrivesRepository.GetDriveIdsByMonthAndYear(month, int.Parse(Year), LoggedDriver.Id);
            return _drivesDrivenRepository.CalculateAveragePriceForDrives(idsPerMonth);
        }

        private double FindAvgDuration(int month)
        {
            ObservableCollection<int> idsPerMonth = _successfulDrivesRepository.GetDriveIdsByMonthAndYear(month, int.Parse(Year), LoggedDriver.Id);
            return _drivesDrivenRepository.CalculateAverageDurationForDrives(idsPerMonth);
        }

        private int FindNumberOfDrivesForMonth(int month)
        {
            return _successfulDrivesRepository.GetNumberOfDrivesByMonthAndYear(month, int.Parse(Year), LoggedDriver.Id);
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
