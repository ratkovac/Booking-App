using BookingApp.Model;
using BookingApp.Service;
using BookingApp.Service.Factories;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourRequestStatisticsViewModel : INotifyPropertyChanged
    {
        public SeriesCollection PieChartData { get; set; }
        private readonly TourRequestStatisticsFactory _statFactory;
        private readonly int TouristId;

        private ObservableCollection<string> _years = new ObservableCollection<string>();
        public ObservableCollection<string> Years
        {
            get { return _years; }
            set
            {
                if (_years != value)
                {
                    _years = value;
                    OnPropertyChanged(nameof(Years));
                }
            }
        }

        private string _selectedYear;
        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    StatisticsForChosenYear(value);
                    OnPropertyChanged();
                }
            }
        }

        private double _acceptedPercentage;
        public double AcceptedPercentage
        {
            get { return _acceptedPercentage; }
            set
            {
                _acceptedPercentage = value;
                OnPropertyChanged(nameof(AcceptedPercentage));
            }
        }

        private double _declinedPercentage;
        public double DeclinedPercentage
        {
            get { return _declinedPercentage; }
            set
            {
                _declinedPercentage = value;
                OnPropertyChanged(nameof(DeclinedPercentage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourRequestStatisticsViewModel(int touristId, TourRequestService tourRequestService, TourRequestSegmentService tourRequestSegmentService)
        {
            _statFactory = new TourRequestStatisticsFactory(tourRequestService, tourRequestSegmentService);
            TouristId = touristId;
            GenerateYears();
            PieChartData = new SeriesCollection();
            SelectedYear = "Every year";
        }

        private void StatisticsForChosenYear(string year)
        {
            TourRequestStatistics requestStatistics;
            if (year == "Every year")
            {
                requestStatistics = _statFactory.CreateTouristStatForEveryYear(TouristId);
            }
            else
            {
                requestStatistics = _statFactory.CreateTouristStatForYear(int.Parse(year), TouristId);
            }

            int totalTours = requestStatistics.AcceptedTours + requestStatistics.DeclinedTours;
            double acceptedPercentage = (double)requestStatistics.AcceptedTours / totalTours * 100;
            double declinedPercentage = (double)requestStatistics.DeclinedTours / totalTours * 100;

            AcceptedPercentage = Math.Round(acceptedPercentage, 2);
            DeclinedPercentage = Math.Round(declinedPercentage, 2);

            var chartData = new SeriesCollection
            {
                new PieSeries { Title = "Accepted", Values = new ChartValues<int> { requestStatistics.AcceptedTours }, DataLabels = true },
                new PieSeries { Title = "Rejected", Values = new ChartValues<int> { requestStatistics.DeclinedTours }, DataLabels = true }
            };

            PieChartData = chartData;
            OnPropertyChanged(nameof(PieChartData));
        }


        private void GenerateYears()
        {
            Years.Add("Every year");

            int currentYear = DateTime.Now.Year;
            int firstYear = _statFactory.GetEarliestYear(TouristId);

            Years.Clear();
            Years.Add("Every year");
            for (int year = firstYear; year <= currentYear; year++)
            {
                Years.Add(year.ToString());
            }
        }
    }
}
