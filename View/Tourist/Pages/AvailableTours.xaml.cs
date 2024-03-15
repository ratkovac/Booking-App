﻿using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace BookingApp.View.Tourist.Pages
{
    /// <summary>
    /// Interaction logic for AvailableTours.xaml
    /// </summary>
    public partial class AvailableTours : Page, INotifyPropertyChanged
    {

        private TourRepository tourRepository;
        public ObservableCollection<Tour> ListTour { get; set; }

        private int _maxNumberGuest;
        public int MaxNumberGuests
        {
            get => _maxNumberGuest;
            set
            {
                if (value != _maxNumberGuest)
                {
                    _maxNumberGuest = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AvailableTours()
        {
            InitializeComponent();
            DataContext = this;

            tourRepository = new TourRepository();
            ListTour = new ObservableCollection<Tour>(tourRepository.GetAll());
        }

        private void UpdateTourList()
        {
            ListTour.Clear();
            foreach (var tour in tourRepository.GetAll())
            {
                ListTour.Add(tour);
            }
        }

        public void Update()
        {
            UpdateTourList();
        }
    }
}