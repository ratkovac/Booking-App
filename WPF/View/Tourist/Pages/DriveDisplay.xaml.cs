using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class DriveDisplay : Page, INotifyPropertyChanged
    {

        private DriveRepository driveRepository;
        public ObservableCollection<Drive> ListDrive { get; set; }
        public Drive SelectedTour { get; set; }
        public BookingApp.Model.Tourist Tourist { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DriveDisplay(BookingApp.Model.Tourist tourist)
        {
            InitializeComponent();
            DataContext = this;
            Tourist = tourist;

            driveRepository = new DriveRepository();
            ListDrive = new ObservableCollection<Drive>(driveRepository.GetAll());
        }

        private void UpdateDriveList()
        {
            ListDrive.Clear();
            foreach (var drive in driveRepository.GetAll())
            {
                ListDrive.Add(drive);
            }
        }

        public void Update()
        {
            UpdateDriveList();
        }
    }
}
