﻿using BookingApp.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.Repository
{
    public class SuccessfulDrivesRepository
    {
        private const string FilePath = "../../../Resources/Data/successfulDrives.csv";
        private readonly Serializer<Drive> _serializer;
        private List<Drive> _successfulDrives;

        public SuccessfulDrivesRepository()
        {
            _serializer = new Serializer<Drive>();
            _successfulDrives = _serializer.FromCSV(FilePath);
        }

        public List<Drive> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(Drive drive)
        {
            _successfulDrives = _serializer.FromCSV(FilePath);
            _successfulDrives.Add(drive);
            _serializer.ToCSV(FilePath, _successfulDrives);
        }

        public void Delete(Drive drive)
        {
            _successfulDrives = _serializer.FromCSV(FilePath);
            Drive founded = _successfulDrives.Find(c => c.Id == drive.Id);
            if (founded != null)
            {
                _successfulDrives.Remove(founded);
            }
            _serializer.ToCSV(FilePath, _successfulDrives);
        }
        public List<int> GetDriveIdsByMonthAndYear(int month, int year, int driverId)
        {
            var drivesInMonth = _successfulDrives
                .Where(drive => drive.Date.Month == month && drive.Date.Year == year && drive.DriverId == driverId)
                .Select(drive => drive.Id);

            return new List<int>(drivesInMonth);
        }
        public int GetNumberOfDrivesByMonthAndYear(int month, int year, int driverId)
        {
            var drivesInMonth = _successfulDrives
                .Count(drive => drive.Date.Month == month && drive.Date.Year == year && drive.DriverId == driverId);

            return drivesInMonth;
        }
        public List<string> GetYears()
        {
            var years = _successfulDrives
                .Select(drive => drive.Date.Year.ToString()) 
                .Distinct() 
                .OrderBy(year => year) 
                .ToList();

            return new List<string>(years);
        }
        public int GetNumberOfDrivesByYear(int year, int driverId)
        {
            int numberOfDrives = _successfulDrives
                .Count(drive => drive.Date.Year == year && drive.DriverId == driverId);

            return numberOfDrives;
        }
        public List<int> GetDriveIdsByYear(int year, int driverId)
        {
            var driveIds = _successfulDrives
                .Where(drive => drive.Date.Year == year && drive.DriverId == driverId)
                .Select(drive => drive.Id)
                .ToList();

            return driveIds;
        }
    }
}
