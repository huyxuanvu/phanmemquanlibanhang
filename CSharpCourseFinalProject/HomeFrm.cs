using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Models;
using Controllers;

namespace CSharpCourseFinalProject
{
    public enum ActionType
    {
        NORMAL, SEARCH
    }

    public interface IViewController
    {
        void AddNewItem<T>(T item);
        void UpdateItem<T>(T updatedItem);
        void Remove<T>(T item);
        
    }

    
    

    public partial class HomeFrm : Form, IViewController
    {
        

        #region main
        private static string DATE_FORMAT = "dd/MM/yyyy";
        private static string DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss";

        private List<Item> _items;
        private List<Discount> _discounts;
        private List<Customer> _customers;
        private CommonController _commonController;
        private ItemController _itemController;
        private List<Item> _searchItemResults;
        private List<Customer> _searchCustomerResults;
        private ActionType _actionType;
        private CustomerController _customerController;
        private List<Discount> _searchDiscountResults;
        private IDiscountController _discountController;
        private List<BillDetail> _bills;
        private List<BillDetail> _searchedBillResults;
        private IOController _ioController;
        private AutoIdUpdater _autoIdUpdater;
        private BillSorter _billSorter;

        public HomeFrm()
        {
            InitializeComponent();
            CenterToScreen();
            _ioController = new IOController();
            _items = new List<Item>();
            _discounts = new List<Discount>();
            _customers = new List<Customer>();
            _bills = new List<BillDetail>();
            _commonController = new CommonController();
            _customerController = new CustomerController();
            _discountController = new DiscountController();
            _itemController = new ItemController();
            _searchItemResults = new List<Item>();
            _searchCustomerResults = new List<Customer>();
            _searchDiscountResults = new List<Discount>();
            _searchedBillResults = new List<BillDetail>();
            _billSorter = new BillSorter();
            _autoIdUpdater = new AutoIdUpdater();
            _actionType = ActionType.NORMAL;
            // nạp dữ liệu
            _ioController.LoadDataList(_items, _customers, _discounts, _bills);
            // cập nhật mã tự tăng
            _autoIdUpdater.UpdateDiscountAutoId(_discounts);
            _autoIdUpdater.UpdateItemAutoId(_items);
            _autoIdUpdater.UpdateBillAutoId(_bills);
            // hiển thị dữ liệu
            ShowItems(_items);
            ShowCustomers(_customers);
            ShowDiscounts(_discounts);
            ShowBills(_bills);
        }

        private void ShowDiscounts(List<Discount> discounts)
        {
            tblDiscount.Rows.Clear();
            foreach (var discount in discounts)
            {
                tblDiscount.Rows.Add(
                    new object[] {
                        discount.DiscountId, discount.Name, discount.StartTime.ToString(DATE_TIME_FORMAT),
                        discount.EndTime.ToString(DATE_TIME_FORMAT), discount.DiscounType,
                        discount.DiscountPercent, $"{discount.DiscountPriceAmount:N0}"
                    }
                );
            }
        }

        private void ShowCustomers(List<Customer> customers)
        {
            tblCustomer.Rows.Clear();
            foreach (var customer in customers)
            {
                tblCustomer.Rows.Add(new object[] {
                        customer.PersonId, customer.FullName?.ToString(), customer.BirthDate.ToString(DATE_FORMAT),
                        customer.Address, customer.PhoneNumber, customer.CustomerType, $"{customer.Point:N0}",
                        customer.CreateTime.ToString(DATE_TIME_FORMAT), customer.Email
                    });
            }
        }

