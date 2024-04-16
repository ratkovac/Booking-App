using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Model;
using BookingApp.Service;
using CLI.Observer;

namespace BookingApp.View.ViewModel
{
    public class RateViewModel : IObserver
    {

        public ObservableCollection<Image> Images {get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        private GradeAccommodationService gradeAccommodationService;
        private ImageService imageService;
        private AccommodationService accommodationService;


        private int cleanliness;
        public int Cleanliness
        {
            get { return cleanliness; }
            set
            {
                if (value != cleanliness)
                {
                    cleanliness = value;
                    OnPropertyChanged("Cleanliness");
                }
            }
        }

        private int ownerCorrectness;
        public int OwnerCorrectness
        {
            get { return ownerCorrectness; }
            set
            {
                if (value != ownerCorrectness)
                {
                    ownerCorrectness = value;
                    OnPropertyChanged("OwnerCorrectness");
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }


        public RateViewModel(AccommodationReservation accommodationReservation)
        {
            Images = new ObservableCollection<Image>();
            imageService = new ImageService();
            gradeAccommodationService = new GradeAccommodationService();
            accommodationService = new AccommodationService();
            gradeAccommodationService.Subscribe(this);
            AccommodationReservation = accommodationReservation;
            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void EnterPictures()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    Image image = new Image(filename,AccommodationReservation.Accommodation.Id, -1);
                    Images.Add(image);
                    imageService.Create(image);
                }
            }
        }

        public void AddGrade()
        {
            GradeAccommodation gradeAccommodation = new GradeAccommodation(AccommodationReservation.Id, Cleanliness,
                OwnerCorrectness, Comment, Images.ToList());
            gradeAccommodationService.Create(gradeAccommodation);
            MessageBox.Show("Request successfully rated the owner.");
        }

        public void Update()
        {

        }
    }
}
