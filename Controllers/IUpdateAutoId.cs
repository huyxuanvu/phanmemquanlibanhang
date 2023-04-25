using Models;
using System.Collections.Generic;

namespace Controllers
{
    public interface IAutoIdUpdater
    {
        void UpdateItemAutoId(List<Item> items);
        void UpdateDiscountAutoId(List<Discount> discounts);
        void UpdateBillAutoId(List<BillDetail> bills);
    }
}
