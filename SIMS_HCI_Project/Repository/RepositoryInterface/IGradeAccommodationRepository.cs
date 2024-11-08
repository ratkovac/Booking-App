﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Model;
using CLI.Observer;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface IGradeAccommodationRepository : IGenericRepository<GradeAccommodation, int>
    {
        public List<GradeAccommodation> GetAllByUser(int userId);
        public void Subscribe(IObserver observer)
        {

        }
    }
}
