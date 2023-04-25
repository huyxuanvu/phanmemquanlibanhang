using Models;

namespace Controllers
{
    public interface IBillSorter
    {
        int CompareBillByTotalItems(BillDetail b1, BillDetail b2);
        int CompareBillByTotalAmount(BillDetail b1, BillDetail b2);
        int CompareBillByCreatedDateTimeASC(BillDetail b1, BillDetail b2);
        int CompareBillByCreatedDateTimeDESC(BillDetail b1, BillDetail b2);
        int CompareBillByCustomerName(BillDetail b1, BillDetail b2);
    }
}
