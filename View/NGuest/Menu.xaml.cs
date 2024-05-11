using System;
using System.Collections.Generic;
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
using BookingApp.View.ViewModel.Guest;

namespace BookingApp.View.NGuest
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        private MenuViewModel menuViewModel;
        public Menu(MenuViewModel menuViewModel)
        {
            this.DataContext = menuViewModel;
            this.menuViewModel = menuViewModel;
            InitializeComponent();
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            menuViewModel.Back_OnClick();
        }

        private void OnClick_Rate(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DeleteMessage(object sender, RoutedEventArgs e)
        {
            menuViewModel.DeleteMessage(sender,e);
        }
    }
}
