using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class GradeTourFormViewModel : INotifyPropertyChanged
    {
        private int _selectedRating;
        public TourGuest Guest { get; set; }
        public string Comment { get; set; }
        public ObservableCollection<string> ImagePaths { get; set; } = new ObservableCollection<string>();

        public List<int> Ratings { get; } = new List<int> { 1, 2, 3, 4, 5 };

        public int SelectedRating
        {
            get => _selectedRating;
            set
            {
                if (_selectedRating != value)
                {
                    _selectedRating = value;
                    OnPropertyChanged(nameof(SelectedRating));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
