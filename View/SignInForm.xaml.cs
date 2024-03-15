using BookingApp.Model;
using BookingApp.Repository;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using BookingApp.View.Owner;
using BookingApp.View.Tourist;
using BookingApp.View.GuideView;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;



        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password)
                {
                    switch (user.Role)
                    {
                        case "Owner":
                            OwnerFrontPage ownerFrontPage = new OwnerFrontPage(user);
                            ownerFrontPage.Show();
                            Close();
                            break;
                        case "Guest":
                            break;
                        case "Guide":
                            TourForm tourForm = new TourForm();
                            tourForm.Show();
                            Close();
                            break;
                        case "Tourist":
                            TouristFrontPage touristFrontPage = new TouristFrontPage(user);
                            touristFrontPage.Show();
                            Close();
                            break;
                        case "Driver":
                            break;
                        default:
                            break;
                    }
                    //CommentsOverview commentsOverview = new CommentsOverview(user);
                    //commentsOverview.Show();
                    //Close();
                }
                else if (user.Password != txtPassword.Password)
                {
                    MessageBox.Show("Wrong password!");
                }
                else
                {
                    MessageBox.Show("Wrong role!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }

        }
    }
}
