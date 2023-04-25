using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;

namespace Controllers
{
    public interface IBillController
    {
        void UpdateBill(BillDetail bill, SelectedItem item, bool isUpdated=false);
        void RemoveItem(BillDetail bill, int index);
    }
}
