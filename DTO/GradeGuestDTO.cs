using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class GradeGuestDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public int Id { get; set; }
        
        private AccommodationReservation accommodationReservation;
        public AccommodationReservation AccommodationReservation
        {
            get
            {
                return accommodationReservation;
            }
            set
            {
                if (value != accommodationReservation)
                {
                    accommodationReservation = value;
                    OnPropertyChanged("AccommodationReservation");
                }
            }
        }

        private int cleanliness;
        public int Cleanliness
        {
            get
            {
                return cleanliness;
            }
            set
            {
                if (value != cleanliness)
                {
                    cleanliness = value;
                    OnPropertyChanged("Cleanliness");
                }
            }
        }

        private int rulesFollowing;
        public int RulesFollowing
        {
            get
            {
                return rulesFollowing;
            }
            set
            {
                if (value != rulesFollowing)
                {
                    rulesFollowing = value;
                    OnPropertyChanged("RulesFollowing");
                }
            }
        }

        private string comment;
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }
        public GradeGuestDTO()
        {
        }
        public GradeGuestDTO(GradeGuest gradeGuest)
        {
            Id = gradeGuest.Id;
            accommodationReservation = gradeGuest.AccommodationReservation;
            cleanliness = gradeGuest.Cleanliness;
            rulesFollowing = gradeGuest.RulesFollowing;
            comment = gradeGuest.Comment;

        }
        public GradeGuest ToGradeGuest()
        {
            GradeGuest gradeGuest = new GradeGuest(Id, accommodationReservation, cleanliness, rulesFollowing, comment);
            return gradeGuest;
        }
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
