﻿using BookingApp.Model;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BookingApp.Repository.RepositoryInterface;

namespace BookingApp.Repository
{
    public class TourRepository : ITourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<Tour> _serializer;

        private List<Tour> _tours;
        public Subject TourSubject;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
            TourSubject = new Subject();
        }

        public List<Tour> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tour);
            TourSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }
        public void Create(Tour tour)
        {
            tour.Id = NextId();
            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tour);
            TourSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _tours);
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(t => t.Id) + 1;
        }

        public void Delete(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour founded = _tours.Find(t => t.Id == tour.Id);
            if (founded != null)
            {
                _tours.Remove(founded);
            }
            TourSubject.NotifyObservers();
            _serializer.ToCSV(FilePath, _tours);

        }

        public void Update(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour current = _tours.Find(t => t.Id == tour.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tour); 
            _serializer.ToCSV(FilePath, _tours);
            TourSubject.NotifyObservers();
        }

        public List<Tour> GetToursForToday()
        {
            DateTime today = DateTime.Now.Date;
            string formattedToday = today.ToString("M/d/yyyy"); 

            List<int> tourIds = GetTourIdsForToday(formattedToday);
            return _tours.Where(t => tourIds.Contains(t.Id)).ToList();
        }

        private List<int> GetTourIdsForToday(string formattedToday)
        {
            string[] lines = File.ReadAllLines("../../../Resources/Data/dateRealizations.csv");
            List<int> tourIds = new List<int>();

            foreach (string line in lines) 
            {
                string[] values = line.Split('|');
                string datePart = values[1].Split(' ')[0]; 

                if (datePart == formattedToday)
                {
                    int tourId = Convert.ToInt32(values[2]);
                    tourIds.Add(tourId);
                }
            }

            return tourIds;
        }

        public List<Tour> GetToursByLocationId(int locationId)
        {
            _tours = _serializer.FromCSV(FilePath);
            return _tours.Where(t => t.Location.Id == locationId).ToList();
        }

        public Tour GetById(int id)
        {
            return _tours.Find(t => t.Id == id);
        }

        public void Subscribe(IObserver observer)
        {

        }
    }
}
