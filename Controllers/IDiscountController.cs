using Models;

namespace Controllers
{
    public interface IDiscountController
    {
        bool IsStartTimeMatch(Discount discount, string dateTime);
        bool IsEndTimeMatch(Discount discount, string dateTime);
        bool IsDiscountNameMatch(Discount discount, string name);
    }
}
