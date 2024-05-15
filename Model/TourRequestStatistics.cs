using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class TourRequestStatistics
    {
        public int ChosenYear { get; set; }
        public int AcceptedTours { get; set; }
        public int DeclinedTours { get; set; }

        public TourRequestStatistics() {}

        public TourRequestStatistics(int acceptedTours, int declinedTours, int chosenYear)
        {
            AcceptedTours = acceptedTours;
            DeclinedTours = declinedTours;
            ChosenYear = chosenYear;
        }

        public string[] ToCSV()
        {
            return new string[] { ChosenYear.ToString(), AcceptedTours.ToString(), DeclinedTours.ToString() };
        }

        public void FromCSV(string[] values)
        {
            ChosenYear = Convert.ToInt32(values[0]);
            AcceptedTours = Convert.ToInt32(values[1]);
            DeclinedTours = Convert.ToInt32(values[2]);
        }
    }
}
