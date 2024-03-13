using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    internal class TourGuest
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age {  get; set; }
        public int TourReservationId { get; set; }
        public int CheckPointId { get; set; }

    }
}
