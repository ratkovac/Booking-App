﻿using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using BookingApp.Repository;

namespace BookingApp.Domain.RepositoryInterface
{
    public interface IGradeTourRepository : IGenericRepository<GradeTour, int>
    {
        public List<GradeTour> GetAllRatingsByTour(TourReservation tourReservation);
        public bool HasTouristGradedTour(int userId, int tourInstanceId);

        public void Subscribe(IObserver observer)
        {

        }
    }
}
