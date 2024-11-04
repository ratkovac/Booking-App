using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    class DriveStats
    {
        public int DriveId { get; set; }

        public Drive Drive { get; set; }

        public DriveDriven Stats { get; set; }

        public DriveStats()
        {
        }
        public DriveStats(int driveId, Drive drive, DriveDriven stats)
        {
            DriveId = driveId;
            Drive = drive;
            Stats = stats;
        }

    }
}
