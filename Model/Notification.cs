using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Notification : ISerializable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateIssued { get; set; }
        public int UserId { get; set; }

        public Notification() { }

        public Notification(string title, string text, DateTime dateIssued, int userId)
        {
            Title = title;
            Text = text;
            DateIssued = dateIssued;
            UserId = userId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Title = values[1];
            Text = values[2];
            DateIssued = DateTime.Parse(values[3]);
            UserId = int.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Title, Text, DateIssued.ToString(), UserId.ToString() };
            return csvValues;
        }
    }
}
