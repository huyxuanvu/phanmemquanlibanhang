using Models;

namespace Controllers
{
    public interface IBillFinder
    {
        bool IsBillCreatedTimeMatch(BillDetail bill, string timeTxt);
        bool IsBillCustomerNameMatch(BillDetail bill, string customerName);
        bool IsBillStatusMatch(BillDetail bill, string status);
        bool IsBillStaffNameMatch(BillDetail bill, string staffName);
    }
}
