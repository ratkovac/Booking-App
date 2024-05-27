using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BookingApp.DTO;
using BookingApp.GUI_Elements;
using BookingApp.Model;
using BookingApp.Service;
using BookingApp.View.NGuest;
using CLI.Observer;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.CSharp.RuntimeBinder;
using NavigationService = System.Windows.Navigation.NavigationService;

namespace BookingApp.View.ViewModel.Guest
{
    public class MenuViewModel : IObserver, INotifyPropertyChanged
    {
        private string activeMenuItem;
        private NavigationService _navigationService;

        public ObservableCollection<DelayReservationDTO> Messages { get; set; }

        private DelayReservationService delayReservationService;

        private SuperGuestManagmentService SuperGuestManagmentService { get; set; }

        public string ActiveMenuItem
        {
            get { return activeMenuItem; }
            set
            {
                activeMenuItem = value;
                OnPropertyChanged(nameof(ActiveMenuItem));
            }
        }

        private string _isSuperGuestImage;

        public string IsSuperGuestImage
        {
            get { return _isSuperGuestImage; }
            set
            {
                _isSuperGuestImage = value;
                OnPropertyChanged(nameof(IsSuperGuestImage));
            }
        }
        public MenuViewModel(string activeButton, NavigationService navigationService, User user)
        {
            ActiveMenuItem = activeButton;
            _navigationService = navigationService;

            Messages = new ObservableCollection<DelayReservationDTO>();
            delayReservationService = new DelayReservationService();

            SuperGuestManagmentService = new SuperGuestManagmentService();

            IsSuperGuest(user.Id);

            Update();
        }

        private void IsSuperGuest(int userId)
        {
            if (SuperGuestManagmentService.AddSuperGuest(userId))
            {
                IsSuperGuestImage = "../../Icon/true.png";
            }
            else
            {
                IsSuperGuestImage = "../../Icon/x.png";
            }
        }


        public void Update()
        {
            Messages.Clear();
            foreach (var message in delayReservationService.GetAll())
            {
                if(message.Status == DelayReservationStatusEnum.Declined)
                {
                    Messages.Add(new DelayReservationDTO(message));
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Back_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var previousPage = this._navigationService.Content as dynamic;
                if (previousPage != null)
                {
                    try
                    {
                        previousPage.RemoveBlurEffect();
                    }
                    catch (RuntimeBinderException)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing menu: {ex.Message}");
            }
        }

        public void DeleteMessage(object sender, RoutedEventArgs e)
        {
            var contentControl = sender as ContentControl;
            if (contentControl != null)
            {
                var message = contentControl.DataContext as DelayReservationDTO;
                if (message != null)
                {
                    ItemsControlExtensions.SetSelectedItem(contentControl, message);
                    delayReservationService.DeleteById(message.Id);
                }
            }
            Update();
        }

    }
}
