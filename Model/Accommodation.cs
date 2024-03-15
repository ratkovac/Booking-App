using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.Model
{
    public enum AccommodationType { Apartment, House, Hut };
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; } 
        public AccommodationType Type { get; set; }
        public int Capacity { get; set; }
        public int MinReservationDays { get; set; } 
        public int DaysBeforeCancel { get; set; }
        //public User User { get; set; }

        public Accommodation() 
        {
          
        }

        public Accommodation(int id, string name, Location location, AccommodationType type, int capacity, int minReservationDays, int daysBeforeCancel)
        {
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            Capacity = capacity;
            MinReservationDays = minReservationDays;
            DaysBeforeCancel = daysBeforeCancel;
        }

        public string[] ToCSV()
        {
            string location = Location.Id.ToString();
            string[] values =
            {
                Id.ToString(),
                Name,
                location,
                Type.ToString(),
                Capacity.ToString(),
                MinReservationDays.ToString(),
                DaysBeforeCancel.ToString()
            };
            return values;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            int locationId = Convert.ToInt32(values[2]);
            LocationRepository locationRepository = new LocationRepository();
            Location = locationRepository.GetLocationById(locationId);
            bool success = Enum.TryParse(values[3], out AccommodationType parsedType);
            Type = parsedType;
            Capacity = Convert.ToInt32(values[4]);
            MinReservationDays = Convert.ToInt32(values[5]);
            DaysBeforeCancel = Convert.ToInt32(values[6]);
        }
    }
}
