using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Models
{
    public class Cart : IComparable<Cart>
    {
        [JsonIgnore]
        private static int s_autoId = 10000000;
        [JsonProperty("id")]
        public int CartId { get; set; }
        [JsonProperty("customer")]
        public Customer Customer { get; set; }
        [JsonProperty("items")]
        public List<SelectedItem> SelectedItems { get; set; } = new List<SelectedItem>();
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

        public Cart()
        {
        }

        public Cart(int cartId)
        {
            CartId = cartId > 0 ? cartId : s_autoId++;
        }

        public Cart(int cartId, Customer customer, 
            List<SelectedItem> selectedItems, int totalItems) : this(cartId)
        {
            Customer = customer;
            SelectedItems = selectedItems;
            TotalItems = totalItems;
        }

        public override bool Equals(object obj)
        {
            return obj is Cart cart &&
                   CartId == cart.CartId;
        }

        public override int GetHashCode()
        {
            return -1568810734 + CartId.GetHashCode();
        }

        public int CompareTo(Cart other)
        {
            return CartId - other.CartId;
        }

        public static void UpdateAutoId(int v)
        {
            s_autoId = v;
        }
    }
}
