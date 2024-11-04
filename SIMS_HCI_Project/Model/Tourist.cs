using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Tourist : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<int> VoucherIds { get; set; }
        public List<Voucher> Vouchers { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfToursAttended { get; set; }
        public string ProfilePicture { get; set; }

        public Tourist()
        {
            VoucherIds = new List<int>();
        }
        public Tourist(int id, string name, string lastname, string adress, string email, int userId, DateTime birthDate, string phoneNumber, int numberOfToursAttended, string profilePicture)
        {
            Id = id;
            Name = name;
            LastName = lastname;
            Address = adress;
            Email = email;
            UserId = userId;
            VoucherIds = new List<int>();
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            NumberOfToursAttended = numberOfToursAttended;
            ProfilePicture = profilePicture;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LastName = values[2];
            Address = values[3];
            Email = values[4];
            UserId = Convert.ToInt32(values[5]);
            if (values[6] != "")
            {
                foreach (string voucher in values[6].Split(","))
                {
                    int voucherId = Convert.ToInt32(voucher);
                    VoucherIds.Add(voucherId);
                }
            }
            BirthDate = DateTime.Parse(values[7]);
            PhoneNumber = values[8];
            NumberOfToursAttended = Convert.ToInt32(values[9]);
            ProfilePicture = values[10];
        }

        public string[] ToCSV()
        {
            string VoucherString = "";
            if (VoucherIds.Count > 0)
            {
                foreach (int voucherId in VoucherIds)
                {
                    if (voucherId != VoucherIds.Last())
                    {
                        VoucherString += voucherId.ToString() + ",";
                    }
                }
                VoucherString += VoucherIds.Last();
            }
            string[] csvvalues = { Id.ToString(), Name, LastName, Address, Email, UserId.ToString(), VoucherString, BirthDate.ToString(), PhoneNumber, NumberOfToursAttended.ToString(), ProfilePicture };
            return csvvalues;
        }
    }
}
