using BookingApp.Model;
using BookingApp.Service;
using Syncfusion.Blazor.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ComplexRequestDisplayViewModel : INotifyPropertyChanged
    {
        private User Tourist;
        private TourRequestService _tourRequestService;
        private TourRequestSegmentService _tourRequestSegmentService;

        private ObservableCollection<TourRequest> _listComplexTours;
        public ObservableCollection<TourRequest> ListComplexTours
        {
            get { return _listComplexTours; }
            set
            {
                _listComplexTours = value;
                OnPropertyChanged();
            }
        }

        private TourRequest _selectedComplexTour;
        public TourRequest SelectedComplexTour
        {
            get { return _selectedComplexTour; }
            set
            {
                _selectedComplexTour = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ComplexRequestDisplayViewModel(User tourist)
        {
            Tourist = tourist;
            _tourRequestService = new TourRequestService();
            _tourRequestSegmentService = new TourRequestSegmentService();
            ListComplexTours = new ObservableCollection<TourRequest>(_tourRequestService.GetComplexRequests());
            foreach (var tourRequest in ListComplexTours)
            {
                tourRequest.SegmentCount = _tourRequestSegmentService.GetAllComplexSegmentsByComplexTourRequestId(tourRequest.Id).Count;
            }
        }
    }
}
