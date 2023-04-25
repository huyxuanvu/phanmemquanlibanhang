using System;
using System.Collections.Generic;
using Models;

namespace Controllers
{
    public class Utils
    {
        public static List<Item> CreateFakeItems()
        {
            List<Item> items = new List<Item>();
            items.Add(new Item(0, "Iphone 14", "Điện tử", 20, "Apple", DateTime.Now, 28000000, null));
            items.Add(new Item(0, "Iphone 14 Pro", "Điện tử", 10, "Apple", DateTime.Now, 32000000, null));
            items.Add(new Item(0, "Iphone 14 Pro Max", "Điện tử", 30, "Apple", DateTime.Now, 35000000, null));
            items.Add(new Item(0, "Xà lách", "Thực phẩm", 30, "VGarden", DateTime.Now, 20000, null));
            items.Add(new Item(0, "Thịt gà", "Thực phẩm", 20, "VGarden", DateTime.Now, 80000, null));
            items.Add(new Item(0, "Mì hảo hảo", "Thực phẩm", 300, "AceCook", DateTime.Now, 3500, null));
            items.Add(new Item(0, "Xi măng", "Vật liệu xây dựng", 1000, "Thành Thái", DateTime.Now, 135000, null));
            items.Add(new Item(0, "Quần short", "Thời trang", 100, "Việt Tiến", DateTime.Now, 125000, null));
            return items;
        }

        public static List<Customer> CreateFakeCustomer()
        {
            List<Customer> customers = new List<Customer>();
            customers.Add(new Customer("0123654789123", "Trần Trung Dũng",
                DateTime.ParseExact("12/08/2007", "dd/MM/yyyy", null), "Hà Nội", "0978123456",
                "Khách mua buôn", 1090, DateTime.Now, "trungdung@xmail.com"));
            customers.Add(new Customer("0123654789124", "Lê Viết Luân",
                DateTime.ParseExact("22/09/2007", "dd/MM/yyyy", null), "Hải Dương", "0972183457",
                "Khách mua lẻ", 180, DateTime.Now, "vietluan@xmail.com"));
            customers.Add(new Customer("0123654789125", "Trần Thu Hà",
                DateTime.ParseExact("12/01/2002", "dd/MM/yyyy", null), "Đà Nẵng", "0978143458",
                "Khách mua buôn", 5000, DateTime.Now, "thuha@xmail.com"));
            customers.Add(new Customer("0123654789126", "Lê Bá Tuấn",
                DateTime.ParseExact("16/10/2000", "dd/MM/yyyy", null), "Hồ Chí Minh", "0978173459",
                "Khách mua lẻ", 200, DateTime.Now, "batuan@xmail.com"));
            customers.Add(new Customer("0123654789120", "Mai Anh Dũng",
                DateTime.ParseExact("19/08/2001", "dd/MM/yyyy", null), "Hòa Bình", "0978623466",
                "Khách mua buôn", 600, DateTime.Now, "anhdunguu@xmail.com"));
            customers.Add(new Customer("0123654789127", "Ngô Quốc Công",
                DateTime.ParseExact("22/04/1996", "dd/MM/yyyy", null), "Vũng Tàu", "0978123476",
                "Khách mua lẻ", 700, DateTime.Now, "quoccong@xmail.com"));
            customers.Add(new Customer("0123654789129", "Hà Văn Thắng",
                DateTime.ParseExact("11/05/2000", "dd/MM/yyyy", null), "Nghệ An", "0978193851",
                "Khách mua lẻ", 400, DateTime.Now, "vanthangha@xmail.com"));
            customers.Add(new Customer("0123654789128", "Nguyễn Thùy Dung",
                DateTime.ParseExact("17/09/2000", "dd/MM/yyyy", null), "Thái Bình", "0979123450",
                "Khách mua lẻ", 100, DateTime.Now, "thuydungnguyen@xmail.com"));
            customers.Add(new Customer("0923654789128", "Khắc Đức Tâm",
                DateTime.ParseExact("18/01/2003", "dd/MM/yyyy", null), "Bắc Giang", "0979123450",
                "Khách mua buôn", 1000, DateTime.Now, "avantamkaka@xmail.com"));
            return customers;
        }

        public static List<Discount> CreateFakeDiscounts()
        {
            var dateTimeFormat = "dd/MM/yyyy HH:mm:ss";
            List<Discount> discounts = new List<Discount>();
            discounts.Add(new Discount(0, "KM 1/1",
                DateTime.ParseExact("01/01/2022 01:01:01", dateTimeFormat, null),
                DateTime.ParseExact("01/01/2022 23:59:59", dateTimeFormat, null),
                "Giảm giá trực tiếp", 50000, 0));
            discounts.Add(new Discount(0, "KM 15/1",
                DateTime.ParseExact("15/01/2022 15:15:15", dateTimeFormat, null),
                DateTime.ParseExact("15/01/2022 23:59:59", dateTimeFormat, null),
                "Giảm giá trực tiếp", 50000, 0));
            discounts.Add(new Discount(0, "KM 2/2",
                DateTime.ParseExact("02/02/2022 02:02:02", dateTimeFormat, null),
                DateTime.ParseExact("02/02/2022 23:59:59", dateTimeFormat, null),
                "Giảm giá trực tiếp", 150000, 0));
            discounts.Add(new Discount(0, "KM 3/3",
                DateTime.ParseExact("03/03/2022 03:03:03", dateTimeFormat, null),
                DateTime.ParseExact("03/03/2022 23:59:59", dateTimeFormat, null),
                "Khuyến mãi % giá bán", 0, 20));
            discounts.Add(new Discount(0, "KM 4/4",
                DateTime.ParseExact("04/04/2022 04:04:04", dateTimeFormat, null),
                DateTime.ParseExact("04/04/2022 23:59:59", dateTimeFormat, null),
                "Giảm giá trực tiếp", 50000, 0));
            discounts.Add(new Discount(0, "KM 5/5",
                DateTime.ParseExact("05/05/2022 05:05:05", dateTimeFormat, null),
                DateTime.ParseExact("05/05/2022 23:59:59", dateTimeFormat, null),
                "Khuyến mãi % giá bán", 0, 30));
            return discounts;
        }
    }
}
