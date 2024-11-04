using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class DriverStats: ISerializable
    {
        public int DriverId { get; set; }
        public User Driver { get; set; }
        public int FastDrives { get; set; }
        public int BonusPoints { get; set; }
        public int CancelledDrives { get; set; }

        public DriverStats()
        {

        }

        public DriverStats(int driverId, int fastDrives, int bonusPoints, int cancelledDrives)
        {
            DriverId = driverId;
            FastDrives = fastDrives;
            BonusPoints = bonusPoints;
            CancelledDrives = cancelledDrives;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                DriverId.ToString(),
                FastDrives.ToString(),
                BonusPoints.ToString(),
                CancelledDrives.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 4)
            {
                throw new ArgumentException("Neispravan format CSV podataka.");
            }
            DriverId = Convert.ToInt32(values[0]);
            FastDrives = Convert.ToInt32(values[1]);
            BonusPoints = Convert.ToInt32(values[2]);
            CancelledDrives = Convert.ToInt32(values[3]);
        }
    }
}
