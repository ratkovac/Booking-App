using BookingApp.View.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.View.Owner
{
    public partial class AllRenovations : Window
    {
        public AllRenovations(AllRenovationsViewModel allRenovationsViewModel)
        {
            InitializeComponent();
            this.DataContext = allRenovationsViewModel;
        }
    }
}
