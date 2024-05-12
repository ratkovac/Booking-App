using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.NGuest;
using BookingApp.View.Owner;
using CLI.Observer;
using GalaSoft.MvvmLight.Command;

namespace BookingApp.View.ViewModel.Owner
{
    public class OwnerFrontPageViewModel : IObserver
    {
        private User LoggedInUser;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationService accommodationService;
        private List<Double> accommoationGrades = new List<Double>();
        public ObservableCollection<AccommodationDTO> accommodations { get; set; }
        public ICommand RenovationCommand { get; private set; }
        public ICommand StatisticCommand { get; set; }
        public bool SuperOwner(User user)
        {
            foreach (AccommodationReservation ar in accommodationReservationService.GetAll())
            {
                if (ar.AccommodationGrade != 0 && ar.Accommodation.User.Id == user.Id)
                {
                    accommoationGrades.Add(ar.AccommodationGrade);
                }
            }
            double avarageAccommodationGrade = AvarageAccommoationGrade();
            if (accommoationGrades.Count >= 50 && avarageAccommodationGrade >= 4.5)
                return true;
            else
                return false;
        }
        public void allAccommodations()
        {
            accommodations.Clear();
            foreach(var accommodation in accommodationService.GetAll())
            {
                if(accommodation.User.Id == LoggedInUser.Id)
                {
                    accommodations.Add(new AccommodationDTO
                    {
                        Id = accommodation.Id,
                        City = accommodation.Location.City,
                        Country = accommodation.Location.Country,
                        Type = accommodation.Type,
                        Name = accommodation.Name
                    });
                }
            }
        }
        private double AvarageAccommoationGrade()
        {
            double sum = 0;
            int len = 0;
            double avg = 0;
            foreach (Double aag in accommoationGrades)
            {
                sum += aag;
            }
            len = accommoationGrades.Count;
            avg = sum / len;
            return avg;
        }
        public OwnerFrontPageViewModel(User user)
        {
            accommodationReservationService = new AccommodationReservationService();
            accommodations = new ObservableCollection<AccommodationDTO>();
            accommodationService = new AccommodationService();
            LoggedInUser = user;
            RenovationCommand = new RelayCommand<AccommodationDTO>(ExecuteRenovationCommand);
            StatisticCommand = new RelayCommand<AccommodationDTO>(ExecuteStatisticCommand);
            Update();
        }
        private void ExecuteRenovationCommand(AccommodationDTO SelectedAccommodation)
        {
            AddRenovationViewModel addRenovationViewModel = new AddRenovationViewModel(SelectedAccommodation);
            AddRenovation addRenovation = new AddRenovation(addRenovationViewModel, SelectedAccommodation);
            addRenovation.Show();
        }
        private void ExecuteStatisticCommand(AccommodationDTO SelectedAccommodation)
        {
            AccommodationStatisticViewModel accommodationStatisticViewModel = new AccommodationStatisticViewModel(SelectedAccommodation);
            AccommodationStatistic accommodationStatistic = new AccommodationStatistic(accommodationStatisticViewModel, SelectedAccommodation);
            accommodationStatistic.Show();
        }
        public void Update()
        {
            allAccommodations();
        }
    }
}
