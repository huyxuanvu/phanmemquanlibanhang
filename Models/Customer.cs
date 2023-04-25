using System;
using System.Collections.Generic;
using Newtonsoft.Json; 

namespace Models
{
    public class Customer : Person, IComparable<Customer>
    {
        [JsonProperty("customerType")]
        public string CustomerType { get; set; }
        [JsonProperty("point")]
        public int Point { get; set; }
        [JsonProperty("createdTime")]
        public DateTime CreateTime { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }

        public Customer() { }

        public Customer(string customerType, int point, DateTime createTime, string email)
        {
            CustomerType = customerType;
            Point = point;
            CreateTime = createTime;
            Email = email;
        }

        public Customer(string personId, string fullName, DateTime birthDate,
            string address, string phoneNumber, string customerType,
            int point, DateTime createTime, string email) :
            base(personId, fullName, birthDate, address, phoneNumber)
        {
            CustomerType = customerType;
            Point = point;
            CreateTime = createTime;
            Email = email;
        }

        public override bool Equals(object obj)
        {
            return obj is Customer customer &&
                   base.Equals(obj) &&
                   PersonId == customer.PersonId;
        }

        public override int GetHashCode()
        {
            int hashCode = 2079290131;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PersonId);
            return hashCode;
        }

        public int CompareTo(Customer other)
        {
            return PersonId.CompareTo(other.PersonId);
        }
    }
}
