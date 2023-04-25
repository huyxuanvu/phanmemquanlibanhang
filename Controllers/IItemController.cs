using Models;

namespace Controllers
{
    public interface IItemController
    {
        int CompareItemByPriceASC(Item item1, Item  item2);
        int CompareItemByPriceDESC(Item item1, Item  item2);
        int CompareItemByName(Item item1, Item  item2);
        int CompareItemByDate(Item item1, Item  item2);
        int CompareItemByQuantity(Item item1, Item  item2);
        bool IsItemNameMatch(Item item, string name);
        bool IsItemTypeMatch(Item item, string type);
        bool IsItemBrandMatch(Item item, string brand);
        bool IsItemPriceMatch(Item item, int from, int to);
        bool IsItemQuantityMatch(Item item, int from, int to);
        string GetDiscountName(Discount discount);
    }
}
