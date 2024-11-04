using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
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

        private int _lastSelectedCheckpointId = -1;

        private TourInstanceService tourInstanceService = new TourInstanceService();

        private ObservableCollection<TourInstance> _tourInstances;
        public ObservableCollection<TourInstance> TourInstances
        {
            get { return _tourInstances; }
            set
            {
                _tourInstances = value;
                OnPropertyChanged();
            }
        }


        private TourInstance _selectedTourInstance;
        public TourInstance SelectedTourInstance
        {
            get { return _selectedTourInstance; }
            set
            {
                _selectedTourInstance = value;
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

        private CheckPointRepository _checkPointRepository;
        private TourInstanceRepository _tourInstanceRepository;

        public TrackTourLivePage()
        {
            InitializeComponent();
            DataContext = this; 
            _checkPointRepository = new CheckPointRepository();
            _tourInstanceRepository = new TourInstanceRepository();
            TourInstances = new ObservableCollection<TourInstance>(_tourInstanceRepository.GetToursForToday());
 
        }
        
        private void LoadCheckPoints()
        {
            if (SelectedTourInstance != null)
            {
                CheckPoints = new ObservableCollection<CheckPoint>(_checkPointRepository.GetCheckPoints(SelectedTourInstance.TourId));
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
                _lastSelectedCheckpointId = checkpoint.Id;


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
            SelectedTourInstance.State = TourInstanceState.Finished;
            tourInstanceService.Update(SelectedTourInstance);
            NavigationService.GoBack();

        }



        private void ButtonEndTour_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTourInstance != null)
            {
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da iznenada zavrsite turu?", "Potvrda zavrsetka ture", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Tura je iznenadno zavrsena.", "Kraj ture", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelectedTourInstance.State = TourInstanceState.Finished;
                    tourInstanceService.Update(SelectedTourInstance);
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

        private void btnAddTourist_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTourInstance != null)
            {
                int tourId = SelectedTourInstance.Id;
                var touristListPage = new TouristListPage(tourId, _lastSelectedCheckpointId);
                var window = new Window
                {
                    Title = "Tourist List",
                    Content = touristListPage,
                    Width = 430, 
                    Height = 750, 
                    SizeToContent = SizeToContent.Manual
                };
                window.ShowDialog(); 
            }
            else
            {
                MessageBox.Show("Molimo izaberite turu pre nego što dodate turistu.", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
