using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.RepositoryInterface
{
    public interface IGenericRepository<T, ID>
    {
        List<T> GetAll();
        public int NextId();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(ID key);
    }
}
