using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class CheckPoint
    {
        int Id { get; set; }
        public string PointText { get; set; }
        //public int TourId { get; set; }
        
        //, int tourId
        public CheckPoint(string pointText)
        {
            PointText = pointText;
           // TourId = tourId;
        }
    }   

}
