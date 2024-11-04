using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface IFastDriveRepository : IGenericRepository<FastDrive, int>
    {
        List<FastDrive> GetDrivesByLocations(ObservableCollection<int> locations);

        public void Subscribe(IObserver observer)
        {

        }
    }
}
