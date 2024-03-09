using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.Model
{
    public enum Type { Apartment, House, Hut };
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; } /* --> Moguca ispravka <-- */
        public Type AccommodationType { get; set; }
        public int MaxGuest { get; set; }
        public int MinNumberOfDaysForReservation { get; set; } 
        public int NumberOfDayCancelReservation { get; set; }
        public User User { get; set; }

        //public List<string> Image; /* --> Moguca ispravka <-- */

        public Accommodation() 
        {
            //Image = new List<string>();
        }

        public Accommodation(int id, string name, string location, int maxGuest, int minNumberOfDaysForReservation, User user, int numberOfDayCancelReservation = 1)
        {
            Id = id;
            Name = name;
            Location = location;
            MaxGuest = maxGuest;
            MinNumberOfDaysForReservation = minNumberOfDaysForReservation;
            NumberOfDayCancelReservation = numberOfDayCancelReservation;
            //Image = new List<string>();
            User = user;
        }

        public string[] ToCSV()
        {
            string[] values =
{
                Id.ToString(),
                Name,
                Location,
                MaxGuest.ToString(),
                MinNumberOfDaysForReservation.ToString(),
                NumberOfDayCancelReservation.ToString()
            };
            return values;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location = values[3];
            MaxGuest = Convert.ToInt32(values[4]);
            MinNumberOfDaysForReservation = Convert.ToInt32(values[5]);
            NumberOfDayCancelReservation = Convert.ToInt32(values[6]);
        }
    }
}
