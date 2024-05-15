﻿using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BookingApp.Repository
{
    public class LocationRepository : ILocationRepository
    {

        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> _serializer;

        private List<Location> _locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            _locations = _serializer.FromCSV(FilePath);
        }

        public List<Location> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Location Save(Location location)
        {
            location.Id = NextId();           
            _locations = _serializer.FromCSV(FilePath);
            _locations.Add(location);
            _serializer.ToCSV(FilePath, _locations);
            return location;
        }

        public void Create(Location location)
        {
            location.Id = NextId();
            _locations.Add(location);
            _serializer.ToCSV(FilePath, _locations);
        }

        public int NextId()
        {
            _locations = _serializer.FromCSV(FilePath);
            if (_locations.Count < 1)
            {
                return 1;
            }
            return _locations.Max(c => c.Id) + 1;
        }

        public void Delete(Location location)
        {
            _locations = _serializer.FromCSV(FilePath);
            Location founded = _locations.Find(c => c.Id == location.Id);
            _locations.Remove(founded);
            _serializer.ToCSV(FilePath, _locations);
        }

        public void Update(Location location)
        {
            int index = _locations.FindIndex(gd => location.Id == gd.Id);
            if (index != -1)
            {
                _locations[index] = location;
                _serializer.ToCSV(FilePath, _locations);
            }
        }

        /*public Location Update(Location location)
        {
            _locations = _serializer.FromCSV(FilePath);
            Location current = _locations.Find(c => c.Id == location.Id);
            int index = _locations.IndexOf(current);
            _locations.Remove(current);
            _locations.Insert(index, location);        
            _serializer.ToCSV(FilePath, _locations);
            return location;
        }*/

        public Location? GetById(int locationId)
        {
            return _locations.Find(c => c.Id == locationId);
        }
        public List<Location> GetAllLocations()
        {
            _locations = _serializer.FromCSV(FilePath);
            return _locations;
        }
        public int ExistsLocation(string city, string country)
        {
            var existingLocation = _locations.FirstOrDefault(location => location.City == city && location.Country == country);
            return existingLocation != null ? existingLocation.Id : 0;
        }
        public Location? GetLocationByCityAndCountry(string city, string country)
        {
            return _locations.FirstOrDefault(location => location.City == city && location.Country == country);
        }
        public int GetCityIdByName(string cityName)
        {
            return _locations.FirstOrDefault(loc => loc.City == cityName)?.Id ?? 0;
        }
        public List<KeyValuePair<int, string>> GetCitiesByCountry(string country)
        {
            _locations = _serializer.FromCSV(FilePath);
            return _locations
                .Where(l => l.Country == country)
                .Select(l => new KeyValuePair<int, string>(l.Id, l.City))
                .ToList();
        }
        public List<KeyValuePair<int, string>> GetAllCountries()
        {
            var locations = GetAll();
            var countries = locations
                .GroupBy(loc => loc.Country)
                .Select(grp => grp.First())
                .Select(loc => new KeyValuePair<int, string>(loc.Id, loc.Country))
                .OrderBy(c => c.Value)
                .ToList();
            return countries;
        }
    }
}