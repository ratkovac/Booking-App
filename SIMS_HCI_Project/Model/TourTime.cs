using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    internal class TourTime
    {
        public int Id { get; set; }
        public int MaxGuests { get; set; }
        public DateTime time { get; set; }
        public int TourId { get; set; }
        public bool Started { get; set; }

    }
}
