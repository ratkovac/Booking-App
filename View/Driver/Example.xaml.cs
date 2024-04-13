using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using BookingApp.Repository;
using System.Collections.ObjectModel;

namespace BookingApp.View.Driver
{
    /// <summary>
    /// Interaction logic for Example.xaml
    /// </summary>
    public partial class Example : Window
    {
        private readonly SuccessfulDrivesRepository _successfulDrivesRepository;
        private readonly DrivesDrivenRepository _drivesDrivenRepository;

        public Example()
        {
            InitializeComponent();

            // Inicijalizacija repozitorijuma
            _successfulDrivesRepository = new SuccessfulDrivesRepository();
            _drivesDrivenRepository = new DrivesDrivenRepository();

            // Kreiranje podataka
            SeriesCollection series1 = new SeriesCollection();
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

            series1.Add(seriesLine1);

            // Postavljanje podataka na grafikon
            MyChart1.Series = series1;

            // Postavljanje oznaka osi X i Y
            MyChart1.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Avg", "Sep", "Okt", "Nov", "Dec" }
            });
            MyChart1.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                LabelFormatter = value => value.ToString("N") // Formatiranje brojeva
            });


            
            SeriesCollection series2 = new SeriesCollection();
            ChartValues<double> avgDurations = new ChartValues<double>();

            for (int i = 1; i <= 12; i++)
            {
                var avgLength = FindAvgDuration(i);
                avgDurations.Add(avgLength);
            }

            LineSeries seriesLine2 = new LineSeries
            {
                Title = "Average Price",
                Values = avgDurations
            };

            series2.Add(seriesLine2);

            // Postavljanje podataka na grafikon
            MyChart2.Series = series2;

            // Postavljanje oznaka osi X i Y
            MyChart2.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Avg", "Sep", "Okt", "Nov", "Dec" }
            });
            MyChart2.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                LabelFormatter = value =>
                {
                    TimeSpan time = TimeSpan.FromSeconds(value);
                    return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
                }
            });

            SeriesCollection series3 = new SeriesCollection();
            ChartValues<double> Counts = new ChartValues<double>();

            for (int i = 1; i <= 12; i++)
            {
                var Count = FindNumberOfDrivesForMonth(i);
                Counts.Add(Count);
            }

            LineSeries seriesLine3 = new LineSeries
            {
                Title = "Average Price",
                Values = Counts
            };

            series3.Add(seriesLine3);

            // Postavljanje podataka na grafikon
            MyChart3.Series = series3;

            // Postavljanje oznaka osi X i Y
            MyChart3.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "Maj", "Jun", "Jul", "Avg", "Sep", "Okt", "Nov", "Dec" }
            });
            MyChart3.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                LabelFormatter = value => value.ToString("N") // Formatiranje brojeva
            });
        }

        private double FindAvgPrice(int month)
        {
            ObservableCollection<int> idsPerMonth = _successfulDrivesRepository.GetDriveIdsByMonthAndYear(month, 2024);
            return _drivesDrivenRepository.CalculateAveragePriceForDrives(idsPerMonth);
        }

        private double FindAvgDuration(int month)
        {
            ObservableCollection<int> idsPerMonth = _successfulDrivesRepository.GetDriveIdsByMonthAndYear(month, 2024);
            return _drivesDrivenRepository.CalculateAverageDurationForDrives(idsPerMonth);
        }

        private int FindNumberOfDrivesForMonth(int month)
        {
            return _successfulDrivesRepository.GetNumberOfDrivesByMonthAndYear(month, 2024);
        }
    }
}
