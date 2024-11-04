using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Service
{
    public class FastDriveService
    {
        private IFastDriveRepository fastDriveRepository;
        private readonly DriveRepository _driveRepository;
        private LocationRepository _locationRepository { get; set; }
        private AddressRepository _addressRepository { get; set; }
        private readonly DriverStatsRepository _driverStatsRepository;
        private readonly DriverStatsUpdateRepository _driverStatsUpdateRepository;

        public FastDriveService()
        {
            fastDriveRepository = Injector.CreateInstance<IFastDriveRepository>();
            _driveRepository = new DriveRepository();
            _locationRepository = new LocationRepository();
            _addressRepository = new AddressRepository();
            _driverStatsRepository = new DriverStatsRepository();
            _driverStatsUpdateRepository = new DriverStatsUpdateRepository();
        }
        public int NextId()
        {
            return fastDriveRepository.NextId();
        }
        public List<FastDrive> GetAllFastDrives()
        {
            return fastDriveRepository.GetAll();
        }
        public FastDrive GetFastDriveById(int id)
        {
            return fastDriveRepository.GetById(id);
        }
        public void Create(FastDrive fastDrive)
        {
            fastDriveRepository.Create(fastDrive);
        }
        public void Delete(FastDrive fastDrive)
        {
            fastDriveRepository.Delete(fastDrive);
        }
        public void Update(FastDrive fastDrive)
        {
            fastDriveRepository.Update(fastDrive);
        }
        public void SaveDrive(Drive drive)
        {
            _driveRepository.Save(drive);
        }
        public void Subscribe(IObserver observer)
        {
            fastDriveRepository.Subscribe(observer);
        }
        public DateTime CreateDateTimeFromSelections(DateTime DepartureDate, string DepartureHour, string SelectedMinute)
        {
            int hour = int.Parse(DepartureHour);
            int minute = int.Parse(SelectedMinute);

            if (hour < 0 || hour > 23)
            {
                return DateTime.MinValue;
            }
            
            DateTime selectedDateTime = new DateTime(DepartureDate.Year, DepartureDate.Month, DepartureDate.Day, hour, minute, 0);
            return selectedDateTime;
        }
        public List<KeyValuePair<int, string>> GetCitiesByCountry(string country)
        {
            return _locationRepository.GetAll()
                                      .Where(location => location.Country == country)
                                      .Select(location => new KeyValuePair<int, string>(location.Id, location.City))
                                      .Distinct()
                                      .OrderBy(pair => pair.Value)
                                      .ToList();
        }
        public int AddNewAddress(string address, string selectedCity, string selectedCountry)
        {
            string[] parts = address.Split(',');

            if (parts.Length != 2)
            {
                MessageBox.Show("Neispravan format unosa. Molimo unesite ulicu i broj odvojene zarezom.");
                return 0;
            }

            string streetName = parts[0].Trim();
            string streetNumber = parts[1].Trim();

            Location selectedLocation = _locationRepository.GetLocationByCityAndCountry(selectedCity, selectedCountry);
            Address newAddress = new Address
            {
                Id = _addressRepository.NextId(),
                Location = selectedLocation,
                Street = streetName,
                Number = streetNumber
            };

            _addressRepository.Save(newAddress);

            return newAddress.Id;
        }

        public bool CheckDuration(double duration)
        {
            if (duration > 6)
            {
                return false;
            }
            return true;
        }

        /*public int GetAvailableDriver()
        {
            return _driveRepository.GetAvailableDriverId();
        }*/

        public void AddBonusPoints(int id)
        {
            var driverStats = _driverStatsRepository.GetByDriverId(id);

            DriverStatsUpdate update = new DriverStatsUpdate(id);
            if (driverStats != null)
            {
                if (driverStats.FastDrives <= 15)
                {
                    driverStats.FastDrives += 1;
                    update.FastDrivesUpdate += 1;
                    _driverStatsUpdateRepository.Create(update);
                }
                else
                {
                    driverStats.BonusPoints += 5;
                    update.BonusPointsUpdate += 5;
                    _driverStatsUpdateRepository.Create(update);
                }

                _driverStatsRepository.Update(driverStats);
            }
        }
        public Location GetLocationById(int id)
        {
            return _locationRepository.GetLocationById(id);
        }
    }
}
