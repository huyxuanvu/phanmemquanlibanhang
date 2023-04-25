using Models;
using System.Text.RegularExpressions;

namespace Controllers
{
    public class BillFinder : IBillFinder
    {
        public bool IsBillCreatedTimeMatch(BillDetail bill, string timeTxt)
        {
            var pattern = $".*{timeTxt}.*";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(bill.CreatedTime.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        public bool IsBillCustomerNameMatch(BillDetail bill, string customerName)
        {
            var pattern = $".*{customerName}.*";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(bill.Cart?.Customer?.FullName.FirstName);
        }

        public bool IsBillStaffNameMatch(BillDetail bill, string staffName)
        {
            var pattern = $".*{staffName}.*";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(bill?.StaffName);
        }

        public bool IsBillStatusMatch(BillDetail bill, string status)
        {
            var pattern = $".*{status}.*";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(bill?.Status);
        }
    }
}
