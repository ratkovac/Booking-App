using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class EventEnum
    {
        public enum EventType {
            Reserved,
            Cancelled,
            Moved,
            RenovationProposal
        };
    }
}