        public void AddNewItem<T>(T item)
        {
            if (typeof(T) == typeof(Item))
            {
                var newItem = item as Item;
                _commonController.AddNewItem(_items, newItem);
                tblItem.Rows.Add(new object[]
                {
                    newItem.ItemId, newItem.ItemName, newItem.ItemType, newItem.Quantity,
                    newItem.Brand, newItem.ReleaseDate.ToString("dd/MM/yyyy"), $"{newItem.Price:N0}",
                    _itemController.GetDiscountName(newItem.Discount)
                });
            }
            else if (typeof(T) == typeof(Customer))
            {
                var customer = item as Customer;
                int indexOfCustomer = _commonController.IndexOfItem(_customers, customer);
                if (indexOfCustomer >= 0)
                {
                    var title = "Lỗi trùng lặp";
                    var msg = "Thông tin khách hàng này đã tồn tại!";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    _commonController.AddNewItem(_customers, customer);
                    tblCustomer.Rows.Add(new object[] {
                        customer.PersonId, customer.FullName?.ToString(), customer.BirthDate.ToString(DATE_FORMAT),
                        customer.Address, customer.PhoneNumber, customer.CustomerType, $"{customer.Point:N0}",
                        customer.CreateTime.ToString(DATE_TIME_FORMAT), customer.Email
                    });
                }
            }
            else if (typeof(T) == typeof(Discount))
            {
                var discount = item as Discount;
                _commonController.AddNewItem(_discounts, discount);
                tblDiscount.Rows.Add(
                    new object[] {
                        discount.DiscountId, discount.Name, discount.StartTime.ToString(DATE_TIME_FORMAT),
                        discount.EndTime.ToString(DATE_TIME_FORMAT), discount.DiscounType,
                        discount.DiscountPercent, $"{discount.DiscountPriceAmount:N0}"
                    }
                );
            } else if(typeof(T) == typeof(BillDetail))
            {
                var billDetail = item as BillDetail;
                if(_commonController.IndexOfItem(_bills, billDetail) >= 0)
                {
                    UpdateItem(item);
                    return;
                }
                _commonController.AddNewItem(_bills, billDetail);
                tblBill.Rows.Add(
                    new object[] {
                        billDetail.BillId, billDetail.Cart?.Customer?.FullName?.ToString(),
                        billDetail.StaffName, billDetail.CreatedTime.ToString("dd/MM/yyyy HH:mm:ss"), 
                        $"{billDetail.TotalItem:N0}",
                        $"{billDetail.SubTotal:N0}",
                        $"{billDetail.TotalDiscountAmount:N0}",
                        $"{billDetail.TotalAmount:N0}",
                        billDetail.Status
                    }
                );
            }
        }

