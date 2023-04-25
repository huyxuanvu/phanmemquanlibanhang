using Models;
using System;
using System.Windows.Forms;

namespace CSharpCourseFinalProject
{
    public partial class PaymentFrm : Form
    {
        private IViewController _controller;
        private BillDetail _bill;
        private DateTime _createdTime;

        public PaymentFrm()
        {
            InitializeComponent();
            CenterToParent();
            _createdTime = DateTime.Now; 
        }

        public PaymentFrm(IViewController controller, BillDetail bill) : this()
        {
            _controller = controller;
            _bill = bill;
            ShowData();
        }

        private void ShowData()
        {
            txtBillId.Text = _bill.BillId + "";
            txtCustomerName.Text = _bill.Cart?.Customer?.FullName.ToString();
            txtStaffName.Text = _bill.StaffName;
            txtCreatedTime.Text = _createdTime.ToString("dd/MM/yyyy HH:mm:ss");
            txtTotalItem.Text = $"{_bill.TotalItem}sp";
            txtTotalDiscount.Text = $"{_bill.TotalDiscountAmount}đ";
            txtTotalAmount.Text = $"{_bill.TotalAmount:N0}đ";
        }

        private void BtnFinishClick(object sender, EventArgs e)
        {
            if(comboPaymentMethod.SelectedIndex == -1)
            {
                var title = "Lỗi thanh toán";
                var msg = "Chọn hình thức thanh toán trước!";
                MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else
            {
                _bill.Status = "Đã thanh toán";
                _bill.CreatedTime = _createdTime;
                _bill.PaymentMethod = comboPaymentMethod.Text;
                _controller.AddNewItem(_bill);
                Dispose();
            }
        }
    }
}
