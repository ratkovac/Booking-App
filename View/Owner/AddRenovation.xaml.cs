using BookingApp.DTO;
using BookingApp.Service;
using BookingApp.View.ViewModel.Owner;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace BookingApp.View.Owner
{
    public partial class AddRenovation : Window
    {
        private AddRenovationViewModel addRenovationViewModel;
        private Border SelectedBorder { get; set; }
        public AccommodationDTO SelectedAccommodation { get; private set; }

        public RenovationDatesDTO SelectedDate { get; set; }

        public AddRenovation(AddRenovationViewModel addRenovationViewModel, AccommodationDTO selectedAccommodationDTO)
        {
            InitializeComponent();
            this.DataContext = addRenovationViewModel;
            this.addRenovationViewModel = addRenovationViewModel;
            SelectedAccommodation = selectedAccommodationDTO;

        }
        


        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = StartDatePicker.SelectedDate.Value;
            DateTime end = EndDatePicker.SelectedDate.Value;
            int numberOfDays = Convert.ToInt32(Days.Text);
            List<(DateTime, DateTime)> reservations = addRenovationViewModel.GetAllReservations(SelectedAccommodation.Id);
            List<(DateTime, DateTime)> renovations = addRenovationViewModel.GetAllPossibleDates(start, end, numberOfDays);
            List<(DateTime, DateTime)> possibleDates = addRenovationViewModel.GetNonOverlappingRenovationDates(renovations, reservations);
        }*/
    }
}
