using AForge.Video.DirectShow;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using ZXing;
using ZXing.QrCode;

namespace APD_Project
{
    public partial class Program_Form : Form
    {
        ComShopEntities _context = new ComShopEntities();
        Form ParentForm;

        FilterInfoCollection webcams;
        VideoCaptureDevice videoIn;
        public Program_Form(String username, String depart, Form parent)
        {
            InitializeComponent();
            this.ParentForm = parent;
            this.Text = "CompShop ผู้ใช้ : " + username + " ประเภท : " + depart;
            label19.Text = "ผู้ใช้ : " + username + " ประเภท : " + depart;

            if(depart == "เจ้าของร้าน")
            {
                Tab_Panel.Controls.Remove(promotion_page);
                Tab_Panel.Controls.Remove(Cart_Page);
                DisableCRUD_Product();

                label25.Visible = false;
                groupBox8.Visible = false;
                groupBox6.Location = new Point(268, 377);
                groupBox6.Anchor = AnchorStyles.Bottom;

            }
            else if(depart == "พนักงานคลังสินค้า")
            {
                AddProductToDB addProductToDB = new AddProductToDB();
                Tab_Panel.Controls.Remove(EmpData_Page);
                Tab_Panel.Controls.Remove(Customer_page);
                Tab_Panel.Controls.Remove(Cart_Page);
                label25.Visible = false;
                groupBox8.Visible = false;
                button26.Visible = true;
                textBox15.Visible = true;
                groupBox6.Anchor = AnchorStyles.Bottom;
                groupBox17.Visible = true;

                //addProductToDB.Location = new Point(500, 376);
                product_page.Controls.Add(addProductToDB);
            }
            else if(depart == "พนักงานขายหน้าร้าน")
            {
                groupBox16.Visible = true;
                Tab_Panel.Controls.Remove(EmpData_Page);
                DisableCRUD_Product();
            }

            webcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in webcams)
            {
                comboBox5.Items.Add(info.Name);
            }
        }

        private void DisableCRUD_Product()
        {
            button18.Visible = false;
            button19.Visible = false;
            button20.Visible = false;
            textBox11.ReadOnly = true;
            comboBox6.Enabled = false;
            textBox14.ReadOnly = true;
            textBox26.ReadOnly = true;
            textBox8.ReadOnly = true;

            textBox11.BackColor = Color.White;
            comboBox6.BackColor = Color.White;
            textBox14.BackColor = Color.White;
            textBox26.BackColor = Color.White;
            textBox8.BackColor = Color.White;
        }
        
        

