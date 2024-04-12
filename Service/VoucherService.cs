using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class VoucherService
    {

        private IVoucherRepository voucherRepository;
        public VoucherService()
        {
            voucherRepository = Injector.CreateInstance<IVoucherRepository>();
        }
        public int NextId()
        {
            return voucherRepository.NextId();
        }
        public List<Voucher> GetAllVouchers()
        {
            return voucherRepository.GetAll();
        }
        public Voucher GetVoucherById(int id)
        {
            return voucherRepository.GetById(id);
        }
        public List<Voucher> GetVouchersWithIds(List<int> ids)
        {
            return voucherRepository.GetWithIds(ids);
        }
        public List<Voucher> GetActiveVouchersWithIds(List<int> ids)
        {
            return voucherRepository.GetActiveVouchers(ids);
        }
        public void Create(Voucher voucher)
        {
            voucherRepository.Create(voucher);
        }
        public void Delete(Voucher voucher)
        {
            voucherRepository.Delete(voucher);
        }
        public void Update(Voucher voucher)
        {
            voucherRepository.Update(voucher);
        }
        public void Subscribe(IObserver observer)
        {
            voucherRepository.Subscribe(observer);
        }

        public void UpdateValidVouchers()
        {
            List<Voucher> vouchers = new List<Voucher>(GetAllVouchers());
            foreach(Voucher voucher in vouchers)
            {
                if (voucher.ExpirationDate < DateTime.Now && voucher.ValidVoucher == true)
                {
                    voucher.ValidVoucher = false;
                    Update(voucher);
                }
            }
        }

        public List<Voucher> GetValidVouchers(Tourist tourist)
        {
            List<Voucher> vouchers = new List<Voucher>();
            foreach(Voucher voucher in GetVouchersWithIds(tourist.VoucherIds))
            {
                if(voucher.ValidVoucher == true)
                {
                    vouchers.Add(voucher);
                }
            }
            return vouchers;
        }
    }
}
