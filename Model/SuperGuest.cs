using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class SuperGuest : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set;}
        public int BonusPoens { get; set; }

        public SuperGuest(int userId, DateOnly startDate, DateOnly endDate, int bonusPoens)
        {
            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
            BonusPoens = bonusPoens;
        }

        public string[] ToCSV()
        {
            string[] values =
            {
                Id.ToString(),
                UserId.ToString(),
                StartDate.ToString("yyyy-MM-dd"),
                EndDate.ToString("yyyy-MM-dd"),
                BonusPoens.ToString()
            };
            return values;
        }

        public SuperGuest()
        {

        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            StartDate = DateOnly.ParseExact(values[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            EndDate = DateOnly.ParseExact(values[3], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            BonusPoens = Convert.ToInt32(values[4]);
        }

    }
}
