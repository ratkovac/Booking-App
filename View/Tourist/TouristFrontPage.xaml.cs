using BookingApp.Model;
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
using System.Windows.Shapes;

namespace BookingApp.View.Tourist
{
    public partial class TouristFrontPage : Window
    {
        public User LoggedInUser { get; set; }
        public TouristFrontPage(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
        }
    }
}
