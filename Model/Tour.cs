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
            Location = location;
            Language = language;
        }

        public string[] ToCSV()
        {         
            string location = Location.Id.ToString();
            string language = Language.Id.ToString();
            string[] csvValues = { Id.ToString(), Name, location, Description, language, 
                MaxGuests.ToString(), Duration.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            ParseLocation(values);
            Description = values[3];
            ParseLanguage(values);
            MaxGuests = Convert.ToInt32(values[5]);
            Duration = Convert.ToSingle(values[6]);
        }

        private void ParseLocation(string[] values)
        {
            int locationId = Convert.ToInt32(values[2]);
            LocationRepository locationRepository = new LocationRepository();
            Location = locationRepository.GetLocationById(locationId);
        }

        private void ParseLanguage(string[] values)
        {
            int languageId = Convert.ToInt32(values[4]);
            LanguageRepository languageRepository = new LanguageRepository();
            Language language = languageRepository.GetLanguageById(languageId);
            if (language != null)
            {
                Language = language;
            }
        }

    }
}
