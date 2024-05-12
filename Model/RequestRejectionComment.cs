using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository;
using BookingApp.Serializer;

namespace BookingApp.Model
{
    public class RequestRejectionComment : ISerializable
    {
        public int Id { get; set; }
        public DelayReservation DelayReservation { get; set; }
        public string Comment { get; set; }

        public RequestRejectionComment() { }
        public RequestRejectionComment(DelayReservation delayReservation, string comment)
        {
            DelayReservation = delayReservation;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string delayReservation = DelayReservation.Id.ToString();
            string[] values =
            {
                Id.ToString(),
                delayReservation,
                Comment
            };
            return values;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            int delayReservationId = Convert.ToInt32(values[1]);
            DelayReservationRepository delayReservationRepository = new DelayReservationRepository();
            DelayReservation = delayReservationRepository.GetByID(delayReservationId);
            Comment = values[2];
        }

    }
}
