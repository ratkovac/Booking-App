﻿using BookingApp.WPF.ViewModel.Tourist;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class GradeTourPage : Page
    {
        private GradeTourViewModel _viewModel;

        public GradeTourPage(BookingApp.Model.TourReservation tourReservation, int touristId)
        {
            InitializeComponent();
            _viewModel = new GradeTourViewModel(tourReservation, touristId);
            DataContext = _viewModel;
        }

        private void AddPictureButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }

            if (!(button.DataContext is GradeTourFormViewModel gradeTourFormViewModel))
            {
                return;
            }

            _viewModel.AddPicture(gradeTourFormViewModel);
        }

        private void RemoveLastPictureButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }

            if (!(button.DataContext is GradeTourFormViewModel gradeTourFormViewModel))
            {
                return;
            }

            _viewModel.RemoveLastPicture(gradeTourFormViewModel);
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            while (parentObject != null)
            {
                if (parentObject is T parent)
                {
                    return parent;
                }

                parentObject = VisualTreeHelper.GetParent(parentObject);
            }

            return null;
        }


        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveReviews();
        }
    }
}