        public void UpdateItem<T>(T updatedItem)
        {
            if (typeof(T) == typeof(Item))
            {
                var newItem = updatedItem as Item;
                int updatedIndex = -1;
                if (_actionType == ActionType.NORMAL)
                {
                    updatedIndex = _commonController.UpdateItem(_items, newItem);
                }
                else
                {
                    updatedIndex = _commonController.UpdateItem(_searchItemResults, newItem);
                    _commonController.UpdateItem(_items, newItem);
                }
                tblItem.Rows.RemoveAt(updatedIndex);
                tblItem.Rows.Insert(updatedIndex,
                    new object[] {
                            newItem.ItemId, newItem.ItemName, newItem.ItemType, newItem.Quantity,
                            newItem.Brand, newItem.ReleaseDate.ToString("dd/MM/yyyy"), $"{newItem.Price:N0}",
                            _itemController.GetDiscountName(newItem.Discount)
                        }
                );
            }
            else if (typeof(T) == typeof(Customer))
            {
                var customer = updatedItem as Customer;
                int updatedIndex = -1;
                if (_actionType == ActionType.NORMAL)
                {
                    updatedIndex = _commonController.UpdateItem(_customers, customer);
                }
                else
                {
                    updatedIndex = _commonController.UpdateItem(_searchCustomerResults, customer);
                    _commonController.UpdateItem(_customers, customer);
                }
                tblCustomer.Rows.RemoveAt(updatedIndex);
                tblCustomer.Rows.Insert(updatedIndex,
                    new object[] {
                        customer.PersonId, customer.FullName?.ToString(), customer.BirthDate.ToString(DATE_FORMAT),
                        customer.Address, customer.PhoneNumber, customer.CustomerType, $"{customer.Point:N0}",
                        customer.CreateTime.ToString(DATE_TIME_FORMAT), customer.Email
                    }
                );
            }
            else if (typeof(T) == typeof(Discount))
            {
                var discount = updatedItem as Discount;
                int updatedIndex = -1;
                if (_actionType == ActionType.NORMAL)
                {
                    updatedIndex = _commonController.UpdateItem(_discounts, discount);
                }
                else
                {
                    updatedIndex = _commonController.UpdateItem(_searchDiscountResults, discount);
                    _commonController.UpdateItem(_discounts, discount);
                }
                tblDiscount.Rows.RemoveAt(updatedIndex);
                tblDiscount.Rows.Insert(updatedIndex,
                    new object[] {
                        discount.DiscountId, discount.Name, discount.StartTime.ToString(DATE_TIME_FORMAT),
                        discount.EndTime.ToString(DATE_TIME_FORMAT), discount.DiscounType,
                        discount.DiscountPercent, $"{discount.DiscountPriceAmount:N0}"
                    }
                );
            }
            else if (typeof(T) == typeof(BillDetail))
            {
                var bill = updatedItem as BillDetail;
                if(_commonController.IndexOfItem(_bills, bill) == -1)
                {
                    AddNewItem(updatedItem);
                    return;
                }
                int updatedIndex = -1;
                if (_actionType == ActionType.NORMAL)
                {
                    updatedIndex = _commonController.UpdateItem(_bills, bill);
                }
                else
                {
                    updatedIndex = _commonController.UpdateItem(_searchedBillResults, bill);
                    _commonController.UpdateItem(_bills, bill);
                }
                tblBill.Rows.RemoveAt(updatedIndex);
                tblBill.Rows.Insert(updatedIndex,
                    new object[] {
                        bill.BillId, bill.Cart?.Customer?.FullName?.ToString(),
                        bill.StaffName, bill.CreatedTime.ToString("dd/MM/yyyy HH:mm:ss"),
                        $"{bill.TotalItem:N0}",
                        $"{bill.SubTotal:N0}",
                        $"{bill.TotalDiscountAmount:N0}",
                        $"{bill.TotalAmount:N0}",
                        bill.Status
                    }
                );
            }
        }

        private void BtnAddNewClick(object sender, EventArgs e)
        {
            if (sender.Equals(btnAddNewItem))
            {
                var childView = new AddEditItemFrm(this, _discounts);
                childView.Show();
            }
        }

