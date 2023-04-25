using System;
using Newtonsoft.Json;

namespace Models
{
    /// <summary>
    /// Lớp mô tả thông tin mặt hàng
    /// </summary>
    public class Item : IComparable<Item>
    {
        [JsonIgnore]
        private static int s_autoId = 100000;
        [JsonProperty("id")]
        public int ItemId { get; set; }
        [JsonProperty("name")]
        public string ItemName { get; set; }
        [JsonProperty("type")]
        public string ItemType { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("brand")]
        public string Brand { get; set; }
        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("discount")]
        public Discount Discount { get; set; }

        public Item() { }

        public Item(int id)
        {
            ItemId = id > 0 ? id : s_autoId++;
        }

        public Item(int itemId, string itemName, string itemType,
            int quantity, string brand, DateTime releaseDate, int price,
            Discount discount) : this(itemId)
        {
            ItemName = itemName;
            ItemType = itemType;
            Quantity = quantity;
            Brand = brand;
            ReleaseDate = releaseDate;
            Price = price;
            Discount = discount;
        }

        public static void UpdateAutoId(int v)
        {
            s_autoId = v;
        }


        public int CompareTo(Item other)
        {
            return ItemId - other.ItemId;
        }

        public override bool Equals(object obj)
        {
            return obj is Item item &&
                   ItemId == item.ItemId;
        }

        public override int GetHashCode()
        {
            return -2113648141 + ItemId.GetHashCode();
        }
    }
}
