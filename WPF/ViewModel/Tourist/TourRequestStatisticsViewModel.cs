using BookingApp.Model;
using BookingApp.Service;
using BookingApp.Service.Factories;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Wpf;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourRequestStatisticsViewModel : INotifyPropertyChanged
    {
        public SeriesCollection PieChartData { get; set; }
        private readonly TourRequestStatisticsFactory _statFactory;
        private readonly int TouristId;
        public BookingApp.Model.Tourist Tourist { get; set; }
        public TouristService _touristService { get; set; }
        public TourRequestSegmentService _tourRequestSegmentService { get; set; }
        public ICommand GeneratePDFCommand { get; set; }

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
            _touristService = new TouristService();
            _tourRequestSegmentService = tourRequestSegmentService;
            Tourist = _touristService.GetById(touristId);
            GeneratePDFCommand = new RelayCommand<object>(ExecuteGeneratePDFCommand);
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

            AcceptedPercentage = Math.Round(acceptedPercentage, 0);
            DeclinedPercentage = Math.Round(declinedPercentage, 0);

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

        private void ExecuteGeneratePDFCommand(object sender)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "TourRequestReport.pdf",
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };

            bool? result = saveDialog.ShowDialog();

            if (result == true)
            {
                string filePath = saveDialog.FileName;

                XImage logo = XImage.FromFile("C:\\Users\\HP\\OneDrive\\Radna površina\\projekat\\sims-ra-2024-group-7-team-a\\Resources\\Images\\logo.jpg");

                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Verdana", 18, XFontStyle.Bold);
                XFont subtitleFont = new XFont("Verdana", 14, XFontStyle.Bold);
                XFont regularFont = new XFont("Verdana", 10, XFontStyle.Regular);
                XFont smallFont = new XFont("Verdana", 10, XFontStyle.Regular);

                double maxWidth = 120;
                double maxHeight = 120;

                double originalWidth = 626;
                double originalHeight = 626;

                double aspectRatio = originalWidth / originalHeight;

                double logoWidth, logoHeight;

                if (originalWidth > originalHeight)
                {
                    logoWidth = maxWidth;
                    logoHeight = maxWidth / aspectRatio;
                }
                else
                {
                    logoHeight = maxHeight;
                    logoWidth = maxHeight * aspectRatio;
                }
                double logoX = 20;
                double logoY = 20;
                gfx.DrawImage(logo, logoX, logoY, logoWidth, logoHeight);

                string authorName = $"Username: {Tourist.User.Username}";
                string currentDate = $"Date created: {DateTime.Now.ToString("dd.MM.yyyy")}";
                double authorNameWidth = gfx.MeasureString(authorName, smallFont).Width;
                double currentDateWidth = gfx.MeasureString(currentDate, smallFont).Width;
                double authorX = page.Width - authorNameWidth - 60;
                double authorY = logoY + 60;
                double dateX = authorX;
                double dateY = authorY + smallFont.Height + 5;
                gfx.DrawString(authorName, smallFont, XBrushes.Black, authorX, authorY);
                gfx.DrawString(currentDate, smallFont, XBrushes.Black, dateX, dateY);

                // Center the title
                gfx.DrawString("Tour Request Report", titleFont, XBrushes.Black, new XRect(0, 0, page.Width, 40), XStringFormats.Center);

                double yPoint = logoY + logoHeight + 20;
                double margin = 40;

                foreach (var year in Years)
                {
                    if (!int.TryParse(year, out int parsedYear))
                    {
                        continue;
                    }

                    if (yPoint > page.Height - margin)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        yPoint = margin;
                    }

                    gfx.DrawString($"Year: {year}", subtitleFont, XBrushes.Black, new XRect(margin, yPoint, page.Width - margin * 2, page.Height), XStringFormats.TopLeft);
                    yPoint += 25;

                    TourRequestStatistics requestStatistics = _statFactory.CreateTouristStatForYear(parsedYear, TouristId);
                    int totalTours = requestStatistics.AcceptedTours + requestStatistics.DeclinedTours;
                    double acceptedPercentage = (double)requestStatistics.AcceptedTours / totalTours * 100;
                    double declinedPercentage = (double)requestStatistics.DeclinedTours / totalTours * 100;
                    double averageNumberOfPeople = _tourRequestSegmentService.GetAverageNumberOfPeopleForYear(parsedYear);
                    string mostFrequentLanguage = _tourRequestSegmentService.GetMostFrequentLanguageForYear(parsedYear);
                    string mostFrequentCountry = _tourRequestSegmentService.GetMostFrequentCountryForYear(parsedYear);
                    string mostFrequentCity = _tourRequestSegmentService.GetMostFrequentCityForYear(parsedYear);

                    acceptedPercentage = Math.Round(acceptedPercentage, 0);
                    declinedPercentage = Math.Round(declinedPercentage, 0);

                    gfx.DrawString($"  Number of Tours: {totalTours}", regularFont, XBrushes.Black, new XRect(margin + 40, yPoint, page.Width - margin * 2, page.Height), XStringFormats.TopLeft);
                    yPoint += 20;
                    gfx.DrawString($"  Percentage of accepted tours: {acceptedPercentage}%", regularFont, XBrushes.Black, new XRect(margin + 40, yPoint, page.Width - margin * 2, page.Height), XStringFormats.TopLeft);
                    yPoint += 20;
                    gfx.DrawString($"  Percentage of cancelled tours: {declinedPercentage}%", regularFont, XBrushes.Black, new XRect(margin + 40, yPoint, page.Width - margin * 2, page.Height), XStringFormats.TopLeft);
                    yPoint += 20;
                    gfx.DrawString($"  Average number of people: {averageNumberOfPeople}", regularFont, XBrushes.Black, new XRect(margin + 40, yPoint, page.Width - margin * 2, page.Height), XStringFormats.TopLeft);
                    yPoint += 20;
                    gfx.DrawString($"  Most frequent language: {mostFrequentLanguage}", regularFont, XBrushes.Black, new XRect(margin + 40, yPoint, page.Width - margin * 2, page.Height), XStringFormats.TopLeft);
                    yPoint += 20;
                    gfx.DrawString($"  Most frequent location: {mostFrequentCountry}, {mostFrequentCity}", regularFont, XBrushes.Black, new XRect(margin + 40, yPoint, page.Width - margin * 2, page.Height), XStringFormats.TopLeft);
                    yPoint += 25;

                    gfx.DrawLine(XPens.Gray, margin, yPoint, page.Width - margin, yPoint);
                    yPoint += 15;

                    /*for (int month = 1; month <= 12; month++)
                    {
                        if (yPoint > page.Height - margin)
                        {
                            page = document.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            yPoint = margin;
                        }
                        
                        string monthName = new DateTime(1, month, 1).ToString("MMMM", CultureInfo.InvariantCulture);

                        gfx.DrawString($"Month: {monthName}", regularFont, XBrushes.Black, new XRect(margin + 20, yPoint, page.Width - margin * 2, page.Height), XStringFormats.TopLeft);
                        yPoint += 15;
                    }*/
                }

                document.Save(filePath);

                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
        }
    }
}
