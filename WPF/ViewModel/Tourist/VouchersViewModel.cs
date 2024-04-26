using BookingApp.Model;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class VouchersViewModel
    {
        public BookingApp.Model.Tourist Tourist { get; set; }
        public ObservableCollection<Voucher> ListVoucher { get; set; }

        private VoucherService voucherService;
        public VouchersViewModel(BookingApp.Model.Tourist t)
        {
            Tourist = t;
            voucherService = new VoucherService();
            ListVoucher = new ObservableCollection<Voucher>(voucherService.GetVouchersWithIds(Tourist.VoucherIds));
        }
    }
}
