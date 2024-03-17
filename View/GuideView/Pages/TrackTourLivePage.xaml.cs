using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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

        private TourRepository _tourRepository;
        private CheckPointRepository _checkPointRepository;

        public TrackTourLivePage()
        {
            InitializeComponent();
            DataContext = this; // Postavljanje DataContext na instancu TrackTourLivePage
            _tourRepository = new TourRepository();
            _checkPointRepository = new CheckPointRepository();
           // Tours = new ObservableCollection<Tour>(_tourRepository.GetAll());
            Tours = new ObservableCollection<Tour>(_tourRepository.GetToursForToday());
        }
        

        private void LoadCheckPoints()
        {
            if (SelectedTour != null)
            {
                CheckPoints = new ObservableCollection<CheckPoint>(_checkPointRepository.GetCheckPoints(SelectedTour.Id));
            }
            else
            {
                CheckPoints = null;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
