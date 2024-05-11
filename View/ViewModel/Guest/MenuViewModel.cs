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
using NavigationService = System.Windows.Navigation.NavigationService;

namespace BookingApp.View.ViewModel.Guest
{
    public class MenuViewModel : IObserver, INotifyPropertyChanged
    {
        private string activeMenuItem;
        private NavigationService _navigationService;

        public ObservableCollection<DelayReservationDTO> Messages { get; set; }

        private DelayReservationService delayReservationService;

        public string ActiveMenuItem
        {
            get { return activeMenuItem; }
            set
            {
                activeMenuItem = value;
                OnPropertyChanged(nameof(ActiveMenuItem));
            }
        }
        public MenuViewModel(string activeButton, NavigationService navigationService)
        {
            ActiveMenuItem = activeButton;
            _navigationService = navigationService;

            Messages = new ObservableCollection<DelayReservationDTO>();
            delayReservationService = new DelayReservationService();

            Update();
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

        public void Back_OnClick()
        {
            _navigationService.GoBack();
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
