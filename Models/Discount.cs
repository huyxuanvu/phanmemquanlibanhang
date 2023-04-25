using System;
using Newtonsoft.Json;

namespace Models
{
    public class Discount
    {
        [JsonIgnore]
        private static int s_autoId = 1000000;
        [JsonProperty("id")]
        public int DiscountId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("start")]
        public DateTime StartTime { get; set; }
        [JsonProperty("end")]
        public DateTime EndTime { get; set; }
        [JsonProperty("type")]
        public string DiscounType { get; set; }
        [JsonProperty("amount")]
        public int DiscountPriceAmount { get; set; }
        [JsonProperty("percent")]
        public int DiscountPercent { get; set; }

        public Discount() { }
        public Discount(int id)
        {
            DiscountId = id > 0 ? id : s_autoId++;
        }

        public Discount(int discountId, string name, DateTime startTime,
            DateTime endTime, string discounType, int discountPriceAmount,
            int discountPercent) : this(discountId)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            DiscounType = discounType;
            DiscountPriceAmount = discountPriceAmount;
            DiscountPercent = discountPercent;
        }

        public override bool Equals(object obj)
        {
            return obj is Discount discount &&
                   DiscountId == discount.DiscountId;
        }

        public override int GetHashCode()
        {
            return 1574009819 + DiscountId.GetHashCode();
        }

        public static void UpdateAutoId(int v)
        {
            s_autoId = v;
        }
    }
}
