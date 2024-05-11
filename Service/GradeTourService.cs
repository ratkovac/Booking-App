using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel.Tourist;

namespace BookingApp.Service
{
    public class GradeTourService
    {
        private IGradeTourRepository gradeTourRepository;
        private ITouristRepository touristRepository;
        private ITourInstanceRepository tourInstanceRepository;
        private ITourReservationRepository tourReservationRepository;
        private TourReservationService tourReservationService;
        //private TourReservationRepository tourReservationRepository;
        private ITourImageRepository tourImageRepository;

        public GradeTourService()
        {
            gradeTourRepository = Injector.CreateInstance<IGradeTourRepository>();
            touristRepository = Injector.CreateInstance<ITouristRepository>();
            tourReservationRepository = Injector.CreateInstance<ITourReservationRepository>();
            tourInstanceRepository = Injector.CreateInstance<ITourInstanceRepository>();
            tourImageRepository = Injector.CreateInstance<ITourImageRepository>();
            tourReservationRepository = new TourReservationRepository();
            tourReservationService = new TourReservationService();
            InitializeTourist();
            InitializeTourReservation();
        }
        private void InitializeTourist()
        {
            foreach (var item in gradeTourRepository.GetAll())
            {
                item.Tourist = touristRepository.GetById(item.TouristId);
            }
        }
        private void InitializeTourReservation()
        {
            foreach (var item in gradeTourRepository.GetAll())
            {
                item.TourReservation = tourReservationRepository.GetById(item.Id);
            }
        }
        public List<GradeTour> GetAll()
        {
            return gradeTourRepository.GetAll();
        }

        public GradeTour GetById(int id)
        {
            return gradeTourRepository.GetById(id);
        }
        public List<GradeTour> GetAllRatingsByTour(TourReservation tourReservation)
        {
            return gradeTourRepository.GetAllRatingsByTour(tourReservation);
        }
        public void Create(GradeTour gradeTours)
        {
            gradeTourRepository.Create(gradeTours);
        }
        public void ReportRating(GradeTour gradeTours)
        {
            gradeTours.IsValid = false;
            gradeTourRepository.Update(gradeTours);
        }
        public void Delete(GradeTour gradeTours)
        {
            gradeTourRepository.Delete(gradeTours);
        }
        public void Update(GradeTour gradeTours)
        {
            gradeTourRepository.Update(gradeTours);
        }
        public void Subscribe(IObserver observer)
        {
            gradeTourRepository.Subscribe(observer);
        }
        public void SaveReviews(IEnumerable<GradeTourFormViewModel> reviewForms, int tourReservationId, int touristId)
        {
            foreach (var reviewForm in reviewForms)
            {
                GradeTour grade = new GradeTour(tourReservationId, touristId, reviewForm.Guest.Id, reviewForm.SelectedGrade, reviewForm.Comment, true);
                gradeTourRepository.Create(grade);
                foreach (var path in reviewForm.ImagePaths)
                {
                    TourImage tourImage = new TourImage(path, grade.Id, touristId);
                    tourImageRepository.Create(tourImage);
                }
            }

            tourReservationService.MarkTourReservationAsRated(tourReservationId);
        }
    }
}
