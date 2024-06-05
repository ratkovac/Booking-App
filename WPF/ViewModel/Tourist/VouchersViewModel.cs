using BookingApp.Model;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class VouchersViewModel : INotifyPropertyChanged
    {
        public BookingApp.Model.Tourist Tourist { get; set; }
        public ObservableCollection<Voucher> ListVoucher { get; set; }
        public TourReservationService _tourReservationService { get; set; }
        public TouristService _touristService { get; set; }
        private VoucherService voucherService;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VouchersViewModel(BookingApp.Model.Tourist t)
        {
            Tourist = t;
            voucherService = new VoucherService();
            _tourReservationService = new TourReservationService();
            _touristService = new TouristService();
            ListVoucher = new ObservableCollection<Voucher>(voucherService.GetVouchersWithIds(Tourist.VoucherIds));
            CheckTouristReservation();
        }

        public void CheckTouristReservation()
        {
            int number = Tourist.NumberOfToursAttended;
            List<BookingApp.Model.TourReservation> reservations = new List<BookingApp.Model.TourReservation>();
            foreach (BookingApp.Model.TourReservation reservation in _tourReservationService.GetReservationsForGuest(Tourist.Id))
            {
                if (reservation.State == TouristState.Present && reservation.TourInstance.StartTime > DateTime.Now.AddYears(-1) && !reservation.WonVoucher)
                {
                    number++;
                    reservations.Add(reservation);
                    if (number % 5 == 0)
                    {
                        _touristService.GiveVoucherForGuestWhenFiveTimePresent(Tourist.Id);
                        if (App.CurrentLanguage == "en-US")
                        {
                            MessageBox.Show("Congratulations! You have won a voucher!");
                        }
                        else
                        {
                            MessageBox.Show("Čestitamo! Osvojili ste vaučer!");
                        }
                        Tourist.NumberOfToursAttended = 0;
                        foreach (BookingApp.Model.TourReservation res in reservations)
                        {
                            _tourReservationService.UsedForWinningVoucher(res);
                        }
                        UpdateVouchers();
                    }
                }
            }
        }

        public void UpdateVouchers()
        {
            ListVoucher = new ObservableCollection<Voucher>(voucherService.GetVouchersWithIds(Tourist.VoucherIds));
            OnPropertyChanged(nameof(ListVoucher));
        }
    }
}
