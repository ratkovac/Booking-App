using BookingApp.Model;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Repository
{
    public class VehicleRepository
    {
        private const string FilePath = "../../../Resources/Data/vehicle.csv";

        private readonly Serializer<Vehicle> _serializer;

        private List<Vehicle> _vehicles;

        public Subject VehicleSubject;

        public VehicleRepository()
        {
            _serializer = new Serializer<Vehicle>();
            _vehicles = _serializer.FromCSV(FilePath);
            VehicleSubject = new Subject(); 
        }

        public List<Vehicle> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Vehicle Save(Vehicle vehicle)
        {
            vehicle.Id = NextId();
            _vehicles = _serializer.FromCSV(FilePath);
            _vehicles.Add(vehicle);
            VehicleSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _vehicles);
            return vehicle;
        }

        public int NextId()
        {
            _vehicles = _serializer.FromCSV(FilePath);
            if(_vehicles.Count < 1)
            {
                return 1;
            }
            return _vehicles.Max(c => c.Id) + 1;
        }

        public void Delete(Vehicle vehicle) 
        {
            _vehicles = _serializer.FromCSV(FilePath);
            Vehicle founded = _vehicles.Find(c => c.Id == vehicle.Id);
            if (founded != null)
            {
                _vehicles.Remove(founded);
            }
            VehicleSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _vehicles);
        }

        public Vehicle Update(Vehicle vehicle)
        {
            _vehicles = _serializer.FromCSV(FilePath);
            Vehicle current = _vehicles.Find(c => c.Id == vehicle.Id);
            int index = _vehicles.IndexOf(current);
            _vehicles.Remove(current);
            _vehicles.Insert(index, vehicle);
            _serializer.ToCSV(FilePath, _vehicles);
            VehicleSubject.NotifyObservers();
            return vehicle;
        }
        public List<Vehicle> GetByUser(User user)
        {
            _vehicles = _serializer.FromCSV(FilePath);
            return _vehicles.FindAll(c => c.User.Id == user.Id);
        }

        public List<int> GetDriverIdsByLocationId(int locationId)
        {
            _vehicles = _serializer.FromCSV(FilePath);

            var driverIds = _vehicles
                .Where(vehicle => vehicle.Locations.Any(location => location.Id == locationId)) 
                .Select(vehicle => vehicle.User.Id)
                .Distinct()
                .ToList();

            return driverIds;
        }

        public void Subscribe(IObserver observer)
        {

        }

        public List<Vehicle> GetVehiclesByDriver(User user)
        {
            _vehicles = _serializer.FromCSV(FilePath);
            List<Vehicle> filteredVehicles = _vehicles.FindAll(v => v.User.Id == user.Id);
            return new List<Vehicle>(filteredVehicles);
        }
        public List<int> GetLocationsByDriver(User user)
        {
            _vehicles = _serializer.FromCSV(FilePath);

            var locationIds = _vehicles
                .Where(vehicle => vehicle.User.Id == user.Id)
                .SelectMany(vehicle => vehicle.Locations.Select(location => location.Id))
                .Distinct()
                .ToList();

            return new List<int>(locationIds);
        }
    }
}