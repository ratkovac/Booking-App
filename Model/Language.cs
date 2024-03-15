using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Model
{
    public class Language : ISerializable
    {
        public int Id;

        public String Name;

        public Language(string name)
        {
            Id = 0;
            Name = name;
        }
        public Language() { }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
        }
    }
}
