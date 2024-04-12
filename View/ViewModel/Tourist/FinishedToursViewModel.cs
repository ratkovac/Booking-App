using BookingApp.Model;
using BookingApp.View.Tourist.Pages;
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

namespace BookingApp.View.ViewModel.Tourist
{
    public class FinishedToursViewModel : IObserver
    {
        private TourInstanceService tourInstanceService;
        public TourReservationService tourReservationService;
        public ObservableCollection<TourInstance> ListTourInstance { get; set; }
        public TourInstance SelectedTourInstance { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }
        public Action NavigateToGradeTour { get; set; }
        public FinishedToursViewModel(BookingApp.Model.Tourist tourist)
        {
            Tourist = tourist;
            tourInstanceService = new TourInstanceService();
            tourReservationService = new TourReservationService();
            tourReservationService.Subscribe(this);
            tourInstanceService.Subscribe(this);
            ListTourInstance = new ObservableCollection<TourInstance>(tourInstanceService.GetToursWhichFinished());
        }
        private void UpdateListTourInstance()
        {
            ListTourInstance.Clear();
            foreach (var tour in tourInstanceService.GetToursWhichFinished())
            {
                ListTourInstance.Add(tour);
            }
        }
        public void Update()
        {
            UpdateListTourInstance();
        }
        public void GradeTour()
        {
            if (SelectedTourInstance != null)
            {
                var gradeTour = new GradeTourView(SelectedTourInstance, Tourist, tourReservationService);
                NavigateToGradeTour?.Invoke();
            }
            else
            {
                MessageBox.Show("You must select a tour for rating!");
            }
        }
    }
}
