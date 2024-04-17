using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourInstanceViewModel
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private bool isFinished;

        public bool IsFinished
        {
            get => isFinished;
            set
            {
                if (isFinished != value)
                {
                    isFinished = value;
                    OnPropertyChanged(nameof(IsFinished));
                }
            }
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private List<string> checkpointNames = new List<string>();
        public List<string> CheckpointNames
        {
            get => checkpointNames;
            set
            {
                if (checkpointNames != value)
                {
                    checkpointNames = value;
                    OnPropertyChanged(nameof(CheckpointNames));
                }
            }
        }

        private string currentCheckpoint;

        public string CurrentCheckpoint
        {
            get => currentCheckpoint;
            set
            {
                if(currentCheckpoint != value)
                {
                    currentCheckpoint = value;
                    OnPropertyChanged(nameof(CurrentCheckpoint));
                }
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        private List<TourGuest> guests = new List<TourGuest>();
        public List<TourGuest> Guests
        {
            get => guests;
            set
            {
                if (guests != value)
                {
                    guests = value;
                    OnPropertyChanged(nameof(Guests));
                }
            }
        }

        public TourInstanceViewModel()
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
