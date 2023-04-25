using System;
using System.Drawing;
using System.Windows.Forms;
using Controllers;
using Models;

namespace CSharpCourseFinalProject
{
    public partial class AddEditCustomerFrm : Form
    {
        private Customer _customer;
        private IViewController _controller;

        public AddEditCustomerFrm()
        {
            InitializeComponent();
            CenterToParent();
        }

        public AddEditCustomerFrm(IViewController controller, Customer customer=null) : this()
        {
            _customer = customer;
            _controller = controller;
            if(customer != null)
            {
                Text = "SỬA THÔNG TIN KHÁCH HÀNG";
                btnAddNew.Text = "Cập nhật";
                ShowCustomerInfo();
            }
        }

        private void ShowCustomerInfo()
        {
            txtCustomerId.Text = _customer.PersonId;
            txtCustomerId.Enabled = false;
            txtFullName.Text = _customer.FullName.ToString();
            txtAddress.Text = _customer.Address;
            txtEmail.Text = _customer.Email;
            txtPhoneNumber.Text = _customer.PhoneNumber;
            numericPoint.Value = _customer.Point;
            datePickerBirthDate.Value = _customer.BirthDate;
            datePickerCreatedAcc.Value = _customer.CreateTime;
            datePickerCreatedAcc.Enabled = false;
            for (int i = 0; i < comboCustomerType.Items.Count; i++)
            {
                if (_customer.CustomerType.CompareTo(comboCustomerType.Items[i]) == 0)
                {
                    comboCustomerType.SelectedIndex = i;
                    break;
                }
            }
        }

        private void ClearCustomerIdHintHandler(object sender, MouseEventArgs e)
        {
            txtCustomerId.Text = "";
        }

        private void TxtIdTakeForcus(object sender, EventArgs e)
        {
            txtCustomerId.Text = "";
            txtCustomerId.ForeColor = Color.Black;
        }

        private void TxtIdLeaveFocus(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtCustomerId.Text))
            {
                txtCustomerId.Text = "CMND/CCCD";
                txtCustomerId.ForeColor = Color.Gray;
            } else
            {
                txtCustomerId.ForeColor= Color.Black;
            }
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            var title = "Xác nhận";
            var message = "Bạn có chắc muốn hủy bỏ?";
            var ans = ShowConfirmMessage(title, message);
            if (ans == DialogResult.Yes)
            {
                Dispose();
            }
        }

        private DialogResult ShowConfirmMessage(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void BtnAddUpdateClick(object sender, EventArgs e)
        {
            var customerController = new CustomerController();
            try
            {
                if (!customerController.IsNameValid(txtFullName.Text))
                {
                    throw new InvalidNameException("Họ và tên không hợp lệ.", txtFullName.Text);
                }
                if (!customerController.IsEmailValid(txtEmail.Text))
                {
                    throw new InvalidEmailException("Email không hợp lệ.", txtEmail.Text);
                }
                if (!customerController.IsPhoneValid(txtPhoneNumber.Text))
                {
                    throw new InvalidPhoneNumberException("Số điện thoại không hợp lệ.", txtPhoneNumber.Text);
                }
                if(btnAddNew.Text.CompareTo("Cập nhật") != 0)
                {
                    var id = txtCustomerId.Text;
                    var name = txtFullName.Text;
                    var address = txtAddress.Text;
                    var email = txtEmail.Text;
                    var phone = txtPhoneNumber.Text;
                    var birthDate = datePickerBirthDate.Value;
                    var createdAccTime = DateTime.Now;
                    var point = (int)numericPoint.Value;
                    var customerType = comboCustomerType.Text;
                    var customer = new Customer(customerType, point, createdAccTime, email);
                    customer.PersonId = id;
                    customer.FullName = new FullName(name);
                    customer.Address = address;
                    customer.PhoneNumber = phone;
                    customer.BirthDate = birthDate;
                    customer.CreateTime = createdAccTime;
                    _controller.AddNewItem(customer);
                } else
                {
                    _customer.Address = txtAddress.Text;
                    _customer.PhoneNumber = txtPhoneNumber.Text;
                    _customer.FullName = new FullName(txtFullName.Text);
                    _customer.Point = (int)numericPoint.Value;
                    _customer.BirthDate = datePickerBirthDate.Value;
                    _customer.CustomerType = comboCustomerType.Text;
                    _customer.Email = txtEmail.Text;
                    var title = "Xác nhận";
                    var msg = "Bạn có chắc chắn muốn lưu các thay đổi?";
                    var ans = ShowConfirmMessage(title, msg);
                    if (ans == DialogResult.Yes)
                    {
                        _controller.UpdateItem(_customer);
                        Dispose();
                    }
                }
            }
            catch (InvalidNameException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (InvalidEmailException ex)
            {
                ShowErrorMessage(ex.Message);
            }
            catch (InvalidPhoneNumberException ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private void ShowErrorMessage(string msg)
        {
            var title = "Lỗi dữ liệu";
            MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
