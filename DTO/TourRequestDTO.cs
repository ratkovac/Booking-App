using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class TourRequestDTO
    {

        private readonly LocationRepository _locationRepository;
        private readonly LanguageRepository _languageRepository;

        public int Id { get; set; }

        private int _tourRequestId;
        public int TourRequestId
        {
            get { return _tourRequestId; }
            set
            {
                if (_tourRequestId != value)
                {
                    _tourRequestId = value;
                    OnPropertyChanged("TourRequestId");
                }
            }
        }

        private string _tourDescription { get; set; }
        public string TourDescription
        {
            get { return _tourDescription; }
            set
            {
                if (_tourDescription != value)
                {
                    _tourDescription = value;
                    OnPropertyChanged("TourDescription");
                }
            }
        }

        private int _locationId;
        public int LocationId
        {
            get { return _locationId; }
            set
            {
                if (_locationId != value)
                {
                    _locationId = value;
                    OnPropertyChanged("LocationId");
                }
            }
        }

        private int _languageId;
        public int LanguageId
        {
            get { return _languageId; }
            set
            {
                if (_languageId != value)
                {
                    _languageId = value;
                    OnPropertyChanged("LanguageId");
                }
            }
        }

        private int _capacity;
        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                    OnPropertyChanged("MaximumCapacity");
                }
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged("Location");
                }
            }
        }
        private string _language;
        public string Language
        {
            get { return _language; }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }
        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }
        private DateTime _dateAccepted;
        public DateTime DateAccepted
        {
            get { return _dateAccepted; }
            set
            {
                if (_dateAccepted != value)
                {
                    _dateAccepted = value;
                    OnPropertyChanged("DateAccepted");
                }
            }
        }
        private TourRequestStatus _isAccepted;
        public TourRequestStatus IsAccepted
        {
            get { return _isAccepted; }
            set
            {
                if (_isAccepted != value)
                {
                    _isAccepted = value;
                    OnPropertyChanged("IsAccepted");
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public TourRequestDTO()
        {
            Language = "not set";
            Location = "not set";
        }

        public TourRequestDTO(TourRequestSegment tourRequest)
        {
            _locationRepository = new LocationRepository();
            _languageRepository = new LanguageRepository();

            Id = tourRequest.Id;
            TourDescription = tourRequest.TourDescription;
            LocationId = tourRequest.LocationId;
            LanguageId = tourRequest.LanguageId;
            Capacity = tourRequest.Capacity;

            Language language = _languageRepository.GetLanguageById(LanguageId);
            Location location = _locationRepository.GetById(LocationId);

            Language = language.Name;
            Location = location.City + " " + location.Country;

            StartDate = tourRequest.StartDate;
            EndDate = tourRequest.EndDate;
            DateAccepted = tourRequest.DateAccepted;
            TourRequestId = tourRequest.TourRequestId;
        }
        public TourRequestSegment ToRequest()
        {
            return new TourRequestSegment(TourRequestId, TourDescription, LocationId, LanguageId, Capacity, StartDate, EndDate);
        }



    }
}
