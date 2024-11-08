﻿using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class ReservationsDisplay : Page
    {
        public ReservationsDisplay(ReservationsDisplayViewModel reservationsDisplayViewModel)
        {
            InitializeComponent();
            this.DataContext = reservationsDisplayViewModel;
        }

        private void TrackTour_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            BookingApp.Model.TourReservation selectedReservation = (BookingApp.Model.TourReservation)button.DataContext;

            if (selectedReservation.TourInstance.IsCompleted)
            {
                if (App.CurrentLanguage == "en-US")
                {
                    MessageBox.Show("The tour has already finished.");
                }
                else
                {
                    MessageBox.Show("Ova tura je već završena.");
                }
                return;
            }

            var trackTour = new TourTrackingView(selectedReservation);
            NavigationService.Navigate(trackTour);
        }

        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            BookingApp.Model.TourReservation selectedReservation = (BookingApp.Model.TourReservation)button.DataContext;

            if (!selectedReservation.TourInstance.IsCompleted)
            {
                if (App.CurrentLanguage == "en-US")
                {
                    MessageBox.Show("The tour has not yet finished.");
                }
                else
                {
                    MessageBox.Show("Ova tura još nije završena.");
                }
                return;
            }

            var gradeTourViewModel = new GradeTourViewModel(selectedReservation, selectedReservation.TouristId);
            NavigationService.Navigate(new GradeTourPage(gradeTourViewModel));
        }
    }
}
