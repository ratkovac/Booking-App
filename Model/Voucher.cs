using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Voucher : ISerializable
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Used { get; set; }
        public bool ValidVoucher { get; set; }

        public Voucher() { }

        public Voucher(int id, DateTime creationDate, DateTime expirationDate, bool used, bool validVoucher)
        {
            Id = id;
            CreationDate = creationDate;
            ExpirationDate = expirationDate;
            Used = used;
            ValidVoucher = validVoucher;
        }
        public Voucher(DateTime creationDate, DateTime expirationDate, bool used, bool validVoucher)
        {
            CreationDate = creationDate;
            ExpirationDate = expirationDate;
            Used = used;
            ValidVoucher = validVoucher;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            CreationDate = DateTime.Parse(values[1]);
            ExpirationDate = DateTime.Parse(values[2]);
            Used = Convert.ToBoolean(values[3]);
            ValidVoucher = Convert.ToBoolean(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvvalues =
            {
                Id.ToString(),
                CreationDate.ToString(),
                ExpirationDate.ToString(),
                Used.ToString(),
                ValidVoucher.ToString()
            };

            return csvvalues;
        }

    }
}
