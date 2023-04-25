using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class AutoIdUpdater : IAutoIdUpdater
    {
        public void UpdateBillAutoId(List<BillDetail> bills)
        {
            int billId = 0;
            foreach (var bill in bills)
            {
                if(billId < bill.BillId)
                {
                    billId = bill.BillId;
                }
            }
            Bill.UpdateAutoId(billId + 1);
        }

        public void UpdateDiscountAutoId(List<Discount> discounts)
        {
            int discountId = 0;
            foreach (var discount in discounts)
            {
                if (discountId < discount.DiscountId)
                {
                    discountId = discount.DiscountId;
                }
            }
            Discount.UpdateAutoId(discountId + 1);
        }

        public void UpdateItemAutoId(List<Item> items)
        {
            int itemId = 0;
            foreach (var item in items)
            {
                if (itemId < item.ItemId)
                {
                    itemId = item.ItemId;
                }
            }
            Item.UpdateAutoId(itemId + 1);
        }
    }
}
