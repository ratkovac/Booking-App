using BookingApp.Model;
using System;
using System.ComponentModel;

namespace BookingApp.DTO
{
    public class DriveDrivenDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int DriveId { get; set; }

        private TimeSpan duration;
        public TimeSpan Duration
        {
            get { return duration; }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }

        private int price;
        public int Price
        {
            get { return price; }
            set
            {
                if (value != price)
                {
                    price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        public DriveDrivenDTO(DriveDriven driveDriven)
        {
            DriveId = driveDriven.DriveId;
            Duration = driveDriven.Duration;
            Price = driveDriven.Price;
        }

        public DriveDrivenDTO()
        {
        }

        public DriveDriven ToDriveDriven()
        {
            DriveDriven driveDriven = new DriveDriven
            {
                DriveId = DriveId,
                Duration = Duration,
                Price = Price
            };
            return driveDriven;
        }

        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
