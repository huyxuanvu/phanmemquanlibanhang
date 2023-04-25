using System;
using System.Collections.Generic;
using Controllers;
using Models;
using System.Windows.Forms;

namespace CSharpCourseFinalProject
{
    public partial class AddEditBillFrm : Form
    {
        private List<Item> _items;
        private List<Customer> _customer;
        private IViewController _controller;
        private BillDetail _bill;
        private List<Customer> _searchedCustomerResults;
        private List<Item> _searchedItemResults;
        private IBillController _billController;
        private bool _isEditing = false;
        private int _selectedIndex = -1;
        private bool _isUpdateBill;
        private CommonController _commonController;
        private List<BillDetail> _bills;

        public AddEditBillFrm()
        {
            InitializeComponent();
            CenterToParent();
            _billController = new BillController();
            _searchedCustomerResults = new List<Customer>();
            _searchedItemResults = new List<Item>();
        }

        public AddEditBillFrm(IViewController controller,
            List<Customer> customers, List<Item> items, 
            List<BillDetail> bills, CommonController commonController,
            BillDetail bill=null) : this()
        {
            _controller = controller;
            _items = items;
            _bills = bills;
            _customer = customers;
            _commonController = commonController;
            if(bill != null)
            {
                _isUpdateBill = true;
                _bill = bill;
                ShowBillData();
            } else
            {
                _bill = new BillDetail(0);
                _isUpdateBill = false;
            }
        }

        private void ShowBillData()
        {
            // update
            tblBillDetail.Rows.Clear();
            foreach (var item in _bill.Cart.SelectedItems)
            {
                tblBillDetail.Rows.Add(
                    new object[]
                    {
                        _bill.BillId, item.ItemId, item.ItemName, $"{item.NumberOfSelectedItem:N0}",
                        $"{item.Price:N0}", $"{item.PriceAfterDiscount:N0}",
                        $"{item.NumberOfSelectedItem * item.PriceAfterDiscount:N0}"
                    }
                );
            }
            txtSearchCustomer.Text = _bill.Cart?.Customer?.FullName.ToString();
            labelCreatedTime.Text = _bill.CreatedTime.ToString("dd/MM/yyyy HH:mm:ss");
            txtStaff.Text = _bill.StaffName;
            ShowTotalInfo();
        }

        private void BtnPayClick(object sender, EventArgs e)
        {
            var paymentView = new PaymentFrm(_controller, _bill);
            paymentView.Show();
        }

        private void BtnSearchCustomerClick(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtSearchCustomer.Text))
            {
                var msg = "Vui lòng nhập tên khách hàng cần tìm.";
                var title = "Lỗi tìm kiếm";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                _searchedCustomerResults = _commonController.Search(_customer, 
                    new CustomerController().IsCustomerNameMatch, txtSearchCustomer.Text);
                tblSearchedCustomer.Rows.Clear();
                foreach (var customer in _searchedCustomerResults)
                {
                    tblSearchedCustomer.Rows.Add(new object[]
                    {
                            customer.PersonId, customer.FullName.ToString()
                    });
                }
                if (_searchedCustomerResults.Count == 0)
                {
                    var msg = "Không tìm thấy kết quả nào.";
                    var title = "Kết quả tìm kiếm";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnSearchItemClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchItem.Text))
            {
                var msg = "Vui lòng nhập tên mặt hàng cần tìm.";
                var title = "Lỗi tìm kiếm";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _searchedItemResults = _commonController.Search(_items,
                    new ItemController().IsItemNameMatch, txtSearchItem.Text);
                
                ShowSearchItemResult();
                if (_searchedItemResults.Count == 0)
                {
                    var msg = "Không tìm thấy kết quả nào.";
                    var title = "Kết quả tìm kiếm";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ShowSearchItemResult()
        {
            tblSearchedItem.Rows.Clear();
            foreach (var item in _searchedItemResults)
            {
                tblSearchedItem.Rows.Add(new object[]
                {
                            item.ItemId, item.ItemName, $"{item.Quantity:N0}"
                });
            }
        }

        private void TblCustomerCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex == tblSearchedCustomer.Columns["tblCustomerColSelect"].Index)
            {
                _bill.Cart.Customer = _searchedCustomerResults[e.RowIndex];
                ShowTotalInfo();
            }
        }

        private void ShowTotalInfo()
        {
            labelCustomerName.Text = $"Tên KH: {_bill.Cart?.Customer?.FullName?.ToString()}";
            labelTotalItem.Text = $"Tổng SP: {_bill.TotalItem:N0}sp";
            labelTotalAmount.Text = $"Tổng tiền: {_bill.TotalAmount:N0}đ";
            labelTotalDiscount.Text = $"Tổng KM: {_bill.TotalDiscountAmount:N0}đ";
        }

