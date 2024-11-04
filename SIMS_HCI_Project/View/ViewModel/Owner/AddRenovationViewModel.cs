using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Service;
using CLI.Observer;
using GalaSoft.MvvmLight.Command;
using Syncfusion.Blazor.DropDowns;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.View.ViewModel.Owner
{
    public class AddRenovationViewModel : IObserver, INotifyPropertyChanged
    {
        public ICommand CalculatePossibleDatesCommand { get; private set; }
        public ICommand SelectDateCommand { get; private set; }
        public ICommand ConfirmRenovationCommand { get; private set; }

        private RenovationService renovationService;
        private AccommodationService accommodationService;
        private List<(DateTime, DateTime)> PossibleDates;
        public ObservableCollection<RenovationDatesDTO> renovationDates { get; set; }
        public AccommodationDTO SelectedAccommodation { get;}
        public RenovationDatesDTO SelectedDate { get; set; }

        public AddRenovationViewModel(AccommodationDTO selectedAccommodation)
        {
            renovationService = new RenovationService();
            accommodationService = new AccommodationService();
            SelectedAccommodation = selectedAccommodation;
            PossibleDates = new List<(DateTime, DateTime)>();
            renovationDates = new ObservableCollection<RenovationDatesDTO>();
            CalculatePossibleDatesCommand = new RelayCommand(CalculatePossibleDates);
            SelectDateCommand = new RelayCommand<RenovationDatesDTO>(SelectDate);
            ConfirmRenovationCommand = new RelayCommand(ConfirmRenovation);
        }
        public AddRenovationViewModel()
        {
            CalculatePossibleDatesCommand = new RelayCommand(CalculatePossibleDates);
        }
        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        private int _numberOfDays;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int NumberOfDays
        {
            get { return _numberOfDays; }
            set
            {
                if (_numberOfDays != value)
                {
                    _numberOfDays = value;
                    OnPropertyChanged(nameof(NumberOfDays));
                }
            }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CalculatePossibleDates()
        {
            DateTime start = StartDate;
            DateTime end = EndDate;
            int numberOfDays = NumberOfDays;

            List<(DateTime, DateTime)> reservations = renovationService.GetAllReservations(SelectedAccommodation.Id);
            List<(DateTime, DateTime)> renovations = renovationService.GetAllPossibleDates(start, end, numberOfDays);
            PossibleDates = renovationService.GetNonOverlappingRenovationDates(renovations, reservations);
            Update();
        }
        public void allDates()
        {
            renovationDates.Clear();
            foreach(var date in PossibleDates)
            {
                renovationDates.Add(new RenovationDatesDTO
                {
                    StartDate = DateOnly.FromDateTime(date.Item1),
                    EndDate = DateOnly.FromDateTime(date.Item2),
                    IsSelected = false
                });
            }
        }
        private void SelectDate(RenovationDatesDTO selectedDateDTO)
        {
            if (selectedDateDTO != null)
            {
                if (SelectedDate != null && SelectedDate == selectedDateDTO)
                {
                    SelectedDate.IsSelected = false;
                    SelectedDate = null;
                }
                else
                {
                    if (SelectedDate != null)
                    {
                        SelectedDate.IsSelected = false;
                    }
                    selectedDateDTO.IsSelected = true;
                    SelectedDate = selectedDateDTO;
                }
            }
        }

        private void ConfirmRenovation()
        {
            Accommodation accommodation = accommodationService.GetById(SelectedAccommodation.Id);
            Renovations renovation = new Renovations(accommodation,SelectedDate.StartDate, SelectedDate.EndDate, Description);
            renovationService.Create(renovation);
            MessageBox.Show("Uspesno dodato renoviranje!");
        }
        public void Update()
        {
            allDates();
        }
    }
}
