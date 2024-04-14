using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Address : ISerializable
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public int LocationId { get; set; }

        public Address()
        {
        }

        public Address(int locationId, string street, string number)
        {
            LocationId = locationId;
            Street = street;
            Number = number;
        }

        public Address(int id, Location location, string street, string number)
        {
            Id = id;
            Location = location;
            Street = street;
            Number = number;
        }

        public string[] ToCSV()
        {
            string location = Location.Id.ToString();
            string[] csvValues = { 
                Id.ToString(),
                location,
                Street,
                Number };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 4)
            {
                throw new ArgumentException("Neispravan format CSV podataka.");
            }

            Id = Convert.ToInt32(values[0]);
            LocationId = Convert.ToInt32(values[1]);
            LocationRepository locationRepository = new LocationRepository();
            Location = locationRepository.GetLocationById(LocationId);
            Street = values[2];
            Number = values[3];
        }
    }
    
}

