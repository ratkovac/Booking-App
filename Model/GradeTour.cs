using BookingApp.Model;
using BookingApp.Serializer;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Model
{
    public class GradeTour : ISerializable
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public User Tourist { get; set; }
        public int TourInstanceId { get; set; }
        public TourInstance TourInstance { get; set; }
        public int Grade { get; set; }
        public string Comment { get; set; }
        public List<string> Images { get; set; }
        public bool IsValid { get; set; }

        public GradeTour()
        {
            Images = new List<string>();
        }
        public GradeTour(int touristId, User tourist, int tourInstanceId, int grade, string comment, List<string> images)
        {
            TouristId = touristId;
            Tourist = tourist;
            TourInstanceId = tourInstanceId;
            Grade = grade;
            Comment = comment;
            Images = images;
            IsValid = true;
        }

        public string[] ToCSV()
        {
            string ImageString = "";
            foreach (string image in Images)
            {
                if (image != Images.Last())
                {
                    ImageString += image + ",";
                }
            }
            ImageString += Images.Last();

            string[] csvvalues = { Id.ToString(), TouristId.ToString(), TourInstanceId.ToString(), Grade.ToString(),
                Comment, ImageString,IsValid.ToString()};
            return csvvalues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            TourInstanceId = Convert.ToInt32(values[2]);
            Grade = Convert.ToInt32(values[3]);
            Comment = values[4];
            foreach (string image in values[5].Split(","))
            {
                Images.Add(image);
            }
            IsValid = Convert.ToBoolean(values[6]);
        }
    }
}