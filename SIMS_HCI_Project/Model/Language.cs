﻿using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace BookingApp.Model
{
    public class Language: ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Language() { }

        public Language(string lang) { 
            Name = lang;
        }

        public Language(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
        }
        public override string ToString()
        {
            return Name;
        }
    }

}
