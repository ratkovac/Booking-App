using BookingApp.Model;
using CLI.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookingApp.DTO;
using BookingApp.Service;
using System;
using BookingApp.Repository;
using System.Runtime.CompilerServices;

namespace BookingApp.View.ViewModel.Owner
{
    public class AccommodationsGradesViewModel : IObserver
    {
        public ObservableCollection<GradeAccommodationDTO> accommodationGrades { get; set; }
        private User user { get; set; }

        private List<double> allOverallGrades;

        private ImageRepository imageRepository;

        private GradeAccommodationService gradeAccommodationService;

        public AccommodationsGradesViewModel(User user)
        {
            this.user = user;
            gradeAccommodationService = new GradeAccommodationService();
            gradeAccommodationService.Subscribe(this);
            accommodationGrades = new ObservableCollection<GradeAccommodationDTO>();
            allOverallGrades = new List<Double>();
            imageRepository = new ImageRepository();
            Update();

        }

        public void allGrades()
        {
            accommodationGrades.Clear();
            foreach (var accommodationGrade in gradeAccommodationService.GetAll())
            { 
                if (user.Id == accommodationGrade.AccommodationReservation.Accommodation.User.Id)
                {
                    string imagePath;
                    Image frontImage = imageRepository.GetByAccommodationId(accommodationGrade.AccommodationReservation.Accommodation.Id);
                    if(frontImage != null)
                    {
                        imagePath = frontImage.Path;
                    }
                    else
                    {
                        imagePath = "/View/Owner/noimage.png";
                    }
                    accommodationGrades.Add(new GradeAccommodationDTO
                    {
                        Username = accommodationGrade.AccommodationReservation.User.Username,
                        AccommodationName = accommodationGrade.AccommodationReservation.Accommodation.Name,
                        Cleanliness = accommodationGrade.Cleanliness,
                        Correctness = accommodationGrade.Correctness,
                        FrontImagePath = imagePath,
                        OverallGrade = gradeCalculator(accommodationGrade.Cleanliness, accommodationGrade.Correctness)
                    }); ;
               
                    allOverallGrades.Add(gradeCalculator(accommodationGrade.Cleanliness, accommodationGrade.Correctness));
                }
            }
        }
        public double AvarageGrade()
        {
            double sum = 0;
            foreach(double grade in allOverallGrades)
            {
                sum += grade;
            }
            double avarage = sum / allOverallGrades.Count;
            return Math.Round(avarage,2);
        }//service
        private double gradeCalculator(int firstGrade, int secondGrade)
        {
            double sum = firstGrade + secondGrade;
            return sum / 2;
        } //service
        public void Update()
        {
            allGrades();
        }
    }
}
