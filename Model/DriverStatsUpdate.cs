using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class DriverStatsUpdate: ISerializable
    {
        public int Id { get; set; } 
        public int DriverId {  get; set; }
        public int FastDrivesUpdate { get; set; }

        public int BonusPointsUpdate { get; set; }

        public int CancelledDrivesUpdate { get; set; }

        public DateTime DateOfUpdate {  get; set; }

        public DriverStatsUpdate() { }
        public DriverStatsUpdate(int id)
        {
            DriverId = id;
            this.FastDrivesUpdate = 0;
            this.BonusPointsUpdate = 0;
            this.CancelledDrivesUpdate = 0;
            DateOfUpdate = DateTime.Today;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                DriverId.ToString(),
                FastDrivesUpdate.ToString(),
                BonusPointsUpdate.ToString(),
                CancelledDrivesUpdate.ToString(),
                DateOfUpdate.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 6)
            {
                throw new ArgumentException("Neispravan format CSV podataka.");
            }
            Id = Convert.ToInt32(values[0]);
            DriverId = Convert.ToInt32(values[1]);
            FastDrivesUpdate = Convert.ToInt32(values[2]);
            BonusPointsUpdate = Convert.ToInt32(values[3]);
            CancelledDrivesUpdate = Convert.ToInt32(values[4]);
            DateOfUpdate = Convert.ToDateTime(values[5]);
        }
    }
}
