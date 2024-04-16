﻿using BookingApp.Model;
using BookingApp.Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class AvailableTours : Page, INotifyPropertyChanged
    {

        private TourRepository tourRepository;
        public ObservableCollection<Tour> ListTour { get; set; }
        public Tour SelectedTour { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }

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

        public AvailableTours(BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            Tourist = tourist;

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

        private void Tour_Reservation(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null)
            {
                var tour_reservation = new TourReservation(SelectedTour, Tourist);
                NavigationService.Navigate(tour_reservation);
            }
            else
            {
                MessageBox.Show("Morate selektovati turu da bi ste vidjeli vise detalja o njoj!");
            }
        }

        public void Update()
        {
            UpdateTourList();
        }
    }
}