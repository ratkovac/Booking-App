using BookingApp.WPF.ViewModel.Tourist;
using System.Windows.Controls;

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class ComplexTourRequest : Page
    {
        public ComplexTourRequest(ComplexTourRequestViewModel complexTourRequestViewModel)
        {
            InitializeComponent();
            this.DataContext = complexTourRequestViewModel;
        }
    }
}
