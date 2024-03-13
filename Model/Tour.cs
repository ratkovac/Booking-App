using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xaml.Schema;

namespace BookingApp.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxGuests { get; set; }
        public float Duration { get; set; }
        public Location Location { get; set; }
        public Language Language { get; set; }

        public static int nextId = 1;


        public Tour()
        {

        }
        public Tour(string name, string description, int maxGeusts, float duration, Location location, Language language)
        {
            Id = nextId++;
            Name = name;
            Description = description;
            MaxGuests = maxGeusts;
            Duration = duration;
            Location = location;
            Language = language;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Description, MaxGuests.ToString(), Duration.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Description = values[2];
            MaxGuests = Convert.ToInt32(values[3]);
            Duration = Convert.ToSingle(values[4]);

            // Ovde bi trebalo dodati parsiranje za Location i Language ako su kompleksni tipovi podataka
        }

    }
}
