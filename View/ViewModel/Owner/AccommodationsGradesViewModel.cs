using BookingApp.Model;
using CLI.Observer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookingApp.DTO;
using BookingApp.Service;
using System;

namespace BookingApp.View.ViewModel.Owner
{
    public class AccommodationsGradesViewModel : IObserver
    {
        public ObservableCollection<GradeAccommodationDTO> accommodationGrades { get; set; }
        private GradeAccommodation gradeAccommodation { get; set; }
        private User user { get; set; }

        private List<double> allOverallGrades;

        private GradeAccommodationService gradeAccommodationService;

        public AccommodationsGradesViewModel(User user)
        {
            this.user = user;
            gradeAccommodationService = new GradeAccommodationService();
            gradeAccommodationService.Subscribe(this);
            accommodationGrades = new ObservableCollection<GradeAccommodationDTO>();
            allOverallGrades = new List<Double>();
            Update();

        }

        public ObservableCollection<GradeAccommodationDTO> allGrades()
        {
            accommodationGrades.Clear();
            foreach (var accommodationGrade in gradeAccommodationService.GetAll())
            {
                if (user.Id == accommodationGrade.AccommodationReservation.Accommodation.User.Id)
                {
                    accommodationGrades.Add(new GradeAccommodationDTO
                    {
                        Username = accommodationGrade.AccommodationReservation.User.Username,
                        AccommodationName = accommodationGrade.AccommodationReservation.Accommodation.Name,
                        Cleanliness = accommodationGrade.Cleanliness,
                        Correctness = accommodationGrade.Correctness,
                        OverallGrade = gradeCalculator(accommodationGrade.Cleanliness, accommodationGrade.Correctness)
                    });
                    allOverallGrades.Add(gradeCalculator(accommodationGrade.Cleanliness, accommodationGrade.Correctness));
                }
            }
            return accommodationGrades;
        }
        public double AvarageGrade()
        {
            double sum = 0;
            foreach(double grade in allOverallGrades)
            {
                sum += grade;
            }
            double avarage = sum / allOverallGrades.Count;
            return avarage;
        }
        private double gradeCalculator(int firstGrade, int secondGrade)
        {
            double sum = firstGrade + secondGrade;
            return sum / 2;
        }
        public void Update()
        {
            accommodationGrades = allGrades();
        }
    }
}
