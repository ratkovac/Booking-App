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
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string Language { get; set; }
        public List<string> ImagePaths { get; set; }
        public User User { get; set; }

        public Vehicle() { }

        public Vehicle(int id, string location, int capacity, string language, List<string> imagePaths, User user)
        {
            Id = id;
            Location = location;
            Capacity = capacity;
            Language = language;
            ImagePaths = imagePaths;
            User = user;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length < 5) 
            {
                throw new ArgumentException("Nedovoljno polja u nizu za konverziju u objekat Vehicle.");
            }

            Id = Convert.ToInt32(values[0]);
            Location = values[1];
            Capacity = Convert.ToInt32(values[2]);
            Language = values[3];
            ImagePaths = values.Skip(4).ToList();
            User = new User();
        }
        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                Location,
                Capacity.ToString(),
                Language,
                string.Join(",", ImagePaths)
            };
            return values;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Location: {Location ?? "NULL"}, Capacity: {Capacity}, Language: {Language ?? "NULL"}, ImagePaths: {(ImagePaths != null ? string.Join(",", ImagePaths) : "NULL")}, User: {(User != null ? User.ToString() : "NULL")}";
        }
    }
}