        private void TblItemCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == tblSearchedItem.Columns["tblItemColSelect"].Index)
            {
                SelectedItem item = new SelectedItem(_searchedItemResults[e.RowIndex]);
                item.NumberOfSelectedItem = (int)numericSelectedQuantity.Value;
                if(item.NumberOfSelectedItem > item.Quantity)
                {
                    var title = "Lỗi dữ liệu";
                    var msg = "Số lượng hàng cần mua vượt quá số lượng hiện có.";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } else
                {
                    if (item.NumberOfSelectedItem > 0)
                    {
                        item.Quantity -= item.NumberOfSelectedItem;
                        numericSelectedQuantity.Value = 0;
                        _billController.UpdateBill(_bill, item);
                        // update
                        ShowBillData();
                        ShowTotalInfo();
                        _searchedItemResults[e.RowIndex].Quantity = item.Quantity;
                        ShowSearchItemResult();
                    }
                    else
                    {
                        ShowErrorNumberOfSelectedItem();
                    }
                }
            }
        }

        private void ShowErrorNumberOfSelectedItem()
        {
            var title = "Lỗi dữ liệu";
            var msg = "Vui lòng nhập số mặt hàng khách chọn khác 0";
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowBillDetail(SelectedItem item)
        {
            tblBillDetail.Rows.Add(new object[]
            {
                _bill.BillId, item.ItemId, item.ItemName, $"{item.NumberOfSelectedItem:N0}",
                $"{item.Price:N0}", $"{item.PriceAfterDiscount:N0}", 
                $"{item.NumberOfSelectedItem * item.PriceAfterDiscount:N0}"
            });
        }

        private void BtnUpdateSelectedItemCLick(object sender, EventArgs e)
        {
            if(_isEditing && _selectedIndex >= 0)
            {
                var item = _bill.Cart.SelectedItems[_selectedIndex];
                var newValue = (int)numericSelectedQuantity.Value;
                if(newValue == 0)
                {
                    ShowErrorNumberOfSelectedItem();
                } else
                {
                    item.NumberOfSelectedItem = newValue;
                    // update
                    _billController.UpdateBill(_bill, item, true);
                    ShowTotalInfo();
                    tblBillDetail.Rows.RemoveAt(_selectedIndex);
                    tblBillDetail.Rows.Insert(_selectedIndex, new object[]
                        {
                            _bill.BillId, item.ItemId, item.ItemName, $"{item.NumberOfSelectedItem:N0}",
                            $"{item.Price:N0}", $"{item.PriceAfterDiscount:N0}",
                            $"{item.NumberOfSelectedItem * item.PriceAfterDiscount:N0}"
                        }
                    );
                    var msg = $"Cập nhật mặt hàng \"{item.ItemName}\" thành công.";
                    var title = "Cập nhật thành công";
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _isEditing = false;
                    _selectedIndex = -1;
                }
            }
        }

        private void TblBillDetailCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == tblBillDetail.Columns["tblBillDetailColEdit"].Index)
            {
                _isEditing = true;
                _selectedIndex = e.RowIndex;
                numericSelectedQuantity.Value = _bill.Cart.SelectedItems[e.RowIndex].NumberOfSelectedItem;
            } else if(e.RowIndex >= 0 && e.RowIndex < _bill.Cart.SelectedItems.Count &&
                e.ColumnIndex == tblBillDetail.Columns["tblBillDetailColRemove"].Index)
            {
                var tilte = "Xác nhận xóa";
                var msg = "Bạn có chắc muốn xóa bản ghi này không?";
                var ans = MessageBox.Show(msg, tilte, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(ans == DialogResult.Yes)
                {
                    _billController.RemoveItem(_bill, e.RowIndex);
                    tblBillDetail.Rows.RemoveAt(e.RowIndex);
                    ShowTotalInfo();
                    MessageBox.Show("Xoá thành công", "Kết quả xóa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // sự kiện nút lưu hóa đơn đc click
        private void BtnSaveClick(object sender, EventArgs e)
        {
            _bill.CreatedTime = DateTime.Now;
            if(string.IsNullOrEmpty(_bill.Status))
            {
                _bill.Status = "Đang xử lý";
            }
            if(_isUpdateBill) // cập nhật
            {
                _controller.UpdateItem(_bill);
            } else // thêm mới
            {
                _controller.AddNewItem(_bill);
            }
            Dispose();
        }

        private void UpdateStaffInfo(object sender, EventArgs e)
        {
            _bill.StaffName = txtStaff.Text;
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            var tilte = "Xác nhận hủy hóa đơn";
            var msg = "Bạn có chắc muốn hủy bỏ hóa đơn này không?";
            var ans = MessageBox.Show(msg, tilte, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ans == DialogResult.Yes)
            {
                _bill.Status = "Đã hủy";
                _bill.CreatedTime = DateTime.Now;
                _controller.UpdateItem(_bill);
                Dispose();
            }
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            var tilte = "Xác nhận xóa";
            var msg = "Bạn có chắc muốn xóa bản ghi này không?";
            var ans = MessageBox.Show(msg, tilte, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ans == DialogResult.Yes)
            {
                _commonController.DeleteItem(_bills, _bill);
                _controller.Remove(_bill);
                MessageBox.Show("Xoá thành công", "Kết quả xóa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Dispose();
            }
        }
    }
}
