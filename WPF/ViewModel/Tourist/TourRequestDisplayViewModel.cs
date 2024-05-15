using BookingApp.Model;
using BookingApp.Service;
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
    public class TourRequestDisplayViewModel : INotifyPropertyChanged
    {
        private User Tourist;
        private TourRequestService _tourRequestService;
        private TourRequestSegmentService _tourRequestSegmentService;

        private ObservableCollection<TourRequestSegment> _listTourSegments;
        public ObservableCollection<TourRequestSegment> ListTourSegments
        {
            get { return _listTourSegments; }
            set
            {
                _listTourSegments = value;
                OnPropertyChanged();
            }
        }

        private TourRequestSegment _selectedTourSegment;
        public TourRequestSegment SelectedTourSegment
        {
            get { return _selectedTourSegment; }
            set
            {
                _selectedTourSegment = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourRequestDisplayViewModel(User tourist)
        {
            Tourist = tourist;
            _tourRequestService = new TourRequestService();
            _tourRequestSegmentService = new TourRequestSegmentService();
            CheckAndCancelWaitingTours();
            TourRequestSegmentService tourRequestSegmentService = new TourRequestSegmentService();
            ListTourSegments = new ObservableCollection<TourRequestSegment>(tourRequestSegmentService.GetAllTourRequestSegments());
        }

        private void CheckAndCancelWaitingTours()
        {
            var allSegments = _tourRequestSegmentService.GetAllTourRequestSegments().ToList();

            foreach (var segment in allSegments)
            {
                if (segment.IsAccepted == TourRequestStatus.WAITING && segment.StartDate.Subtract(DateTime.Now).TotalHours < 48)
                {
                    segment.IsAccepted = TourRequestStatus.CANCELLED;
                    _tourRequestSegmentService.Update(segment);
                    var tourRequest = _tourRequestService.GetTourRequestById(segment.TourRequestId);
                    tourRequest.IsAccepted = TourRequestStatus.CANCELLED;
                    _tourRequestService.Update(tourRequest);
                }
            }
        }

    }
}
