using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class LocationService
    {
        private ILocationRepository locationRepository;

        public LocationService()
        {
            locationRepository = Injector.CreateInstance<ILocationRepository>();
        }
        public int NextId()
        {
            return locationRepository.NextId();
        }
        public List<Location> GetAll()
        {
            return locationRepository.GetAll();
        }
        public Location GetById(int id)
        {
            return locationRepository.GetById(id);
        }
        public void Create(Location location)
        {
            locationRepository.Create(location);
        }
        public void Delete(Location location)
        {
            locationRepository.Delete(location);
        }
        public void Update(Location location)
        {
            locationRepository.Update(location);
        }
        public void Subscribe(IObserver observer)
        {
            locationRepository.Subscribe(observer);
        }
        public int ExistsLocation(string city, string country)
        {
            return locationRepository.ExistsLocation(city, country);
        }
        public Location? GetLocationByCityAndCountry(string city, string country)
        {
            return locationRepository.GetLocationByCityAndCountry(city, country);
        }
        public int GetCityIdByName(string cityName)
        {
            return locationRepository.GetCityIdByName(cityName);
        }
        public List<KeyValuePair<int, string>> GetCitiesByCountry(string country)
        {
            return locationRepository.GetCitiesByCountry(country);
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
