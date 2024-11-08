﻿using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class GradeAccommodationDTO : INotifyPropertyChanged
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

        private String username;
        public String Username
        {
            get
            {
                return username;
            }
            set
            {
                if (value != username)
                {
                    username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        private String accommodationName;
        public String AccommodationName
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
                    OnPropertyChanged("Accommodation Name");
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

        private int correctness;
        public int Correctness
        {
            get
            {
                return correctness;
            }
            set
            {
                if (value != correctness)
                {
                    correctness = value;
                    OnPropertyChanged("Correctness");
                }
            }
        }
        private double overallGrade;
        public double OverallGrade
        {
            get
            {
                return overallGrade;
            }
            set
            {
                if (value != overallGrade)
                {
                    overallGrade = value;
                    OnPropertyChanged("OverallGrade");
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
        private string frontImagePath;
        public string FrontImagePath
        {
            get
            {
                return frontImagePath;
            }
            set
            {
                if (frontImagePath != value)
                {
                    frontImagePath = value;
                    OnPropertyChanged("FrontImagePath");
                }
            }
        }

        private string suggest;

        public string Suggest
        {
            get { return suggest; }
            set
            {
                if (suggest != value)
                {
                    suggest = value;
                    OnPropertyChanged(nameof(Suggest));
                }
            }
        }

        private RenovationUrgencyLevel urgencyLevel;
        public RenovationUrgencyLevel UrgencyLevel
        {
            get { return urgencyLevel; }
            set
            {
                if (urgencyLevel != value)
                {
                    urgencyLevel = value;
                    OnPropertyChanged(nameof(UrgencyLevel));
                }
            }
        }
        public GradeAccommodationDTO()
        {
        }
        public GradeAccommodationDTO(GradeAccommodation gradeAccommodation)
        {
            Id = gradeAccommodation.Id;
            accommodationReservation = gradeAccommodation.AccommodationReservation;
            cleanliness = gradeAccommodation.Cleanliness;
            correctness = gradeAccommodation.Correctness;
            comment = gradeAccommodation.Comment;
            username = gradeAccommodation.AccommodationReservation.User.Username;
            accommodationName = gradeAccommodation.AccommodationReservation.Accommodation.Name;
            suggest = gradeAccommodation.Suggest;
            urgencyLevel = gradeAccommodation.UrgencyLevel ?? RenovationUrgencyLevel.Empty; 


        }
        public GradeAccommodation ToGradeAccommodation()
        {
            GradeAccommodation gradeAccommodation = new GradeAccommodation(accommodationReservation, cleanliness, correctness, comment);
            return gradeAccommodation;
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