        private void TblItemCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == tblItem.Columns["tblItemEdit"].Index)
            {
                Item item = _items[e.RowIndex];
                if (_actionType == ActionType.SEARCH)
                {
                    item = _searchItemResults[e.RowIndex];
                }
                var updateItemView = new AddEditItemFrm(this, _discounts, item);
                updateItemView.Show();
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == tblItem.Columns["tblItemRemove"].Index)
            {
                var title = "Xác nhận xóa";
                var msg = "Bạn có chắc chắn muốn xóa bản ghi này không?";
                var ans = ShowConfirmDialog(msg, title);
                if (ans == DialogResult.Yes)
                {
                    int removedItemIndex = -1;
                    if (_actionType == ActionType.NORMAL)
                    {
                        Item item = _items[e.RowIndex];
                        removedItemIndex = _commonController.DeleteItem(_items, item);
                    }
                    else if (_actionType == ActionType.SEARCH)
                    {
                        Item item = _searchItemResults[e.RowIndex];
                        removedItemIndex = _commonController.DeleteItem(_searchItemResults, item);
                        _commonController.DeleteItem(_items, item);
                    }
                    tblItem.Rows.RemoveAt(removedItemIndex);
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            // tblItemRemove
        }

        private DialogResult ShowConfirmDialog(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void SortItemHandler(object sender, EventArgs e)
        {
            //if (radioSortItemByPriceASC.Checked)
            //{
            //    _commonController.Sort(_items, _itemController.CompareItemByPriceASC);
            //}
            //else if (radioSortItemByPriceDESC.Checked)
            //{
            //    _commonController.Sort(_items, _itemController.CompareItemByPriceDESC);
            //}
            //else if (radioSortItemByQuantity.Checked)
            //{
            //    _commonController.Sort(_items, _itemController.CompareItemByQuantity);
            //}
            //else if (radioSortItemByProductDate.Checked)
            //{
            //    _commonController.Sort(_items, _itemController.CompareItemByDate);
            //}
            //else if (radioSortItemByName.Checked)
            //{
            //    _commonController.Sort(_items, _itemController.CompareItemByName);
            //}

            if (radioSortItemByPriceASC.Equals(sender))
            {
                _commonController.Sort(_items, _itemController.CompareItemByPriceASC);
            }
            else if (radioSortItemByPriceDESC.Equals(sender))
            {
                _commonController.Sort(_items, _itemController.CompareItemByPriceDESC);
            }
            else if (radioSortItemByQuantity.Equals(sender))
            {
                _commonController.Sort(_items, _itemController.CompareItemByQuantity);
            }
            
            else if (radioSortItemByName.Equals(sender))
            {
                _commonController.Sort(_items, _itemController.CompareItemByName);
            }
            ShowItems(_items);
        }

        private void ShowItems(List<Item> items)
        {
            tblItem.Rows.Clear();
            foreach (var item in items)
            {
                tblItem.Rows.Add(new object[]
                {
                    item.ItemId, item.ItemName, item.ItemType, item.Quantity,
                    item.Brand, item.ReleaseDate.ToString("dd/MM/yyyy"), $"{item.Price:N0}",
                    item.Discount == null ? "-" : item.Discount.Name
                });
            }
        }

        private void ComboSearchItemSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboSearchItem.SelectedIndex)
            {
                case 0:
                case 2:
                case 3:
                    txtSearchItem.Enabled = true;
                    numericItemFrom.Enabled = false;
                    numericItemTo.Enabled = false;
                    break;
                case 1:
                case 4:
                    txtSearchItem.Enabled = false;
                    numericItemFrom.Enabled = true;
                    numericItemTo.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void BtnSearchItemClick(object sender, EventArgs e)
        {
            tblItem.Rows.Clear();
            _actionType = ActionType.SEARCH;
            if (comboSearchItem.SelectedIndex == -1)
            {
                var msg = "Vui lòng chọn tiêu chí tìm kiếm để tiếp tục";
                var title = "Lỗi dữ liệu";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (comboSearchItem.SelectedIndex == 0)
            {
                var key = txtSearchItem.Text;
                _searchItemResults = _commonController.Search(_items, _itemController.IsItemNameMatch, key);
                if (_searchItemResults.Count == 0)
                {
                    var msg = "Không tìm thấy kết quả nào.";
                    var title = "Kết quả tìm kiếm";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowItems(_searchItemResults);
                }
            }
            else if (comboSearchItem.SelectedIndex == 1)
            {
                int from = (int)numericItemFrom.Value;
                int to = (int)numericItemTo.Value;
                _searchItemResults = _commonController.Search(_items, _itemController.IsItemPriceMatch, from, to);
                if (_searchItemResults.Count == 0)
                {
                    var msg = "Không tìm thấy kết quả nào.";
                    var title = "Kết quả tìm kiếm";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowItems(_searchItemResults);
                }
            }
            else if (comboSearchItem.SelectedIndex == 2) // loại mặt hàng
            {
                var key = txtSearchItem.Text;
                _searchItemResults = _commonController.Search(_items, _itemController.IsItemTypeMatch, key);
                if (_searchItemResults.Count == 0)
                {
                    var msg = "Không tìm thấy kết quả nào.";
                    var title = "Kết quả tìm kiếm";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowItems(_searchItemResults);
                }
            }
            else if (comboSearchItem.SelectedIndex == 3) // hãng
            {
                var key = txtSearchItem.Text;
                _searchItemResults = _commonController.Search(_items, _itemController.IsItemBrandMatch, key);
                if (_searchItemResults.Count == 0)
                {
                    var msg = "Không tìm thấy kết quả nào.";
                    var title = "Kết quả tìm kiếm";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowItems(_searchItemResults);
                }
            }
            else if (comboSearchItem.SelectedIndex == 4)
            {
                int from = (int)numericItemFrom.Value;
                int to = (int)numericItemTo.Value;
                _searchItemResults = _commonController.Search(_items, _itemController.IsItemQuantityMatch, from, to);
                if (_searchItemResults.Count == 0)
                {
                    var msg = "Không tìm thấy kết quả nào.";
                    var title = "Kết quả tìm kiếm";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ShowItems(_searchItemResults);
                }
            }
        }

        private void BtnRefreshClick(object sender, EventArgs e)
        {
            _actionType = ActionType.NORMAL;
            if (sender.Equals(btnRefreshItem))
            {
                ShowItems(_items);
            }
            else if (sender.Equals(btnRefreshCustomer))
            {
                ShowCustomers(_customers);
            }
            else if (sender.Equals(btnRefreshDiscount))
            {
                ShowDiscounts(_discounts);
            }
        }

        private void BtnAddNewCustomerClick(object sender, EventArgs e)
        {
            var childView = new AddEditCustomerFrm(this, null);
            childView.Show();
            
        }

        private void TblCustomerCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == tblCustomer.Columns["tblCustomerColEdit"].Index)
            {
                Customer customer = _customers[e.RowIndex];
                if (_actionType == ActionType.SEARCH)
                {
                    customer = _searchCustomerResults[e.RowIndex];
                }
                var updateItemView = new AddEditCustomerFrm(this, customer);
                updateItemView.Show();
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == tblCustomer.Columns["tblCustomerColRemove"].Index)
            {
                var title = "Xác nhận xóa";
                var msg = "Bạn có chắc chắn muốn xóa bản ghi này không?";
                var ans = ShowConfirmDialog(msg, title);
                if (ans == DialogResult.Yes)
                {
                    int removedItemIndex = -1;
                    if (_actionType == ActionType.NORMAL)
                    {
                        Customer customer = _customers[e.RowIndex];
                        removedItemIndex = _commonController.DeleteItem(_customers, customer);
                    }
                    else if (_actionType == ActionType.SEARCH)
                    {
                        Customer customer = _searchCustomerResults[e.RowIndex];
                        removedItemIndex = _commonController.DeleteItem(_searchCustomerResults, customer);
                        _commonController.DeleteItem(_customers, customer);
                    }
                    tblCustomer.Rows.RemoveAt(removedItemIndex);
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void RadioSortCustomerCheckedChanged(object sender, EventArgs e)
        {
            if (radioSortCustomerById.Checked)
            {
                _commonController.Sort(_customers, _customerController.CompareCustomerById);
            }
            else if (radioSortCustomerByName.Checked)
            {
                _commonController.Sort(_customers, _customerController.CompareCustomerByName);
            }
            
            else if (radioSortCustomerByBirthDate.Checked)
            {
                _commonController.Sort(_customers, _customerController.CompareCustomerByBirthDate);
            }
            ShowCustomers(_customers);
        }

        private void BtnSearchCustomerClick(object sender, EventArgs e)
        {
            if (comboSearchCustomer.SelectedIndex == -1)
            {
                var title = "Lỗi dữ liệu";
                var msg = "Vui lòng chọn tiêu chí tìm kiếm trước.";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(txtSearchCustomer.Text))
            {
                var title = "Lỗi dữ liệu";
                var msg = "Vui lòng nhập từ khóa cần tìm kiếm trước.";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _actionType = ActionType.SEARCH;
                _searchCustomerResults.Clear();
                switch (comboSearchCustomer.SelectedIndex)
                {
                    case 0:
                        _searchCustomerResults.AddRange(
                            _commonController.Search(
                                _customers,
                                _customerController.IsCustomerNameMatch,
                                txtSearchCustomer.Text
                                )
                            );
                        break;
                    case 1:
                        _searchCustomerResults.AddRange(
                           _commonController.Search(
                               _customers,
                               _customerController.IsCustomerIdMatch,
                               txtSearchCustomer.Text
                               )
                           );
                        break;
                    case 2:
                        _searchCustomerResults.AddRange(
                           _commonController.Search(
                               _customers,
                               _customerController.IsCustomerTypeMatch,
                               txtSearchCustomer.Text
                               )
                           );
                        break;
                    case 3:
                        _searchCustomerResults.AddRange(
                           _commonController.Search(
                               _customers,
                               _customerController.IsCustomerAddressMatch,
                               txtSearchCustomer.Text
                               )
                           );
                        break;
                    case 4:
                        _searchCustomerResults.AddRange(
                           _commonController.Search(
                               _customers,
                               _customerController.IsCustomerPhoneNumberMatch,
                               txtSearchCustomer.Text
                               )
                           );
                        break;
                    default:
                        break;
                }
                ShowCustomers(_searchCustomerResults);
                if (_searchCustomerResults.Count == 0)
                {
                    var title = "Kết quả tìm kiếm";
                    var msg = "Không tìm thấy kết quả nào.";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnAddNewDiscountClick(object sender, EventArgs e)
        {
            var childView = new AddEditDiscountFrm(this);
            childView.Show();
        }

        private void BtnDiscountCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == tblDiscount.Columns["tblDiscountColEdit"].Index)
            {
                Discount discount = _discounts[e.RowIndex];
                if (_actionType == ActionType.SEARCH)
                {
                    discount = _searchDiscountResults[e.RowIndex];
                }
                var updateItemView = new AddEditDiscountFrm(this, discount);
                updateItemView.Show();
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == tblDiscount.Columns["tblDiscountColRemove"].Index)
            {
                var title = "Xác nhận xóa";
                var msg = "Bạn có chắc chắn muốn xóa bản ghi này không?";
                var ans = ShowConfirmDialog(msg, title);
                if (ans == DialogResult.Yes)
                {
                    int removedItemIndex = -1;
                    if (_actionType == ActionType.NORMAL)
                    {
                        Discount discount = _discounts[e.RowIndex];
                        removedItemIndex = _commonController.DeleteItem(_discounts, discount);
                    }
                    else if (_actionType == ActionType.SEARCH)
                    {
                        Discount discount = _searchDiscountResults[e.RowIndex];
                        removedItemIndex = _commonController.DeleteItem(_searchDiscountResults, discount);
                        _commonController.DeleteItem(_discounts, discount);
                    }
                    tblDiscount.Rows.RemoveAt(removedItemIndex);
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnSearchDiscountClick(object sender, EventArgs e)
        {
            if (comboSearchDiscount.SelectedIndex == -1)
            {
                var title = "Lỗi tìm kiếm";
                var message = "Vui lòng chọn tiêu chí tìm kiếm trước.";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(txtSearchDiscount.Text))
            {
                var title = "Lỗi tìm kiếm";
                var message = "Vui lòng nhập giá trị cần tìm trước.";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var key = txtSearchDiscount.Text;
                _actionType = ActionType.SEARCH;
                _searchDiscountResults.Clear();
                switch (comboSearchDiscount.SelectedIndex)
                {
                    case 0:
                        _searchDiscountResults.AddRange(
                            _commonController.Search(
                                _discounts,
                                _discountController.IsStartTimeMatch,
                                key
                                )
                            );
                        break;
                    case 1:
                        _searchDiscountResults.AddRange(
                            _commonController.Search(
                                _discounts,
                                _discountController.IsEndTimeMatch,
                                key
                                )
                            );
                        break;
                    case 2:
                        _searchDiscountResults.AddRange(
                             _commonController.Search(
                                 _discounts,
                                 _discountController.IsDiscountNameMatch,
                                 key
                                 )
                             );
                        break;
                }
            }
            ShowDiscounts(_searchDiscountResults);
            if (_searchDiscountResults.Count == 0)
            {
                var title = "Kết quả tìm kiếm";
                var msg = "Không tìm thấy kết quả nào.";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddNewBillClick(object sender, EventArgs e)
        {
            var createBillView = new AddEditBillFrm(this, _customers, _items, _bills, _commonController);
            createBillView.Show();
        }

        public void Remove<T>(T item)
        {
            if(typeof(T) == typeof(BillDetail))
            {
                ShowBills(_bills);
            }
        }

        private void TbBillCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == tblBill.Columns["tblBillColViewDetail"].Index)
            {
                var bill = _bills[e.RowIndex];
                var createBillView = new AddEditBillFrm(this, _customers, _items, _bills, _commonController, bill);
                createBillView.Show();
            }
        }

        private void BtnRefreshBillClick(object sender, EventArgs e)
        {
            ShowBills(_bills);
        }

        private void ShowBills(List<BillDetail> bills)
        {
            tblBill.Rows.Clear();
            foreach (var bill in bills)
            {
                var billDetail = bill;
                // update
                tblBill.Rows.Add(
                    new object[] {
                        billDetail.BillId, billDetail.Cart?.Customer?.FullName?.ToString(),
                        billDetail.StaffName, billDetail.CreatedTime.ToString("dd/MM/yyyy HH:mm:ss"),
                        $"{billDetail.TotalItem:N0}",
                        $"{billDetail.SubTotal:N0}",
                        $"{billDetail.TotalDiscountAmount:N0}",
                        $"{billDetail.TotalAmount:N0}",
                        billDetail.Status
                    }
                );
            }
        }

        private void MenuItemSaveDataClick(object sender, EventArgs e)
        {
            _ioController.SaveDataList(_items, _customers, _discounts, _bills);
            var title = "Thông báo";
            var message = "Lưu file dữ liệu ra thành công";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SortBill(object sender, EventArgs e)
        {
            switch(comboSortBill.SelectedIndex)
            {
                case 0:
                    _commonController.Sort(_bills, _billSorter.CompareBillByTotalItems);
                    break;
                case 1:
                    _commonController.Sort(_bills, _billSorter.CompareBillByTotalAmount);
                    break;
                case 2:
                    _commonController.Sort(_bills, _billSorter.CompareBillByCreatedDateTimeASC);
                    break;
                case 3:
                    _commonController.Sort(_bills, _billSorter.CompareBillByCreatedDateTimeDESC);
                    break;
                case 4:
                    _commonController.Sort(_bills, _billSorter.CompareBillByCustomerName);
                    break;
            }
            ShowBills(_bills);
        }

        private void BtnSearchBillClick(object sender, EventArgs e)
        {
            if(comboSearchBill.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn tiêu chí tìm kiếm trước!", "Lỗi tìm kiếm",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if(txtSearchBill.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập giá trị cần tìm kiếm trước!", "Lỗi tìm kiếm",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                BillFinder finder = new BillFinder();
                _actionType = ActionType.SEARCH;
                _searchedBillResults.Clear();
                switch (comboSearchBill.SelectedIndex)
                {
                    case 0:         
                        _searchedBillResults.AddRange(
                            _commonController.Search(
                                _bills, finder.IsBillCreatedTimeMatch, txtSearchBill.Text));
                        break;
                    case 1:
                        _searchedBillResults.AddRange(
                            _commonController.Search(
                                _bills, finder.IsBillCustomerNameMatch, txtSearchBill.Text));
                        break;
                    case 2:
                        _searchedBillResults.AddRange(
                            _commonController.Search(
                                _bills, finder.IsBillStatusMatch, txtSearchBill.Text));
                        break;
                    case 3:
                        _searchedBillResults.AddRange(
                            _commonController.Search(
                                _bills, finder.IsBillStaffNameMatch, txtSearchBill.Text));
                        break;
                }
                ShowBills(_searchedBillResults);
                if(_searchedBillResults.Count == 0)
                {
                    var title = "Kết quả tìm kiếm";
                    var msg = "Không tìm thấy kết quả nào!";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
                this.Close();
            
            
        }
        #endregion
       
        

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void HomeFrm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Calculater calculater = new Calculater();
            calculater.Show();
        }
    }
}
