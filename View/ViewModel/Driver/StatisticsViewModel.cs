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
using System.Windows.Input;

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
        private int _fastDrivesValue;
        public int FastDrivesValue
        {
            get { return _fastDrivesValue; }
            set
            {
                if (_fastDrivesValue != value)
                {
                    _fastDrivesValue = value;
                    OnPropertyChanged(nameof(FastDrivesValue));
                    SuperDriverVisibility = value == 15 ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private int _bonusPointsValue;
        public int BonusPointsValue
        {
            get { return _bonusPointsValue; }
            set
            {
                if (_bonusPointsValue != value)
                {
                    _bonusPointsValue = value;
                    OnPropertyChanged(nameof(BonusPointsValue));
                }
            }
        }

        private int _cancelledDrivesValue;
        public int CancelledDrivesValue
        {
            get { return _cancelledDrivesValue; }
            set
            {
                if (_cancelledDrivesValue != value)
                {
                    _cancelledDrivesValue = value;
                    OnPropertyChanged(nameof(CancelledDrivesValue));
                }
            }
        }
        private Visibility _superDriverVisibility;
        public Visibility SuperDriverVisibility
        {
            get { return _superDriverVisibility; }
            set
            {
                if (_superDriverVisibility != value)
                {
                    _superDriverVisibility = value;
                    OnPropertyChanged(nameof(SuperDriverVisibility));
                }
            }
        }
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
        private int _numberOfDrives;
        public int NumberOfDrives
        {
            get { return _numberOfDrives; }
            set
            {
                if (_numberOfDrives != value)
                {
                    _numberOfDrives = value;
                    OnPropertyChanged(nameof(NumberOfDrives));
                }
            }
        }

        private double _averagePrice;
        public double AveragePrice
        {
            get { return _averagePrice; }
            set
            {
                if (_averagePrice != value)
                {
                    _averagePrice = value;
                    OnPropertyChanged(nameof(AveragePrice));
                }
            }
        }

        private double _averageDurationSeconds;
        public double AverageDurationSeconds
        {
            get { return _averageDurationSeconds; }
            set
            {
                if (_averageDurationSeconds != value)
                {
                    _averageDurationSeconds = value;
                    OnPropertyChanged(nameof(AverageDurationSeconds));
                    OnPropertyChanged(nameof(AverageDuration));
                }
            }
        }
        public TimeSpan AverageDuration
        {
            get { return TimeSpan.FromSeconds(AverageDurationSeconds); }
        }

        private string _currentYear;
        public string CurrentYear
        {
            get { return _currentYear; }
            set
            {
                if (_currentYear != value)
                {
                    _currentYear = value;
                    OnPropertyChanged(nameof(CurrentYear));
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
            driverStatsService.RefreshStats();
            ComboBoxItems = new ObservableCollection<string>(driverStatsService.GetYears());
            Year = "2024";
            SuperDriverVisibility = FastDrivesValue == 15 ? Visibility.Visible : Visibility.Collapsed;
            int ThisYear = DateTime.Now.Year;
            CurrentYear = ThisYear.ToString();
            AverageDurationSeconds = driverStatsService.GetAverageDurationInYear(ThisYear, LoggedDriver.Id);
            AveragePrice = driverStatsService.GetAveragePriceInYear(ThisYear, LoggedDriver.Id);
            NumberOfDrives = driverStatsService.GetNumberOfDrivesInYear(ThisYear, LoggedDriver.Id);

            InitializeCharts();
        }

        private void InitializeCharts()
        {
            DriverStats driverStats = new DriverStats();
            driverStats = driverStatsService.GetStatsByDriverId(LoggedDriver.Id);
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
