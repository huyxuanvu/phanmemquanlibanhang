using Models;

namespace Controllers
{
    public class BillController : IBillController
    {
        public void RemoveItem(BillDetail bill, int index)
        {
            var item = bill.Cart.SelectedItems[index];
            bill.TotalItem -= item.NumberOfSelectedItem;
            bill.TotalAmount -= item.NumberOfSelectedItem * item.PriceAfterDiscount;
            if (item.Discount != null)
            {
                bill.TotalDiscountAmount -= (int)(item.NumberOfSelectedItem *
                item.Price * (1.0f * item.Discount.DiscountPercent / 100)) +
                item.NumberOfSelectedItem * item.Discount.DiscountPriceAmount;
            }
            bill.SubTotal = bill.TotalAmount;
            bill.Cart.SelectedItems.Remove(item);
        }

        public void UpdateBill(BillDetail bill, SelectedItem item, bool isUpdated = false)
        {
            int index = bill.Cart.SelectedItems.IndexOf(item);
            if(index >= 0)
            {
                if(isUpdated) // sửa số lượng 1 sản phẩm
                {
                    bill.Cart.SelectedItems[index].NumberOfSelectedItem = item.NumberOfSelectedItem;
                } else // thêm mới 1 sản phẩm đã tồn tại tại 2 thời điểm khác nhau
                {
                    bill.Cart.SelectedItems[index].NumberOfSelectedItem += item.NumberOfSelectedItem;
                }
                var items = bill.Cart.SelectedItems;
                bill.TotalItem = 0;
                bill.TotalAmount = 0;
                bill.TotalDiscountAmount = 0;
                foreach (var it in items)
                {
                    bill.TotalItem += it.NumberOfSelectedItem;
                    bill.TotalAmount += it.NumberOfSelectedItem * it.PriceAfterDiscount;
                    if (it.Discount != null)
                    {
                        bill.TotalDiscountAmount += (int)(it.NumberOfSelectedItem *
                        it.Price * (1.0f * it.Discount.DiscountPercent / 100)) +
                        it.NumberOfSelectedItem * it.Discount.DiscountPriceAmount;
                    }
                }
                bill.SubTotal = bill.TotalAmount;
            }
            else
            {
                bill.Cart.SelectedItems.Add(item);
                bill.TotalItem += item.NumberOfSelectedItem;
                bill.TotalAmount += item.NumberOfSelectedItem * item.PriceAfterDiscount;
                bill.SubTotal = bill.TotalAmount;
                if (item.Discount != null)
                {
                    bill.TotalDiscountAmount += (int)(item.NumberOfSelectedItem *
                    item.Price * (1.0f * item.Discount.DiscountPercent / 100)) +
                    item.NumberOfSelectedItem * item.Discount.DiscountPriceAmount;
                }
            }
        }
    }
}
