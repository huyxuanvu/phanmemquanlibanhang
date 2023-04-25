using Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Controllers
{
    public class IOController : IIOController
    {
        public static string ITEM_FILE_NAME = "item.json";
        public static string CUSTOMER_FILE_NAME = "customer.json";
        public static string DISCOUNT_FILE_NAME = "discount.json";
        public static string BILL_FILE_NAME = "bill.json";
        public static string STAT_FILE_NAME = "stat.json";

        public void LoadDataList(List<Item> items, List<Customer> customers,
            List<Discount> discounts, List<BillDetail> billDetails)
        {
            try
            {
                items.AddRange(ReadData<Item>(ITEM_FILE_NAME, "items"));
                customers.AddRange(ReadData<Customer>(CUSTOMER_FILE_NAME, "customers"));
                discounts.AddRange(ReadData<Discount>(DISCOUNT_FILE_NAME, "discounts"));
                billDetails.AddRange(ReadData<BillDetail>(BILL_FILE_NAME, "bills"));
            }
            catch (JsonReaderException e)
            {
                File.AppendAllText("log.txt", e.Message);
            }
        }

        public List<T> ReadData<T>(string fileName, string root)
        {
            var data = File.ReadAllText(fileName);
            var jObject = JObject.Parse(data);
            var jArray = jObject[root];
            List<T> list = new List<T>();
            foreach (var item in jArray)
            {
                list.Add(item.ToObject<T>());
            }
            return list;
        }

        public bool SaveDataList(List<Item> itemList, List<Customer> customerList,
            List<Discount> discountList, List<BillDetail> billDetailList)
        {
            var itemObj = new
            {
                items = itemList
            };
            var customerObj = new
            {
                customers = customerList
            };
            var discountObj = new
            {
                discounts = discountList
            };
            var billObj = new
            {
                bills = billDetailList
            };
            WriteDate(itemObj, ITEM_FILE_NAME);
            WriteDate(customerObj, CUSTOMER_FILE_NAME);
            WriteDate(discountObj, DISCOUNT_FILE_NAME);
            WriteDate(billObj, BILL_FILE_NAME);
            return true;
        }

        public void WriteDate(object obj, string fileName)
        {
            var jsonStr = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(fileName, jsonStr);
        }
    }
}
