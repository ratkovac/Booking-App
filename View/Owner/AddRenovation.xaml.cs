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
        public AccommodationDTO SelectedAccommodation { get; private set; }

        public RenovationDatesDTO SelectedDate { get; set; }

        public AddRenovation(AddRenovationViewModel addRenovationViewModel, AccommodationDTO selectedAccommodationDTO)
        {
            InitializeComponent();
            this.DataContext = addRenovationViewModel;
            //this.addRenovationViewModel = addRenovationViewModel;
            SelectedAccommodation = selectedAccommodationDTO;
        }
    }
}
