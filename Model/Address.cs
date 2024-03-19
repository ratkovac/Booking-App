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

        public Address()
        {
        }

        public string[] ToCSV()
        {
            string location = Location.Id.ToString();
            string[] csvValues = { 
                Id.ToString(),
                location.ToString(),
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
            int locationId = Convert.ToInt32(values[1]);
            LocationRepository locationRepository = new LocationRepository();
            Location = locationRepository.GetLocationById(locationId); 
            Street = values[2];
            Number = values[3];
        }
    }
    
}

