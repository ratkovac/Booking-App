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

namespace BookingApp.View.GuideView.Pages
{
    /// <summary>
    /// Interaction logic for ActionBar.xaml
    /// </summary>
    public partial class ActionBar : UserControl
    {
        public static readonly DependencyProperty PageNameProperty =
            DependencyProperty.Register("PageName", typeof(string), typeof(ActionBar), new PropertyMetadata(""));

        public event EventHandler NavigationButtonClicked;
        public string PageName
        {
            get { return (string)GetValue(PageNameProperty); }
            set { SetValue(PageNameProperty, value); }
        }

        private Frame _mainFrame;

        public ActionBar()
        {
            InitializeComponent();
        }
        public ActionBar(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            DataContext = this;
        }

        public void btnNavigation_Click(object sender, RoutedEventArgs e)
        {
            // Provera da li postoji referenca na glavni okvir (mainFrame)
            if (_mainFrame != null)
            {
                // Navigacija na TourManagementPage
                _mainFrame.Navigate(new Uri("View/GuideView/Pages/TourManagementPage.xaml", UriKind.Relative));
            }
        }


        private Page GetParentPage(FrameworkElement element)
        {
            // Prolazi kroz roditeljske elemente dok ne pronađe stranicu
            var parent = element.Parent;

            while (parent != null)
            {
                if (parent is Page page)
                {
                    return page;
                }
                parent = (parent as FrameworkElement)?.Parent;
            }

            return null; // Ako nije pronađena roditeljska stranica
        }

    }
}
