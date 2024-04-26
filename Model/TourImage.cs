using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourImage : ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int EntityId { get; set; }
        public int UserId { get; set; }

        public TourImage() { }

        public TourImage(string path, int entityId, int userId)
        {
            Path = path;
            EntityId = entityId;
            UserId = userId;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Path = values[1];
            EntityId = int.Parse(values[2]);
            UserId = int.Parse(values[3]);
        }
        public string[] ToCSV()
        {
            return new string[] { Id.ToString(), Path, EntityId.ToString(), UserId.ToString() };
        }
    }
}
