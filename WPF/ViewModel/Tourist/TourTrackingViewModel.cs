using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourTrackingViewModel
    {
        public TourInstanceViewModel TourInstance { get; set; }
        public TourTrackingViewModel(TourInstanceViewModel tourInstance)
        {
            TourInstance = tourInstance;
        }
    }
}
