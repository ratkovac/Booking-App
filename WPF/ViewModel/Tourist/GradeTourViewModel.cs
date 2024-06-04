using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Service;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class GradeTourViewModel
    {
        public BookingApp.Model.TourReservation SelectedTourReservation { get; set; }

        public ObservableCollection<GradeTourFormViewModel> ReviewForms { get; set; }

        private GradeTourService _gradeTourService;
        private TourGuestService _tourGuestService;
        public ICommand AddPictureCommand { get; set; }
        public ICommand RemovePictureCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        public int TouristId { get; set; }

        public GradeTourViewModel(BookingApp.Model.TourReservation tourReservation, int touristId)
        {
            TouristId = touristId;
            _gradeTourService = new GradeTourService();
            _tourGuestService = new TourGuestService();
            SelectedTourReservation = tourReservation;
            ReviewForms = new ObservableCollection<GradeTourFormViewModel>();
            AddPictureCommand = new RelayCommand<object>(ExecuteAddPictureCommand);
            RemovePictureCommand = new RelayCommand<object>(ExecuteRemovePictureCommand);
            ConfirmCommand = new RelayCommand<GradeTourViewModel>(ExecuteConfirmCommand);
            SetReviewForms();
        }

        private void ExecuteAddPictureCommand(object sender)
        {
            var button = sender as Button;
            if (button != null && button.DataContext is GradeTourFormViewModel gradeTourFormViewModel)
            {
                AddPicture(gradeTourFormViewModel);
            }
        }

        private void ExecuteRemovePictureCommand(object sender)
        {
            var button = sender as Button;
            if (button != null && button.DataContext is GradeTourFormViewModel gradeTourFormViewModel)
            {
                RemoveLastPicture(gradeTourFormViewModel);
            }
        }

        private void ExecuteConfirmCommand(GradeTourViewModel gradeTourViewModel)
        {
            if (AreAllGradesEntered())
            {
                SaveReviews();
                ResetReviewForms();
            }
            else
            {
                MessageBox.Show("Please enter grades for all guests before confirming.");
            }
        }

        private bool AreAllGradesEntered()
        {
            foreach (var reviewForm in ReviewForms)
            {
                if (reviewForm.SelectedGrade == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void SetReviewForms()
        {
            List<TourGuest> guests = _tourGuestService.GetAllByTourReservationId(SelectedTourReservation.Id);

            foreach (var guest in guests)
            {
                if (guest.CheckpointId != 0)
                {
                    ReviewForms.Add(new GradeTourFormViewModel { Guest = guest });
                }
            }
        }

        public void AddPicture(GradeTourFormViewModel reviewTourFormViewModel)
        {
            string[] selectedFiles = OpenImageFileDialog();

            if (selectedFiles != null)
            {
                foreach (var file in selectedFiles)
                {
                    reviewTourFormViewModel.ImagePaths.Add(file);
                }
            }
        }

        public void RemoveLastPicture(GradeTourFormViewModel gradeTourFormViewModel)
        {
            if (gradeTourFormViewModel.ImagePaths.Any())
            {
                gradeTourFormViewModel.ImagePaths.RemoveAt(gradeTourFormViewModel.ImagePaths.Count - 1);
            }
        }

        private string[] OpenImageFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileNames;
            }

            return null;
        }

        private void ResetReviewForms()
        {
            ReviewForms.Clear();
            SetReviewForms();
        }

        public void SaveReviews()
        {
            _gradeTourService.SaveReviews(ReviewForms, SelectedTourReservation.Id, TouristId);
            if (App.CurrentLanguage == "en-US")
            {
                MessageBox.Show("Tour successfully rated!");
            }
            else
            {
                MessageBox.Show("Tura uspješno ocijenjena!");
            }
        }

    }
}
