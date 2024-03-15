using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
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
        public int LocationId { get; set; }



        public Tour()
        {

        }
        public Tour(string name, string description, int maxGeusts, float duration, Location location, Language language)
        {
            LocationRepository locationRepository = new LocationRepository();
            Name = name;
            Description = description;
            MaxGuests = maxGeusts;
            Duration = duration;     
            //LocationId = locationId;
            Location = location;
            Language = language;
        }

        public string[] ToCSV()
        {
            MessageBox.Show(string.Format("Id: {0}", Id));
            MessageBox.Show(string.Format("Name: {0}", Name));
            MessageBox.Show(string.Format("Location Id: {0}", Location.Id.ToString()));
            MessageBox.Show(string.Format("Description: {0}", Description));
            MessageBox.Show(string.Format("Language: {0}", Language.Name));
            MessageBox.Show(string.Format("Max Guests: {0}", MaxGuests));
            MessageBox.Show(string.Format("Duration: {0}", Duration));

            string location = Location.Id.ToString();
            string[] csvValues = { Id.ToString(), Name, location, Description, Language.Name, 
                MaxGuests.ToString(), Duration.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            

            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            int locationId = Convert.ToInt32(values[2]);
            LocationRepository locationRepository = new LocationRepository();
            Location = locationRepository.GetLocationById(locationId);
            Description = values[3];
            Language language = new Language(values[4]);
            Language = language;
            MaxGuests = Convert.ToInt32(values[5]);
            Duration = Convert.ToSingle(values[6]);
            
        }

    }
}
