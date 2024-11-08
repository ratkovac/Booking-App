﻿using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.View;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class TouristService
    {

        private ITouristRepository touristRepository;
        private IVoucherRepository voucherRepository;
        private UserRepository userRepository;

        public TouristService()
        {
            touristRepository = Injector.CreateInstance<ITouristRepository>();
            voucherRepository = Injector.CreateInstance<IVoucherRepository>();
            userRepository = new UserRepository();
            InitializeUser();
        }
        public Tourist GetTouristByUserId(int userId)
        {
            return touristRepository.GetByUserId(userId);
        }
        private void InitializeUser()
        {
            foreach (var item in touristRepository.GetAll())
            {
                item.User = userRepository.GetByID(item.UserId);
            }
        }
        public int NextId()
        {
            return touristRepository.NextId();
        }
        public List<Tourist> GetAll()
        {
            return touristRepository.GetAll();
        }
        public Tourist GetById(int id)
        {
            return touristRepository.GetById(id);
        }
        public void Create(Tourist tourist)
        {
            touristRepository.Create(tourist);
        }
        public void Delete(Tourist tourist)
        {
            touristRepository.Delete(tourist);
        }
        public void Update(Tourist tourist)
        {
            touristRepository.Update(tourist);
        }
        public void Subscribe(IObserver observer)
        {
            touristRepository.Subscribe(observer);
        }
        public void GiveVoucher(int id, int years)
        {
            Tourist tourist = touristRepository.GetById(id);
            Voucher voucher = new Voucher(voucherRepository.NextId(), DateTime.Now, DateTime.Now.AddYears(years), false, true);
            voucherRepository.Create(voucher);
            tourist.VoucherIds.Add(voucher.Id);
            Update(tourist);
        }
        public void GiveVoucherForGuestWhenFiveTimePresent(int id)
        {
            Tourist tourist = touristRepository.GetById(id);
            Voucher voucher = new Voucher(voucherRepository.NextId(), DateTime.Now, DateTime.Now.AddMonths(6), false, true);
            voucherRepository.Create(voucher);
            tourist.VoucherIds.Add(voucher.Id);
            Update(tourist);
        }
    }
}
