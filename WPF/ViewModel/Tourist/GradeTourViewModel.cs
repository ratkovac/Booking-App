using BookingApp.Model;
using BookingApp.Repository;
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
        private TourReservationService _tourReservationService;
        private ImageRepository _imageRepository;
        private TourGuestRepository _tourGuestRepository;

        public int TouristId { get; set; }

        public GradeTourViewModel(BookingApp.Model.TourReservation tourReservation, int touristId)
        {
            TouristId = touristId;
            _gradeTourService = new GradeTourService();
            _imageRepository = new ImageRepository();
            _tourGuestRepository = new TourGuestRepository();
            _tourReservationService = new TourReservationService();
            SelectedTourReservation = tourReservation;
            ReviewForms = new ObservableCollection<GradeTourFormViewModel>();
            SetReviewForms();
        }

        private void SetReviewForms()
        {
            List<TourGuest> guests = _tourGuestRepository.GetAllByTourReservationId(SelectedTourReservation.Id);

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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                reviewTourFormViewModel.ImagePaths.Add(openFileDialog.FileName);

            }
        }
        public void RemovePicture(GradeTourFormViewModel reviewForm, string imagePath)
        {
            if (reviewForm != null && reviewForm.ImagePaths.Contains(imagePath))
            {
                reviewForm.ImagePaths.Remove(imagePath);
            }
        }

        public void SaveReviews()
        {
            foreach (var reviewForm in ReviewForms)
            {
                GradeTour grade = new GradeTour(SelectedTourReservation.Id, TouristId, reviewForm.Guest.Id, reviewForm.SelectedRating, reviewForm.Comment, true);
                _gradeTourService.Create(grade);
                foreach (var path in reviewForm.ImagePaths)
                {
                    Image image = new Image(path, grade.Id, TouristId);
                    _imageRepository.Save(image);
                }
            }
            SelectedTourReservation.RatedTour = true;
            _tourReservationService.Update(SelectedTourReservation);
        }


    }
}
