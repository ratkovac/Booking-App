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

namespace BookingApp.WPF.View.Tourist.Pages
{
    public partial class TouristDelay : Window
    {
        public int DelayMinutes { get; private set; }

        public TouristDelay()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            int delay;
            if (int.TryParse(DelayTextBox.Text, out delay))
            {
                DelayMinutes = delay;
            }
            else
            {
                DelayMinutes = 0;
            }
            Close();
        }
    }
}
