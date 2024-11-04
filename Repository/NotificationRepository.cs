using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.WPF.View.Tourist.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BookingApp.Repository
{
    public class NotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/notifications.csv";

        private readonly Serializer<Notification> _serializer;

        private List<Notification> _notifications;

        public NotificationRepository()
        {
            _serializer = new Serializer<Notification>();
            _notifications = _serializer.FromCSV(FilePath);
        }

        public List<Notification> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Notification Save(Notification notification)
        {
            notification.Id = NextId();
            _notifications = _serializer.FromCSV(FilePath);
            _notifications.Add(notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;
        }

        public int NextId()
        {
            _notifications = _serializer.FromCSV(FilePath);
            if (_notifications.Count < 1)
            {
                return 1;
            }
            return _notifications.Max(c => c.Id) + 1;
        }

        public void Delete(Notification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            Notification found = _notifications.Find(c => c.Id == notification.Id);
            if (found != null)
            {
                _notifications.Remove(found);
            }
            _serializer.ToCSV(FilePath, _notifications);
        }

        public Notification Update(Notification notification)
        {
            _notifications = _serializer.FromCSV(FilePath);
            Notification current = _notifications.Find(c => c.Id == notification.Id);
            int index = _notifications.IndexOf(current);
            _notifications.Remove(current);
            _notifications.Insert(index, notification);
            _serializer.ToCSV(FilePath, _notifications);
            return notification;
        }

        public Notification GetById(int id)
        {
            _notifications = _serializer.FromCSV(FilePath);
            return _notifications.FirstOrDefault(d => d.Id == id);
        }
    }
}
