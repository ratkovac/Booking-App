using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.View.ViewModel.Tourist
{
    public class UseVoucherViewModel
    {
        public BookingApp.Model.Tourist tourist { get; set; }
        public TourInstance tourInstance { get; set; }
        public int numberGuests { get; set; }
        public Voucher SelectedVoucher { get; set; }
        public ObservableCollection<Voucher> ListVoucher { get; set; }

        private VoucherService voucherService;

        private TourInstanceService tourInstanceService;

        private TourReservationRepository tourReservationRepository;
        private TouristService touristService;

        public UseVoucherViewModel(BookingApp.Model.Tourist t, TourInstance ti, int numGuests)
        {
            tourist = t;
            tourInstance = ti;
            numberGuests = numGuests;
            voucherService = new VoucherService();
            tourInstanceService = new TourInstanceService();
            tourReservationRepository = new TourReservationRepository();
            touristService = new TouristService();
            ListVoucher = new ObservableCollection<Voucher>(voucherService.GetActiveVouchersWithIds(tourist.VoucherIds));

        }

        public string Reservation()
        {
            TourReservation reservation = new TourReservation();
            int guestAgeOnTour = touristService.GetAgeOnTour(tourist, tourInstance);
            if (SelectedVoucher != null)
            {
                SelectedVoucher.Used = true;
                SelectedVoucher.ValidVoucher = false;
                voucherService.Update(SelectedVoucher);
                //reservation = new TourReservation(tourInstance.Id, numberGuests, tourist.Id, -1, true, false, guestAgeOnTour);
                reservation = new TourReservation(tourInstance.Id, tourist.Id);
            }
            else
            {
                reservation = new TourReservation(tourInstance.Id, tourist.Id);
            }
            tourReservationRepository.Save(reservation);
            tourInstance.AvailableSlots -= numberGuests;
            tourInstanceService.Update(tourInstance);
            return "Rezervacija uspjesna! \nKorisnik " + tourist.Name + " " + tourist.LastName +
                " je izvrsio rezervaciju za " + numberGuests.ToString() + ". ljudi na turi: " + tourInstance.Tour.Name + ".";

        }

    }
}
