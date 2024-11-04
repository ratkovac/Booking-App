using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        public BookingApp.Model.Tourist Tourist { get; set; }
        public NotificationRepository notificationRepository { get; set; }

        private ObservableCollection<Notification> _listNotification;
        public ObservableCollection<Notification> ListNotification
        {
            get => _listNotification;
            set
            {
                _listNotification = value;
                OnPropertyChanged();
            }
        }

        private Notification _selectedNotification;
        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                _selectedNotification = value;
                OnPropertyChanged();
            }
        }

        public NotificationViewModel(BookingApp.Model.Tourist tourist)
        {
            Tourist = tourist;
            notificationRepository = new NotificationRepository();
            ListNotification = new ObservableCollection<Notification>(notificationRepository.GetAll());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateNotificationList()
        {
            ListNotification.Clear();
            foreach (var notification in notificationRepository.GetAll())
            {
                ListNotification.Add(notification);
            }
        }

        public void Update()
        {
            UpdateNotificationList();
        }
    }
}
