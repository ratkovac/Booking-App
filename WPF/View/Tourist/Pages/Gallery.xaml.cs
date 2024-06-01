using BookingApp.Model;
using BookingApp.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class Gallery : Page, INotifyPropertyChanged
    {
        public Tour Tour { get; set; }
        public List<BookingApp.Model.Image> Images { get; set; }
        private ImageService _imageService { get; set; }
        private int CurrentImageIndex;

        public Gallery(Tour tour)
        {
            InitializeComponent();
            DataContext = this;
            Tour = tour;
            _imageService = new ImageService();
            Images = _imageService.GetByTourId(Tour.Id);
            CurrentImageIndex = 0;
            LoadCurrentImage();
        }

        private void LoadCurrentImage()
        {
            if (Images != null && Images.Count > 0)
            {
                CurrentImageSource = Images[CurrentImageIndex].Path;
            }
        }

        private string _currentImageSource;
        public string CurrentImageSource
        {
            get { return _currentImageSource; }
            set
            {
                _currentImageSource = value;
                OnPropertyChanged("CurrentImageSource");
            }
        }

        public void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (Images != null && Images.Count > 0)
            {
                CurrentImageIndex = (CurrentImageIndex + 1) % Images.Count;
                LoadCurrentImage();
            }
        }

        public void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (Images != null && Images.Count > 0)
            {
                CurrentImageIndex = (CurrentImageIndex - 1 + Images.Count) % Images.Count;
                LoadCurrentImage();
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
