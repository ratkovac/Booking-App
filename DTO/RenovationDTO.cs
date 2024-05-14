using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.DTO
{
    public class RenovationDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id { get; set; }

        private Accommodation accommodation;
        public Accommodation Accommodation
        {
            get
            {
                return accommodation;
            }
            set
            {
                if (value != accommodation)
                {
                    accommodation = value;
                    OnPropertyChanged("Accommodation");
                }
            }
        }

        private string accommodationName;
        public string AccommodationName
        {
            get
            {
                return accommodationName;
            }
            set
            {
                if (value != accommodationName)
                {
                    accommodationName = value;
                    OnPropertyChanged("AccommodationName");
                }
            }
        }

        private DateOnly startRenovationDate;
        public DateOnly StartRenovationDate
        {
            get
            {
                return startRenovationDate;
            }
            set
            {
                if (value != startRenovationDate)
                {
                    startRenovationDate = value;
                    OnPropertyChanged("StartRenovationDate");
                }
            }
        }
        private DateOnly endRenovationDate;
        public DateOnly EndRenovationDate
        {
            get
            {
                return endRenovationDate;
            }
            set
            {
                if (value != endRenovationDate)
                {
                    endRenovationDate = value;
                    OnPropertyChanged("EndRenovationDate");
                }
            }
        }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private string imageFrontPath;
        public string ImageFrontPath
        {
            get
            {
                return imageFrontPath;
            }
            set
            {
                if (value != imageFrontPath)
                {
                    imageFrontPath = value;
                    OnPropertyChanged("ImageFrontPath");
                }
            }
        }
        private string warning;
        public string Warning
        {
            get
            {
                return warning;
            }
            set
            {
                if (value != warning)
                {
                    warning = value;
                    OnPropertyChanged("Warning");
                }
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }
        public RenovationDTO()
        {
        }
        public RenovationDTO(Renovations renovations)
        {
            Id = renovations.Id;
            accommodation = renovations.Accommodation;
            startRenovationDate = renovations.StartDate;
            endRenovationDate = renovations.EndDate;
            description = renovations.Description;
        }

        public Renovations ToRenovation()
        {
            Renovations renovation = new Renovations(Id,accommodation, startRenovationDate, endRenovationDate, description);
            return renovation;
        }
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
