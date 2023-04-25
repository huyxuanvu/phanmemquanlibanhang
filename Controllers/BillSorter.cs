using Models;
using System;

namespace Controllers
{
    public class BillSorter : IBillSorter
    {
        public int CompareBillByCreatedDateTimeASC(BillDetail b1, BillDetail b2)
        {
            if(b1.CreatedTime < b2.CreatedTime)
            {
                return -1;
            } else if(b1.CreatedTime == b2.CreatedTime)
            {
                return 0;
            } else
            {
                return 1;
            }
        }

        public int CompareBillByCreatedDateTimeDESC(BillDetail b1, BillDetail b2)
        {
            if (b1.CreatedTime < b2.CreatedTime)
            {
                return 1;
            }
            else if (b1.CreatedTime == b2.CreatedTime)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public int CompareBillByCustomerName(BillDetail b1, BillDetail b2)
        {
            if (b1.Cart.Customer == null && b2.Cart.Customer == null)
            {
                return 0;
            }
            else if(b1.Cart.Customer != null && b2.Cart.Customer == null)
            {
                return -1;
            } else if(b1.Cart.Customer == null && b2.Cart.Customer != null)
            {
                return 1;
            } else
            {
                int nameCompareResult = b1.Cart.Customer.FullName.FirstName
                    .CompareTo(b2.Cart.Customer.FullName.FirstName);
                if(nameCompareResult != 0)
                {
                    return nameCompareResult;
                } else
                {
                    return b1.Cart.Customer.FullName.LastName.CompareTo(b2.Cart.Customer.FullName.LastName);
                }
            }
        }

        public int CompareBillByTotalAmount(BillDetail b1, BillDetail b2)
        {
            return b2.TotalAmount.CompareTo(b1.TotalAmount);
        }

        public int CompareBillByTotalItems(BillDetail b1, BillDetail b2)
        {
            return b2.TotalItem - b1.TotalItem;
        }
    }
}
