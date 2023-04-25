using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpCourseFinalProject
{
    public partial class Calculater : Form
    {
        public Calculater()
        {
            InitializeComponent();
            Voidload();
            list = new List<string>();
        }

        private void unLock()
        {
            btnCong.Enabled = false;
            btnChia.Enabled = false;
            btnNhan.Enabled = false;
            btnTru.Enabled = false;

        }
        private void Nolock()
        {
            btnCong.Enabled = true;
            btnChia.Enabled = true;
            btnNhan.Enabled = true;
            btnTru.Enabled = true;

        }


        private void maytinh_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }
        List<Button> buttons;
        List<string> list;
        private void Voidload()
        {
            buttons = new List<Button>() { btn0,bn1,btn2,btn3,btn4,btn5,btn6,btn7,btn8,btn9,btnChia,btnCong,btnTru,btnNhan};
            foreach( var item in buttons )
            {
                item.Click += new EventHandler(ButtonClick);
            }
        }
        private void sum()
        {
            if (list.Contains("+"))
            {
                list.Remove("+");
                double s = Convert.ToDouble(list[0]) + Convert.ToDouble(list[1]);
                textBox1.Text = s.ToString();
            }


        }
        private void ButtonClick(object sender, EventArgs e)
        {
            string textButton = (sender as  Button).Text;
            textBox1.Text += textButton;
        }
        private void tich()
        {
            if (list.Contains("*"))
            {
                list.Remove("*");
                double s = double.Parse(list[0]) * double.Parse(list[1]);
                textBox1.Text = s.ToString();
            }

        }
        private void chia()
        {
            if (list.Contains("/"))
            {
                list.Remove("/");
                double s = Convert.ToDouble(list[0]) / Convert.ToDouble(list[1]);
                textBox1.Text = s.ToString();
            }

        }
        private void tru()
        {
            if (list.Contains("-"))
            {
                list.Remove("-");
                double s = Convert.ToDouble(list[0]) - Convert.ToDouble(list[1]);
                textBox1.Text = s.ToString();
            }

        }




        private void btnCong_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                list.Add(textBox1.Text);
                list.Add(btnCong.Text);
                textBox1.Text = "";
                unLock();
            }
        }

        private void btnBang_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            { 
                list.Add(textBox1.Text);
                
                tich();
                chia();
                sum();
                tru();                
                Nolock();
                list.Clear();
            }
        }

        private void btnNhan_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                list.Add(textBox1.Text);
                list.Add(btnNhan.Text);
                textBox1.Text = "";
                unLock();
            }
        }

        private void btnTru_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                list.Add(textBox1.Text);
                list.Add(btnTru.Text);

                textBox1.Text = "";
                unLock();
            }
        }

        private void btnChia_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                list.Add(textBox1.Text);
                list.Add(btnChia.Text);

                textBox1.Text ="";
                unLock();
            }
        }

        private void btnPhay_Click(object sender, EventArgs e)
        {
            list.Clear();
            textBox1.Text = "";
            Nolock();
        }
    }
}
