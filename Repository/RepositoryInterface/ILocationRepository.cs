using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface ILocationRepository : IGenericRepository<Location, int>
    {
        public List<KeyValuePair<int, string>> GetCitiesByCountry(string country);
        public int GetCityIdByName(string cityName);
        public Location? GetLocationByCityAndCountry(string city, string country);
        public int ExistsLocation(string city, string country);

        public void Subscribe(IObserver observer)
        {

        }
    }
}
