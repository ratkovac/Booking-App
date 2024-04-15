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
        public Tourist Tourist { get; set; }
        public int TourReservationId { get; set; }
        public TourReservation TourReservation { get; set; }
        public List<string> Grades { get; set; }
        //public List<string> Comments { get; set; }
        public string Comment { get; set; }
        public List<string> Images { get; set; }
        public bool IsValid { get; set; }

        public GradeTour()
        {
            Grades = new List<string>();
            Images = new List<string>();
        }
        public GradeTour(int touristId, Tourist tourist, int tourReservationId, List<string> grades, string comment, List<string> images)
        {
            TouristId = touristId;
            Tourist = tourist;
            TourReservationId = tourReservationId;
            Grades = grades;
            Comment = comment;
            Images = images;
            IsValid = true;
        }

        public string[] ToCSV()
        {
            //string GradeString = string.Join(",", Grades);

            string GradeString = "";
            foreach (string grade in Grades)
            {
                if (grade != Grades.Last())
                {
                    GradeString += grade + ",";
                }
            }
            GradeString += Grades.Last();

            /*string CommentString = "";
            foreach (string comment in Comments)
            {
                if (comment != Comments.Last())
                {
                    CommentString += comment + ",";
                }
            }
            CommentString += Comments.Last();*/

            string ImageString = "";
            foreach (string image in Images)
            {
                if (image != Images.Last())
                {
                    ImageString += image + ",";
                }
            }
            ImageString += Images.Last();

            string[] csvvalues = { Id.ToString(), TouristId.ToString(), TourReservationId.ToString(), GradeString,
                Comment, ImageString, IsValid.ToString()};
            return csvvalues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TouristId = Convert.ToInt32(values[1]);
            TourReservationId = Convert.ToInt32(values[2]);
            foreach (string grade in values[3].Split(","))
            {
                Grades.Add(grade);
            }
            /*foreach (string comment in values[4].Split(","))
            {
                Comments.Add(comment);
            }*/
            Comment = values[4];
            foreach (string image in values[5].Split(","))
            {
                Images.Add(image);
            }
            IsValid = Convert.ToBoolean(values[6]);
        }
    }
}