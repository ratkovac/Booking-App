using System.Windows;
using BookingApp.View.ViewModel.Owner;

namespace BookingApp.View.Owner
{
    public partial class AccommodationsGrades : Window
    {
        public AccommodationsGrades(AccommodationsGradesViewModel accommodationsGradesViewModel)
        {
            InitializeComponent();
            this.DataContext = accommodationsGradesViewModel;
            Average.Content = accommodationsGradesViewModel.AvarageGrade();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
