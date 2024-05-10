﻿using BookingApp.DependencyInjection;
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
    public class GroupDriveService
    {
        private IGroupDriveRepository groupDriveRepository;
        private LocationRepository _locationRepository { get; set; }
        private AddressRepository _addressRepository { get; set; }
        public GroupDriveService()
        {
            groupDriveRepository = Injector.CreateInstance<IGroupDriveRepository>();
            _locationRepository = new LocationRepository();
            _addressRepository = new AddressRepository();
        }
        public int NextId()
        {
            return groupDriveRepository.NextId();
        }
        public List<GroupDrive> GetAllGroupDrives()
        {
            return groupDriveRepository.GetAll();
        }
        public GroupDrive GetGroupDriveById(int id)
        {
            return groupDriveRepository.GetById(id);
        }
        public void Create(GroupDrive groupDrive)
        {
            groupDriveRepository.Create(groupDrive);
        }
        public void Delete(GroupDrive groupDrive)
        {
            groupDriveRepository.Delete(groupDrive);
        }
        public void Update(GroupDrive groupDrive)
        {
            groupDriveRepository.Update(groupDrive);
        }
        public void Subscribe(IObserver observer)
        {
            groupDriveRepository.Subscribe(observer);
        }
        public DateTime CreateDateTimeFromSelections(DateTime DepartureDate, string DepartureHour, string SelectedMinute)
        {
            int hour = int.Parse(DepartureHour);
            int minute = int.Parse(SelectedMinute);

            if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
            {
                throw new ArgumentException("Nevažeći sati ili minuti.");
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
    }
}
