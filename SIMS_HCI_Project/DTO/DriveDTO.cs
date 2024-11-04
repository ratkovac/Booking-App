using BookingApp.Model;
using System;
using System.ComponentModel;

namespace BookingApp.DTO
{
    public class DriveDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        private User driver;
        public User Driver
        {
            get { return driver; }
            set
            {
                if (value != driver)
                {
                    driver = value;
                    OnPropertyChanged("Driver");
                }
            }
        }

        private Address startAddress;
        public Address StartAddress
        {
            get { return startAddress; }
            set
            {
                if (value != startAddress)
                {
                    startAddress = value;
                    OnPropertyChanged("StartAddress");
                }
            }
        }

        private Address endAddress;
        public Address EndAddress
        {
            get { return endAddress; }
            set
            {
                if (value != endAddress)
                {
                    endAddress = value;
                    OnPropertyChanged("EndAddress");
                }
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value != date)
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private User guest;
        public User Guest
        {
            get { return guest; }
            set
            {
                if (value != guest)
                {
                    guest = value;
                    OnPropertyChanged("Guest");
                }
            }
        }

        public DriveDTO(Drive drive)
        {
            Id = drive.Id;
            Driver = drive.Driver;
            StartAddress = drive.StartAddress;
            EndAddress = drive.EndAddress;
            Date = drive.Date;
            Guest = drive.Guest;
        }

        public DriveDTO()
        {
        }

        public Drive ToDrive()
        {
            Drive drive = new Drive
            {
                Id = Id,
                Driver = Driver,
                StartAddress = StartAddress,
                EndAddress = EndAddress,
                Date = Date,
                Guest = Guest
            };
            return drive;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
