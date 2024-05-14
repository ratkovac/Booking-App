using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.View.Driver;
using BookingApp.ViewModel.Driver;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.View.Driver
{
    public partial class DrivesWindow : Window
    {
        public readonly DrivesViewModel _viewModel;

        public DrivesWindow(User user, bool isFastDriver)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            _viewModel = new DrivesViewModel(user);
            DataContext = _viewModel;
            this.PreviewKeyDown += DrivesWindow_PreviewKeyDown;
        }

        private void DrivesWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.H && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                btnHelp_Click(null, null);
            }
            if (e.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                btnBack_Click(null, null);
            }
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                btnDriveReservation_Click(null, null);
            }
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                btnCancelDrive_Click(null, null);
            }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To navigate yourself, use TAB key.\nTo point on a drive, use Arrow keys.\nTo switch to the lower menu, use CTRL+TAB.\nTo close Window, use ALT+F4.", "Help");
        }

        private void btnCancelDrive_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CancelDrive(dataGrid.SelectedItem as DriveDTO);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Driver.Example example = new Driver.Example(_viewModel.LoggedInUser);
            example.Show();
            Close();
        }

        private void btnDriveReservation_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DriveDTO selectedDrive = dataGrid.SelectedItem as DriveDTO;
                DriveReservationWindow reservationWindow = new DriveReservationWindow(selectedDrive, this, false);

                reservationWindow.Owner = this;
                _viewModel.IsOverlayVisible = true;
                reservationWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a drive first.");
            }
        }

        public void MakeVisible()
        {
            _viewModel.IsOverlayVisible = false;
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth - windowWidth) / 2;
            Top = (screenHeight - windowHeight) / 2;
        }
    }
}
