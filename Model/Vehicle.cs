using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Vehicle : ISerializable
    {
        public int Id { get; set; }
        public List<Location> Locations { get; set; } 
        public int Capacity { get; set; }
        public List<Language> Languages { get; set; } 
        public List<string> ImagePaths { get; set; }
        public User User { get; set; }
        public int DriverId { get; set; }

        public Vehicle() { }

        public Vehicle(int id, List<Location> locations, int capacity, List<Language> languages, List<string> imagePaths, User user) 
        {
            Id = id;
            Locations = locations;
            Capacity = capacity;
            Languages = languages;
            ImagePaths = imagePaths;
            User = user;
        }

        UserRepository userRepository = new UserRepository();

        public void FromCSV(string[] values)
        {
            if (values.Length < 5)
            {
                throw new ArgumentException("Nedovoljno polja u nizu za konverziju u objekat Vehicle.");
            }

            Id = Convert.ToInt32(values[0]);
            int[] locationIds = values[1].Split(',').Select(int.Parse).ToArray(); 
            LocationRepository locationRepository = new LocationRepository();
            Locations = locationIds.Select(id => locationRepository.GetLocationById(id)).ToList(); 
            Capacity = Convert.ToInt32(values[2]);
            int[] languageIds = values[3].Split(',').Select(int.Parse).ToArray(); 
            LanguageRepository languageRepository = new LanguageRepository();
            Languages = languageIds.Select(id => languageRepository.GetLanguageById(id)).ToList(); 
            ImagePaths = values[4].Split(',').ToList(); 
            User = userRepository.GetByID(int.Parse(values[5]));
            DriverId = Convert.ToInt32(values[6]);
        }
        public string[] ToCSV()
        {
            string locations = string.Join(",", Locations.Select(location => location.Id.ToString())); 
            string languages = string.Join(",", Languages.Select(language => language.Id.ToString())); 
            string[] values =
            {
                Id.ToString(),
                locations,
                Capacity.ToString(),
                languages,
                string.Join(",", ImagePaths),
                User.Id.ToString(),
                DriverId.ToString()
            };
            return values;
        }
        public override string ToString()
        {

            string locationsStr = string.Join(",", Locations.Select(location => location.ToString())); 
            string languagesStr = string.Join(",", Languages.Select(language => language.ToString())); 
            return $"Id: {Id}, Locations: {locationsStr}, Capacity: {Capacity}, Languages: {languagesStr}, ImagePaths: {(ImagePaths != null ? string.Join(",", ImagePaths) : "NULL")}, User: {(User != null ? User.ToString() : "NULL")}";

        }
    }
}

