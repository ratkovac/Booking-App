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
    /// <summary>
    /// Interaction logic for GradeDialog.xaml
    /// </summary>
    public partial class GradeDialog : Window
    {
        public string SelectedGrade { get; private set; }

        public GradeDialog()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (GradeComboBox.SelectedItem != null)
            {
                SelectedGrade = (GradeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Morate odabrati ocjenu.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public string GetTourGrade()
        {
            return SelectedGrade;
        }
    }
}