        private void Owner_Form_Load(object sender, EventArgs e)
        {
            pEmployeeBindingSource.DataSource = _context.P_Employee
                .Select(s => new { s.emp_id, s.emp_name, s.emp_phone, s.emp_username, s.emp_password, s.P_Department.department }).ToList();

            pMemberBindingSource.DataSource = _context.P_Member.ToList();
            pPromotionBindingSource.DataSource = _context.P_Promotion.Select(s => new { s.pro_id, s.discount, s.product_id_1, s.product_id_2, p1 = s.P_Product.p_name, p2 = s.P_Product1.p_name }).ToList();

            var Product_List = _context.P_Product.ToList();
            pProductBindingSource.DataSource = Product_List;

            
            var Department = _context.P_Department.ToList();
            pDepartmentBindingSource.DataSource = Department;

            var productType = Product_List.Select(x => x.p_type).Distinct().ToArray();
            comboBox2.Items.Add("ทั้งหมด");
            comboBox2.SelectedItem = "ทั้งหมด";
            comboBox2.Items.AddRange(productType);
            comboBox6.Items.AddRange(productType);
            comboBox7.Items.AddRange(productType);
            
            foreach (var depart in Department)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem { 
                    Value = depart.depart_id,
                    Text = depart.department
                };
                comboBox4.Items.Add(comboBoxItem);
            }
        }

        private void ออกจากระบบToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Login_Form login_Form = new Login_Form();
            this.Close();
        }

        private void Owner_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioAll.Checked)
            {
                pEmployeeBindingSource.DataSource = _context.P_Employee
                    .Where(s => s.emp_id.ToString() == tbx_Search.Text || s.emp_name.Contains(tbx_Search.Text) ||
                    s.emp_username.Contains(tbx_Search.Text) || s.emp_phone.Contains(tbx_Search.Text)).ToList();
            }
            else if (radioTel.Checked)
            {
                pEmployeeBindingSource.DataSource = _context.P_Employee.Where(s => s.emp_phone.Contains(tbx_Search.Text)).ToList();
            }
            else if (radioUsername.Checked)
            {
                pEmployeeBindingSource.DataSource = _context.P_Employee.Where(s => s.emp_username.Contains(tbx_Search.Text)).ToList();
            }
            else if (radioName.Checked)
            {
                pEmployeeBindingSource.DataSource = _context.P_Employee.Where(s => s.emp_name.Contains(tbx_Search.Text)).ToList();
            }
            else
            {
                pEmployeeBindingSource.DataSource = _context.P_Employee.Where(s => s.emp_id.ToString().Contains(tbx_Search.Text)).ToList();
            }

            if (tbx_Search.Text == "")
            {
                pEmployeeBindingSource.DataSource = _context.P_Employee
                .Select(s => new { s.emp_id, s.emp_name, s.emp_phone, s.emp_username, s.emp_password, s.P_Department.department }).ToList();
            }
        }

        private void btn_MemSearch_Click(object sender, EventArgs e)
        {
            if (radioMemAll.Checked)
            {
                pMemberBindingSource.DataSource = _context.P_Member
                    .Where(s => s.mem_id.ToString() == txb_memSearch.Text || s.mem_name.Contains(txb_memSearch.Text) || s.mem_phone.Contains(txb_memSearch.Text)).ToList();
            }
            else if (radioMemTel.Checked)
            {
                pMemberBindingSource.DataSource = _context.P_Member.Where(s => s.mem_phone == txb_memSearch.Text).ToList();
            }
            else if (radioMemName.Checked)
            {
                pMemberBindingSource.DataSource = _context.P_Member.Where(s => s.mem_name == txb_memSearch.Text).ToList();
            }
            else if (radioMemID.Checked)
            {
                pMemberBindingSource.DataSource = _context.P_Member.Where(s => s.mem_id.ToString() == txb_memSearch.Text).ToList();
            }

            if (txb_memSearch.Text == "")
            {
                pMemberBindingSource.DataSource = _context.P_Member.ToList();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox4.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox5.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                comboBox1.SelectedValue = _context.P_Employee.Where(s => s.emp_id.ToString() == textBox1.Text).Select(s => s.depart_id).First();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e){}

        private void button2_Click(object sender, EventArgs e)
        {
            int empid = int.Parse(textBox1.Text);
            var emp = _context.P_Employee.SingleOrDefault(s => s.emp_id == empid);
            if(emp != null)
            {
                emp.emp_name = textBox2.Text;
                emp.emp_phone = textBox4.Text;
                emp.emp_username = textBox5.Text;
                emp.emp_password = textBox3.Text;
                emp.depart_id = int.Parse(comboBox1.SelectedValue.ToString());

                int r = _context.SaveChanges();
                MessageBox.Show("บันทึกการแก้ไข " + r.ToString() + " รายการ");

                pEmployeeBindingSource.DataSource = _context.P_Employee
                .Select(s => new { s.emp_id, s.emp_name, s.emp_phone, s.emp_username, s.emp_password, s.P_Department.department }).ToList();
            }
            
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e){}

        private void button5_Click(object sender, EventArgs e)
        {
            int memid = int.Parse(textBox10.Text);
            var mem = _context.P_Member.SingleOrDefault(s => s.mem_id == memid);
            if(mem != null)
            {
                mem.mem_name = textBox9.Text;
                mem.mem_phone = textBox7.Text;

                int r = _context.SaveChanges();
                MessageBox.Show("บันทึกการแก้ไข " + r.ToString() + " รายการ");

                pMemberBindingSource.DataSource = _context.P_Member.ToList();
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            var product = new List<P_Product>();
            string search = textBox6.Text;

            if (comboBox2.Text == "ทั้งหมด") 
            { 
                product = _context.P_Product.ToList();
            }
            else
            {
                product = _context.P_Product.Where(s => s.p_type == comboBox2.Text).ToList();
            }
            

            if (radioButton2.Checked)
            {
                pProductBindingSource.DataSource = product.Where(s => s.p_id.ToString().Contains(search)).ToList();
            }
            else if (radioButton4.Checked)
            {
                pProductBindingSource.DataSource = product.Where(s => s.p_name.Contains(search)).ToList();
            }
            else if (radioButton3.Checked)
            {
                pProductBindingSource.DataSource = product.Where(s => s.p_detail.Contains(search)).ToList();
            }
            else
            {
                pProductBindingSource.DataSource = product;
            }
        }

        Point lastpoint;
        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastpoint.X;
                this.Top += e.Y - lastpoint.Y;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            lastpoint = new Point(e.X, e.Y);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (videoIn != null && videoIn.IsRunning)
            {
                videoSourcePlayer1.Stop();
            }
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Owner_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("ออกจากระบบหรือไม่ ?", "ออกจากระบบ", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.ParentForm.Show();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView3.SelectedRows.Count > 0)
            {
                textBox12.Text = dataGridView3.SelectedRows[0].Cells[0].Value.ToString();
                textBox11.Text = dataGridView3.SelectedRows[0].Cells[1].Value.ToString();
                comboBox6.Text = dataGridView3.SelectedRows[0].Cells[5].Value.ToString();
                textBox8.Text = dataGridView3.SelectedRows[0].Cells[2].Value.ToString();
                textBox14.Text = dataGridView3.SelectedRows[0].Cells[4].Value.ToString();
                label13.Text = double.Parse(dataGridView3.SelectedRows[0].Cells[3].Value.ToString()).ToString("#.00");
                textBox26.Text = double.Parse(dataGridView3.SelectedRows[0].Cells[3].Value.ToString()).ToString("#.00");

                pictureBox1.Image = ImageLoad.LoadImage(dataGridView3.SelectedRows[0].Cells[6].Value.ToString());

                //qr code gen
                QrCodeEncodingOptions options = new QrCodeEncodingOptions();
                BarcodeWriter writer = new BarcodeWriter();
                options.CharacterSet ="UTF-8";
                options.Width = pictureBox2.Width;
                options.Height = pictureBox2.Height;
                writer.Options = options;
                writer.Format = BarcodeFormat.QR_CODE;
                pictureBox2.Image = writer.Write(textBox12.Text);

            }
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView2.SelectedRows.Count > 0)
            {
                textBox10.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                textBox9.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                textBox7.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AddProductToCart();
        }

        private void AddProductToCart()
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                P_Product product = new P_Product
                {
                    p_id = dataGridView3.SelectedRows[0].Cells[0].Value.ToString(),
                    p_name = dataGridView3.SelectedRows[0].Cells[1].Value.ToString(),
                    p_detail = dataGridView3.SelectedRows[0].Cells[2].Value.ToString(),
                    p_price = int.Parse(dataGridView3.SelectedRows[0].Cells[3].Value.ToString()),
                    p_amount = int.Parse(dataGridView3.SelectedRows[0].Cells[4].Value.ToString()),
                    p_type = dataGridView3.SelectedRows[0].Cells[5].Value.ToString(),
                    p_image = dataGridView3.SelectedRows[0].Cells[6].Value.ToString()
                };

                string[] row = { product.p_id, product.p_name, product.p_detail, product.p_type, product.p_price.ToString(), "1", product.p_price.ToString(), product.p_image };

                if (listView1.Items.ContainsKey(product.p_id))
                {
                    int am = int.Parse(listView1.Items[product.p_id].SubItems[5].Text) + 1;
                    listView1.Items[product.p_id].SubItems[5].Text = am.ToString();
                    listView1.Items[product.p_id].SubItems[6].Text = (product.p_price * am).ToString();
                }
                else
                {
                    ListViewItem item = new ListViewItem(row);
                    item.Name = product.p_id;
                    listView1.Items.Add(item);
                }

                Sumcalculate();
                listView1.SelectedItems.Clear();
            }
        }


        private void Sumcalculate()
        {
            double sum = 0;
            int AllItem = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                sum += double.Parse(item.SubItems[6].Text);
                AllItem += int.Parse(item.SubItems[5].Text);
            }
            label23.Text = sum.ToString("#.00");
            label37.Text = sum.ToString("#.00");
            label21.Text = AllItem.ToString();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                textBox16.Text = listView1.SelectedItems[0].SubItems[0].Text;
                textBox17.Text = listView1.SelectedItems[0].SubItems[1].Text;
                textBox18.Text = listView1.SelectedItems[0].SubItems[2].Text;
                comboBox7.Text = listView1.SelectedItems[0].SubItems[3].Text;
                textBox27.Text = double.Parse(listView1.SelectedItems[0].SubItems[4].Text).ToString("#.00");
                pictureBox3.Image = ImageLoad.LoadImage(listView1.SelectedItems[0].SubItems[7].Text);

                numericUpDown1.Value = int.Parse(listView1.SelectedItems[0].SubItems[5].Text);
                label9.Text = (int.Parse(listView1.SelectedItems[0].SubItems[4].Text) * numericUpDown1.Value).ToString("#.00");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            textBox16.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
            comboBox7.Text = "";
            label9.Text = "0.00";
            label21.Text = "0";
            label23.Text = "0.00";
            label37.Text = "";
            pictureBox3.Image = null;
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            string name = textBox23.Text;
            string phone = textBox24.Text;
            string username = textBox20.Text;
            string password = textBox21.Text;
            int depart = int.Parse((comboBox4.SelectedItem as ComboBoxItem).GetValue());

            P_Employee employee = new P_Employee {
                emp_name = name,
                emp_phone = phone,
                emp_username = username,
                emp_password = password,
                depart_id = depart,
            };

            _context.P_Employee.Add(employee);
            int result = _context.SaveChanges();
            if (result > 0) MessageBox.Show("บันทึกข้อมูลพนักงานเรียบร้อย","บันทึกสำเร็จ");
            textBox23.Text = "";
            textBox24.Text = "";
            textBox20.Text = "";
            textBox21.Text = "";
            comboBox4.Text = "";

            pEmployeeBindingSource.DataSource = _context.P_Employee
                .Select(s => new { s.emp_id, s.emp_name, s.emp_phone, s.emp_username, s.emp_password, s.P_Department.department }).ToList();

        }

        private void button14_Click(object sender, EventArgs e)
        {
            int empID = int.Parse(textBox1.Text);
            if (MessageBox.Show("ต้องการลบพนักงานคนนี้หรือไม่ ?", "ลบพนักงาน", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var emp = _context.P_Employee.SingleOrDefault(em => em.emp_id == empID);
                _context.P_Employee.Remove(emp);
                _context.SaveChanges();

                pEmployeeBindingSource.DataSource = _context.P_Employee
                .Select(s => new { s.emp_id, s.emp_name, s.emp_phone, s.emp_username, s.emp_password, s.P_Department.department }).ToList();
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int selected = dataGridView1.CurrentRow.Index;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[selected].Selected = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selected = dataGridView2.CurrentRow.Index;
            dataGridView2.ClearSelection();
            dataGridView2.Rows[selected].Selected = true;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string name = textBox25.Text;
            string phone = textBox22.Text;
            P_Member member = new P_Member
            {
                mem_name = name,
                mem_phone = phone
            };
            _context.P_Member.Add(member);
            int result = _context.SaveChanges();
            if(result > 0)
            {
                MessageBox.Show("บันทึกข้อมูลเรียบร้อย", "บันทึกข้อมูล");
                pMemberBindingSource.DataSource = _context.P_Member.ToList();
            }
            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            int memberID = int.Parse(textBox10.Text);
            if (MessageBox.Show("ต้องการลบลูกค้าคนนี้หรือไม่ ?", "ลบลูกค้า", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var member = _context.P_Member.SingleOrDefault(m => m.mem_id == memberID);
                _context.P_Member.Remove(member);
                _context.SaveChanges();

                pMemberBindingSource.DataSource = _context.P_Member.ToList();

            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            int selected = dataGridView3.CurrentRow.Index;
            dataGridView3.ClearSelection();
            dataGridView3.Rows[selected].Selected = true;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string pID = textBox12.Text;
            if (MessageBox.Show("ต้องการสินค้านี้หรือไม่ ?", "ลบสินค้า", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var product = _context.P_Product.SingleOrDefault(p => p.p_id == pID);
                _context.P_Product.Remove(product);
                _context.SaveChanges();

                var Product_List = _context.P_Product.ToList();
                pProductBindingSource.DataSource = Product_List;

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string id = textBox12.Text;
            string name = textBox11.Text;
            string type = comboBox6.Text;
            int amount = int.Parse(textBox14.Text);
            double price = double.Parse(textBox26.Text);
            string detail = textBox8.Text;

            var product = _context.P_Product.SingleOrDefault(p => p.p_id == id);
            product.p_name = name;
            product.p_type = type;
            product.p_amount = amount;
            product.p_price = price;
            product.p_detail = detail;


            int result = _context.SaveChanges();
            if (result > 0) MessageBox.Show("แก้ไขข้อมูลเรียบร้อย");
            var Product_List = _context.P_Product.ToList();
            pProductBindingSource.DataSource = Product_List;

            button19_Click(sender,e);
        }

        private async void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0){
                await Task.Delay(100);
                if (numericUpDown1.Value == 0)
                {
                    MessageBox.Show("ต้องมากกว่า 0");
                    numericUpDown1.Value = 1;
                    listView1.SelectedItems[0].SubItems[5].Text = numericUpDown1.Text;
                }
                else
                {
                    listView1.SelectedItems[0].SubItems[5].Text = numericUpDown1.Text;
                }
                listView1.SelectedItems[0].SubItems[6].Text = (int.Parse(listView1.SelectedItems[0].SubItems[4].Text) * (int.Parse(listView1.SelectedItems[0].SubItems[5].Text))).ToString();
                Sumcalculate();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (videoIn != null && videoIn.IsRunning)
            {
                timer1.Enabled = false;
                videoSourcePlayer1.Stop();
            }
            else
            {
                videoIn = new VideoCaptureDevice(webcams[comboBox5.SelectedIndex].MonikerString);
                videoSourcePlayer1.VideoSource = videoIn;
                videoSourcePlayer1.Start();
                timer1.Enabled = true;
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (videoIn != null && videoIn.IsRunning)
            {
                timer1.Enabled = false;
                videoSourcePlayer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var capture = videoSourcePlayer1.GetCurrentVideoFrame();
            if (capture != null)
            {
                videoSourcePlayer1.Visible = true;
                BarcodeReader reader = new BarcodeReader();
                var result = reader.Decode(capture);
                if (result != null)
                {
                    bool found = false;
                    foreach(DataGridViewRow row in dataGridView3.Rows)
                    {
                        if (row.Cells[0].Value.ToString() == result.ToString())
                        {
                            dataGridView3.Rows[row.Index].Selected = true;
                            found = true;
                        }
                    }
                    if (found)
                    {
                        videoSourcePlayer1.Visible = false;
                        videoSourcePlayer1.Stop();
                        label44.Text = result.ToString();
                        AddProductToCart();
                        videoSourcePlayer1.Start();
                    }
                    else
                    {
                        MessageBox.Show("ไม่พบข้อมูลสินค้า");
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            string toID = date.ToString("ddHHmmss");

            P_Bill bill = new P_Bill
            {
                bill_id = toID,
                member_id = null,
                sum_price = float.Parse(label23.Text),
                date = date
            };
            _context.P_Bill.Add(bill);

            foreach (ListViewItem item in listView1.Items)
            {
                P_Bill_items bill_Items = new P_Bill_items();
                bill_Items.bill_id = bill.bill_id;
                bill_Items.product_id = item.SubItems[0].Text;
                bill_Items.amount = int.Parse(item.SubItems[5].Text);
                bill_Items.sum_price = int.Parse(item.SubItems[6].Text);
                _context.P_Bill_items.Add(bill_Items);

                var product = _context.P_Product.SingleOrDefault(p => p.p_id == item.Name);
                product.p_amount -= int.Parse(item.SubItems[5].Text);
            }

            int r = _context.SaveChanges();
            MessageBox.Show("ทำรายการเสร็จสิ้น");

            button13_Click(sender, e);
    
            pProductBindingSource.DataSource = _context.P_Product.ToList();
        }

        private void button25_Click_1(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
                Sumcalculate();
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            string id = textBox16.Text;
            string name = textBox17.Text;
            string type = comboBox7.Text;
            double price = double.Parse(textBox27.Text);
            string detail = textBox18.Text;

            listView1.SelectedItems[0].SubItems[0].Text = id;
            listView1.SelectedItems[0].SubItems[1].Text = name;
            listView1.SelectedItems[0].SubItems[2].Text = detail;
            listView1.SelectedItems[0].SubItems[3].Text = type;
            listView1.SelectedItems[0].SubItems[4].Text = price.ToString();

            var product = _context.P_Product.SingleOrDefault(p => p.p_id == id);
            product.p_name = name;
            product.p_type = type;
            product.p_price = price;
            product.p_detail = detail;


            int result = _context.SaveChanges();
            if (result > 0) MessageBox.Show("แก้ไขข้อมูลเรียบร้อย");
            listView1.SelectedItems[0].SubItems[6].Text = (int.Parse(listView1.SelectedItems[0].SubItems[4].Text) * (int.Parse(listView1.SelectedItems[0].SubItems[5].Text))).ToString();
            Sumcalculate();
            pPromotionBindingSource.DataSource = _context.P_Promotion.Select(s => new { s.pro_id, s.discount, s.product_id_1, s.product_id_2, p1 = s.P_Product.p_name, p2 = s.P_Product1.p_name }).ToList();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            bool found = false;
            foreach(DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells[0].Value.ToString().Equals(textBox29.Text))
                {
                    MessageBox.Show("พบข้อมูล : " + row.Cells[0].Value.ToString());
                    dataGridView3.Rows[row.Index].Selected = true;
                    found = true;
                    AddProductToCart();
                }
            }
            if(found == false)
            {
                MessageBox.Show("ไม่พบข้อมูล");
            }
            
            
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string id;
            string name;
            string detail;
            double price;
            string image;
            string type;

            string url = "https://www.jib.co.th/web/product/readProduct/" + textBox15.Text;
            id = textBox15.Text;

            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            HtmlNode titleNode = doc.DocumentNode.SelectSingleNode("//meta[@property=\"og:title\"]");
            name = titleNode.Attributes["content"].Value;
            //MessageBox.Show(name);
            HtmlNode descriptionNode = doc.DocumentNode.SelectSingleNode("//meta[@property=\"og:description\"]");
            detail = descriptionNode.Attributes["content"].Value;
            //MessageBox.Show(detail);
            HtmlNode imageNode = doc.DocumentNode.SelectSingleNode("//meta[@property=\"og:image\"]");
            image = imageNode.Attributes["content"].Value;
            //MessageBox.Show(image);
            HtmlNode typeNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"product_wrap\"]/div[1]/a[3]");
            type = typeNode.Attributes["title"].Value;
            //MessageBox.Show(type);
            HtmlNode priceNode = doc.DocumentNode.SelectSingleNode("//div[@class=\"col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center price_block\"]");
            string strprice = priceNode.InnerText;
            strprice = new string(strprice.Where(c => char.IsDigit(c)).ToArray());
            price = double.Parse(strprice);
            //MessageBox.Show(price.ToString());
            P_Product product = new P_Product { 
                p_id = id,
                p_detail = detail,
                p_amount = 999,
                p_image = image,
                p_name = name,
                p_price = price,
                p_type = type
            };

            _context.P_Product.Add(product);
            var r = _context.SaveChanges();
            MessageBox.Show(product.p_id+" -> "+product.p_name);
        }
    }
}
