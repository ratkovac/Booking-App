using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using BookingApp.WPF.ViewModel.Tourist;
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
    public partial class GradeTourView : Page
    {
        private GradeTourViewModel _viewModel;

        public GradeTourView(BookingApp.Model.TourReservation tourRes, int touristId)
        {
            InitializeComponent();
            _viewModel = new GradeTourViewModel(tourRes, touristId);
            DataContext = _viewModel;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveReviews();
        }

        private void AddPictureButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var gradeTourFormViewModel = button.DataContext as GradeTourFormViewModel;

            _viewModel.AddPicture(gradeTourFormViewModel);
        }


        private void RemovePictureButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var imagePath = button.Tag as string;
            var itemControl = FindParent<ItemsControl>(button);
            var gradeTourFormViewModel = itemControl.DataContext as GradeTourFormViewModel;

            _viewModel.RemovePicture(gradeTourFormViewModel, imagePath);
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent != null) return parent;
            return FindParent<T>(parentObject);
        }
    }
}
