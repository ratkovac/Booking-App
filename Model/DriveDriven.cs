using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BookingApp.Model
{
    public class DriveDriven: ISerializable
    {
        public int DriveId { get; set; }

        public Drive Drive { get; set; }

        public TimeSpan Duration { get; set; }

        public int Price { get; set; }

        public DriveDriven() { }

        public DriveDriven(int driveId, TimeSpan duration, int price) {
            this.DriveId = driveId;
            this.Duration = duration;
            this.Price = price;
        }
        public string[] ToCSV()
        {
            return new string[]
            {
                DriveId.ToString(),
                Duration.TotalSeconds.ToString(),
                Price.ToString()
            };
        }

        public override string ToString()
        {
            return $"DriveId: {DriveId}, Duration: {Duration}, Price: {Price}";
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 3)
            {
                throw new ArgumentException("Neispravan format CSV podataka.");
            }

            DriveId = Convert.ToInt32(values[0]);
            Duration = TimeSpan.FromSeconds(double.Parse(values[1]));
            Price = Convert.ToInt32(values[2]);
        }
    }
}

