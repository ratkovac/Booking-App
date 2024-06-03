using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class DriveDisplay : Page
    {
        public DriveDisplay(DriveDisplayViewModel driveDisplayViewModel)
        {
            InitializeComponent();
            this.DataContext = driveDisplayViewModel;
        }
    }
}
