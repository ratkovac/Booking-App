using BookingApp.Model;
using BookingApp.WPF.View.Tourist.Pages;
using BookingApp.Service;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class FinishedToursViewModel : IObserver
    {
        private TourInstanceService tourInstanceService;
        public TourReservationService tourReservationService;
        public ObservableCollection<BookingApp.Model.TourReservation> ListTourReservation { get; set; }
        public BookingApp.Model.TourReservation SelectedTourReservation { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }
        public Action NavigateToGradeTour { get; set; }
        public FinishedToursViewModel(BookingApp.Model.Tourist tourist)
        {
            Tourist = tourist;
            tourInstanceService = new TourInstanceService();
            tourReservationService = new TourReservationService();
            tourReservationService.Subscribe(this);
            tourInstanceService.Subscribe(this);
            ListTourReservation = new ObservableCollection<BookingApp.Model.TourReservation>(tourReservationService.GetToursWhichFinished());
        }
        private void UpdateListTourInstance()
        {
            ListTourReservation.Clear();
            foreach (var tour in tourReservationService.GetToursWhichFinished())
            {
                ListTourReservation.Add(tour);
            }
        }
        public void Update()
        {
            UpdateListTourInstance();
        }
        public void GradeTour()
        {
            if (SelectedTourReservation != null)
            {
                var gradeTour = new GradeTourView(SelectedTourReservation, Tourist.Id);
                NavigateToGradeTour?.Invoke();
            }
            else
            {
                MessageBox.Show("You must select a tour for rating!");
            }
        }
    }
}
