﻿using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface IReservedGroupDriveRepository : IGenericRepository<GroupDrive, int>
    {

        public void Subscribe(IObserver observer)
        {

        }
    }
}
