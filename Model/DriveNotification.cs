using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Model
{
    public class DriveNotification
    {
        public int Id { get; set; }

        public string Caption { get; set; }

        public string Text { get; set; }

        public FastDrive fastDrive { get; set; }
        
        public DriveNotification(string caption, string text)
        {
            this.Caption = caption;
            this.Text = text;
        }
        public string[] ToCSV()
        {
            return new string[]
            {
                Id.ToString(),
                Caption,
                Text
            };
        }

        public override string ToString()
        {
            return $"Id: {Id}, Caption: {Caption}, Text: {Text}";
        }

        public void FromCSV(string[] values)
        {
            if (values.Length != 3)
            {
                throw new ArgumentException("Neispravan format CSV podataka.");
            }

            Id = Convert.ToInt32(values[0]);
            Caption = values[1];
            Text = values[2];
        }
    }
}
