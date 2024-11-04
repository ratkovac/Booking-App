using BookingApp.Model;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface ILanguageRepository : IGenericRepository<Language, int>
    {

        public int ExistsLanguage(string name);
        public Language GetLanguageByName(string name);

        public void Subscribe(IObserver observer)
        {

        }
    }
}
