using BookingApp.Model;
using BookingApp.Domain.RepositoryInterface;
using BookingApp.Serializer;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    internal class VoucherRepository : IVoucherRepository
    {
        private const string FilePath = "../../../Resources/Data/vouchers.csv";

        private readonly Serializer<Voucher> _serializer;
        private List<IObserver> observers;
        private List<Voucher> _vouchers;

        public VoucherRepository()
        {
            _serializer = new Serializer<Voucher>();
            _vouchers = _serializer.FromCSV(FilePath);
        }

        public Voucher Save(Voucher voucher)
        {
            voucher.Id = NextId();
            _vouchers.Add(voucher);
            _serializer.ToCSV(FilePath, _vouchers);
            return voucher;
        }

        public int NextId()
        {
            if (_vouchers.Count == 0)
            {
                return 0;
            }
            return _vouchers.Max(v => v.Id) + 1;
        }

        public void Create(Voucher voucher)
        {
            voucher.Id = NextId();
            _vouchers.Add(voucher);
            _serializer.ToCSV(FilePath, _vouchers);
        }

        public void Delete(Voucher voucher)
        {
            Voucher found = _vouchers.Find(v => v.Id == voucher.Id);
            _vouchers.Remove(found);
            _serializer.ToCSV(FilePath, _vouchers);
        }

        public void Update(Voucher voucher)
        {
            int index = _vouchers.FindIndex(v => voucher.Id == v.Id);
            if (index != -1)
            {
                _vouchers[index] = voucher;
                _serializer.ToCSV(FilePath, _vouchers);
            }
        }

        public List<Voucher> GetAll()
        {
            return _vouchers;
        }

        public Voucher GetById(int id)
        {
            return _vouchers.Find(v => v.Id == id);
        }

        public List<Voucher> GetWithIds(List<int> ids)
        {
            List<Voucher> vouchers = new List<Voucher>();
            foreach (int id in ids)
            {
                vouchers.Add(GetById(id));
            }
            return vouchers;
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }
    }
}
