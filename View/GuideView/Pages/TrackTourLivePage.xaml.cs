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
using System.Windows.Navigation;

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
            DataContext = this; 
            _tourRepository = new TourRepository();
            _checkPointRepository = new CheckPointRepository();
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

        private int _selectedCheckpointIndex = -1;


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var checkpoint = checkbox.DataContext as CheckPoint;
            var currentIndex = CheckPoints.IndexOf(checkpoint);

            if (currentIndex == _selectedCheckpointIndex + 1)
            {
                _selectedCheckpointIndex = currentIndex;

                if (_selectedCheckpointIndex == CheckPoints.Count - 1)
                {
                    EndTour();
                }
            }
            else
            {
                checkbox.IsChecked = false;
                MessageBox.Show("Morate selektovati checkpointove redom.");
            }
        }

        private void EndTour()
        {
            MessageBox.Show("Tura je zavrsena.", "Kraj ture", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();

        }



        private void ButtonEndTour_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da iznenada zavrsite turu?", "Potvrda zavrsetka ture", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Tura je iznenadno zavrsena.", "Kraj ture", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
            }
            else
            {
                MessageBox.Show("Molimo izaberite turu pre nego sto iznenadno zavrsite ture.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
