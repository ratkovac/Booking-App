﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class DrivesDrivenRepository
    {
        private const string FilePath = "../../../Resources/Data/drivesDriven.csv";

        private readonly Serializer<DriveDriven> _serializer;

        private List<DriveDriven> _driveDrivens;

        public DrivesDrivenRepository()
        {
            _serializer = new Serializer<DriveDriven>();
            _driveDrivens = _serializer.FromCSV(FilePath);
        }

        public List<DriveDriven> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public DriveDriven Save(DriveDriven driveDriven)
        {
            _driveDrivens = _serializer.FromCSV(FilePath);
            _driveDrivens.Add(driveDriven);
            _serializer.ToCSV(FilePath, _driveDrivens);
            return driveDriven;
        }


        public void Delete(DriveDriven driveDriven)
        {
            _driveDrivens = _serializer.FromCSV(FilePath);
            DriveDriven founded = _driveDrivens.Find(c => c.DriveId == driveDriven.DriveId);
            if (founded != null)
            {
                _driveDrivens.Remove(founded);
            }
            _serializer.ToCSV(FilePath, _driveDrivens);
        }

        public DriveDriven Update(DriveDriven driveDriven)
        {
            _driveDrivens = _serializer.FromCSV(FilePath);
            DriveDriven current = _driveDrivens.Find(c => c.DriveId == driveDriven.DriveId);
            int index = _driveDrivens.IndexOf(current);
            _driveDrivens.Remove(current);
            _driveDrivens.Insert(index, driveDriven);
            _serializer.ToCSV(FilePath, _driveDrivens);
            return driveDriven;
        }
    }
}
