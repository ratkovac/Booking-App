using BookingApp.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.GuideView.Pages
{
    /// <summary>
    /// Interaction logic for TrackTourLivePage.xaml
    /// </summary>
    public partial class TrackTourLivePage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Tour> _tours;
        public ObservableCollection<Tour> Tours
        {
            get { return _tours; }
            set
            {
                _tours = value;
                OnPropertyChanged();
            }
        }

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                _selectedTour = value;
                OnPropertyChanged();
                LoadCheckPoints();
            }
        }

        private ObservableCollection<CheckPoint> _checkPoints;
        public ObservableCollection<CheckPoint> CheckPoints
        {
            get { return _checkPoints; }
            set
            {
                _checkPoints = value;
                OnPropertyChanged();
            }
        }

        public TrackTourLivePage()
        {
            InitializeComponent();

            // Load tours
            Tours = new ObservableCollection<Tour>(LoadTours());
        }

        private void LoadCheckPoints()
        {
            // Load check points for selected tour
            CheckPoints = new ObservableCollection<CheckPoint>(LoadCheckPoints(SelectedTour.Id));
        }

        private IEnumerable<Tour> LoadTours()
        {
            List<Tour> tours = new List<Tour>();
            string[] tourLines = File.ReadAllLines("../../../Resources/Data/tours.csv");
            foreach (string line in tourLines) 
            {
                string[] values = line.Split('|'); // Razdvajamo vrednosti koristeći '|'
                Tour tour = new Tour
                {
                    Id = Convert.ToInt32(values[0]),
                    Name = values[1],
                    Description = values[3], // Promenjen je indeks zbog izmene formata CSV datoteke
                    MaxGuests = Convert.ToInt32(values[5]), // Promenjen je indeks zbog izmene formata CSV datoteke
                    Duration = Convert.ToSingle(values[6]), // Promenjen je indeks zbog izmene formata CSV datoteke
                    LocationId = Convert.ToInt32(values[2]), // Promenjen je indeks zbog izmene formata CSV datoteke
                    Language = new Language(values[4]) // Promenjen je indeks zbog izmene formata CSV datoteke
                };
                tours.Add(tour);
            }
            return tours;
        }

        private IEnumerable<CheckPoint> LoadCheckPoints(int tourId)
        {
            List<CheckPoint> checkPoints = new List<CheckPoint>();
            string[] checkPointLines = File.ReadAllLines("../../../Resources/Data/checkpoints.csv");
            foreach (string line in checkPointLines) // Preskačemo prvu liniju jer je zaglavlje
            {
                string[] values = line.Split('|'); // Razdvajamo vrednosti koristeći '|'
                int checkpointTourId = Convert.ToInt32(values[2]); // Promenjen je indeks zbog izmene formata CSV datoteke
                if (checkpointTourId == tourId)
                {
                    CheckPoint checkPoint = new CheckPoint
                    {
                        Id = Convert.ToInt32(values[0]),
                        PointText = values[1],
                        TourId = checkpointTourId
                    };
                    checkPoints.Add(checkPoint);
                }
            }
            return checkPoints;
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
