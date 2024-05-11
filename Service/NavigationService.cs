using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Views;

namespace BookingApp.Service
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateTo(Page page)
        {
            if (_frame != null)
            {
                _frame.Navigate(page);
            }
            else
            {
                MessageBox.Show("Null");
            }
        }

        public void NavigateTo(string uri)
        {
            _frame.Navigate(new Uri(uri, UriKind.Relative));
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
            else
            {
                MessageBox.Show("Ne moze");
            }
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            throw new NotImplementedException();
        }

        public string CurrentPageKey { get; }

        public bool CanGoBack => _frame.CanGoBack;
    }

}
