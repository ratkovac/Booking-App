using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookingApp.Repository;
using System.Collections.ObjectModel;

namespace BookingApp.View.Driver
{
    /// <summary>
    /// Interaction logic for Example.xaml
    /// </summary>
    public partial class Example : Window
    {
        public SuccessfulDrivesRepository _successfulDrivesRepository { get; set; }

        public DrivesDrivenRepository _drivesDrivenRepository { get; set; }

        List<Model.DriveStats> driveStats = new List<Model.DriveStats>();

        public Example()
        {
            InitializeComponent();

            // Kreiranje podataka
            SeriesCollection series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Serija 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2, 7 }
                },
                new LineSeries
                {
                    Title = "Serija 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4, 6 }
                }
            };

            // Postavljanje podataka na grafikon
            MyChart.Series = series;

            // Postavljanje oznaka osi (opcionalno)
            MyChart.AxisX.Add(new Axis
            {
                Title = "X osa"
            });
            MyChart.AxisY.Add(new Axis
            {
                Title = "Y osa",
                LabelFormatter = value => value.ToString("N") // Formatiranje brojeva
            });

            // Ostale konfiguracije grafikona (opcionalno)
            MyChart.LegendLocation = LegendLocation.Right; // Postavljanje pozicije legende
        }
    }
}
