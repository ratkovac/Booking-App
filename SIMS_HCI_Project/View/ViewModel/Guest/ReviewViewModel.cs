using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DTO;
using BookingApp.Service;
using System.ComponentModel;
using BookingApp.Model;

namespace BookingApp.View.ViewModel.Guest
{
    public class ReviewViewModel : IObserver
    {
        private GradeGuestManagmentService gradeGuestManagmentService;
        public  ObservableCollection<GradeGuestDTO> GuestGrades { get; set; }
        private int userId;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ReviewViewModel(int userId)
        {
            gradeGuestManagmentService = new GradeGuestManagmentService();
            GuestGrades = new ObservableCollection<GradeGuestDTO>();
            this.userId = userId;
            Update();
        }

        public void Update()
        {
            foreach (GradeGuest gradeGuest in gradeGuestManagmentService.GenerateGuestScores(userId))
            {
                GuestGrades.Add(new GradeGuestDTO(gradeGuest));
            }
        }
    }
}
