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
        public Location Location { get; set; }
        public int Capacity { get; set; }
        public Language Language { get; set; }
        public List<string> ImagePaths { get; set; }
        public User User { get; set; }

        public Vehicle() { }

        public Vehicle(int id, Location location, int capacity, Language language, List<string> imagePaths, User user)
        {
            Id = id;
            Location = location;
            Capacity = capacity;
            Language = language;
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
            int locationId = Convert.ToInt32(values[1]);
            LocationRepository locationRepository = new LocationRepository();
            Location = locationRepository.GetLocationById(locationId);
            Capacity = Convert.ToInt32(values[2]);
            LanguageRepository languageRepository = new LanguageRepository();
            Language = languageRepository.GetLanguageById(Convert.ToInt32(values[3]));
            ImagePaths = values.Skip(4).ToList();
            User = userRepository.GetById(int.Parse(values[5]));
        }
        public string[] ToCSV()
        {
            string location = Location.Id.ToString();
            string language = Language.Id.ToString();
            string[] values =
            {
                Id.ToString(),
                location,
                Capacity.ToString(),
                language,
                string.Join(",", ImagePaths),
                User.Id.ToString()
            };
            return values;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Location: {Location}, Capacity: {Capacity}, Language: {Language}, ImagePaths: {(ImagePaths != null ? string.Join(",", ImagePaths) : "NULL")}, User: {(User != null ? User.ToString() : "NULL")}";
        }
    }
}

