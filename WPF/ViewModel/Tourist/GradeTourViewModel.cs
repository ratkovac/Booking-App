using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class GradeTourViewModel
    {
        public BookingApp.Model.TourReservation SelectedTourReservation { get; set; }

        public ObservableCollection<GradeTourFormViewModel> ReviewForms { get; set; }

        private GradeTourService _gradeTourService;
        private TourGuestService _tourGuestService;

        public int TouristId { get; set; }

        public GradeTourViewModel(BookingApp.Model.TourReservation tourReservation, int touristId)
        {
            TouristId = touristId;
            _gradeTourService = new GradeTourService();
            _tourGuestService = new TourGuestService();
            SelectedTourReservation = tourReservation;
            ReviewForms = new ObservableCollection<GradeTourFormViewModel>();
            SetReviewForms();
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

        public void SaveReviews()
        {
            _gradeTourService.SaveReviews(ReviewForms, SelectedTourReservation.Id, TouristId);
        }

    }
}
