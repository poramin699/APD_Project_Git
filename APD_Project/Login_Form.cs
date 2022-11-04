using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APD_Project
{
    public partial class Login_Form : Form
    {
        ComShopEntities _context = new ComShopEntities();
        public Login_Form()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Text = "กำลังตรวจสอบ ";
                var user = _context.P_Employee.Where(u => u.emp_username == txb_username.Text && u.emp_password == txb_password.Text).First();

                MessageBox.Show("ยินดีต้อนรับคุณ " + user.emp_username, "เข้าสู่ระบบสำเร็จ");
                this.Hide();
                button1.Text = "เข้าสู่ระบบ";

                Program_Form programForm;
                if (user.depart_id == 1)
                {
                    programForm = new Program_Form(user.emp_username, "เจ้าของร้าน", this);
                    programForm.Show();
                }
                else if(user.depart_id == 2)
                {
                    programForm = new Program_Form(user.emp_username, "พนักงานคลังสินค้า", this);
                    programForm.Show();
                }
                else if (user.depart_id == 3)
                {
                    programForm = new Program_Form(user.emp_username, "พนักงานขายหน้าร้าน", this);
                    programForm.Show();
                }
            }
            catch(InvalidOperationException)
            {
                button1.Text = "เข้าสู่ระบบ";
                MessageBox.Show("ชื่อผู้ใช้ หรือ รหัสผ่าน ไม่ถูกต้อง !", "เข้าสู่ระบบสำเร็จ");
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                button1.Text = "เข้าสู่ระบบ";
                MessageBox.Show("ไม่สามารถเชื่อมต่ออินเตอร์เน็ต", "เกิดข้อผิดพลาด",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

        }

        private void Login_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Login_Form_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        Point lastpoint;
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            lastpoint = new Point(e.X, e.Y);
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastpoint.X;
                this.Top += e.Y - lastpoint.Y;
            }
        }
    }
}
