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
    public partial class AddGuestsView : Window
    {
        public int Age { get; private set; }
        public string Name { get; set; }
        public string Lastname { get; set; }

        public AddGuestsView(int personNumber)
        {
            InitializeComponent();
            PersonLabel.Text = $"{personNumber}.";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AgeTextBox.Text))
            {
                if (App.CurrentLanguage == "en-US")
                {
                    MessageBox.Show("Please enter all fields.");
                }
                else
                {
                    MessageBox.Show("Molimo unesite sva polja.");
                }
                return;
            }

            if (int.TryParse(AgeTextBox.Text, out int age))
            {
                Age = age;
            }
            else
            {
                Age = 0;
            }

            Name = NameTextBox.Text;
            Lastname = LastnameTextBox.Text;

            DialogResult = true;
            Close();
        }
    }

}
