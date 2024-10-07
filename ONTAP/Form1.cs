using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONTAP
{
    public partial class Form1 : Form
    {
        QuanLySinhVien qlsv;
        public Form1()
        {
            
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            qlsv = new QuanLySinhVien();
            qlsv.DanhSach = qlsv.DocFile(); // Tải danh sách sinh viên từ file
            LoadListView(); // Cập nhật ListView để hiển thị thông tin


        }
        private bool IsValidMSSV(string mssv)
        {
            // Kiểm tra xem MSSV có phải là 7 ký tự số hay không
            return mssv.Length == 7 && mssv.All(char.IsDigit);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email; // Kiểm tra xem địa chỉ email có hợp lệ không
            }
            catch
            {
                return false;
            }
        }
        private bool IsValidPhoneNumber(string phone)
        {
            // Kiểm tra xem số điện thoại có phải là 10 ký tự số hay không
            return phone.Length == 10 && phone.All(char.IsDigit);
        }
        private void btnMacDinh_Click(object sender, EventArgs e)
        {
            this.mtbMSSV.Text = null;
            this.txtHoTen.Text = null;
            this.txtEmail.Text = null;
            this.txtDiaChi.Text = null;
            this.txtHinh.Text = null;
            this.dtpNgaySinh.Value = DateTime.Now;
            this.cbbLop.Text = null;    
            this.mtbSdt.Text = null;
            this.pbHinh = new PictureBox();
           
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private SinhVien GetSinhVien ( )
        {
            SinhVien sv = new SinhVien();
            sv.MSSV = mtbMSSV.Text;
            sv.HoTen= txtHoTen.Text;
            sv.Email= txtEmail.Text;
            sv.DiaChi= txtDiaChi.Text;
            sv.Sdt = mtbSdt.Text;
            sv.Hinh= txtHinh.Text;
            sv.NgaySinh = dtpNgaySinh.Value;
            sv.GioiTinh = false;
            if(rdNam.Checked) sv.GioiTinh = true;
            sv.Lop = cbbLop.Text;
            return sv;
        }
        private SinhVien GetSinhVienLV(ListViewItem lv)
        {
            SinhVien sv = new SinhVien();
            sv.MSSV = lv.SubItems[0].Text;
            sv.HoTen = lv.SubItems[1].Text;
            sv.GioiTinh = false;
            if (lv.SubItems[2].Text=="Nam") sv.GioiTinh = true;
            sv.NgaySinh = DateTime.Parse(lv.SubItems[3].Text);
            sv.Lop= lv.SubItems[4].Text;
            sv.Sdt = lv.SubItems[5].Text; 
            sv.Email = lv.SubItems[6].Text;
            sv.DiaChi = lv.SubItems[7].Text;
            sv.Hinh = lv.SubItems[8].Text;

            return sv;
        }
        private void ThemSV (SinhVien sv)
        {
            ListViewItem lv = new ListViewItem(sv.MSSV);
            lv.SubItems.Add(sv.HoTen);
            string gt = "";
            if (sv.GioiTinh) gt = "Nam";else gt = "Nữ";
            lv.SubItems.Add(gt);
            lv.SubItems.Add(sv.NgaySinh.ToShortTimeString());
            lv.SubItems.Add(sv.Lop);
            lv.SubItems.Add(sv.Sdt);
            lv.SubItems.Add(sv.Email);
            lv.SubItems.Add(sv.DiaChi);
            lv.SubItems.Add(sv.Hinh);
            this.lvSinhVien.Items.Add(lv);
        }
        public void LoadListView()
        {
            this.lvSinhVien.Items.Clear();
            foreach (SinhVien sv in qlsv.DanhSach)
            {
                ThemSV(sv);
            }    
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return; // Kiểm tra hợp lệ

            SinhVien sv = GetSinhVien();
            SinhVien kq = qlsv.Tim(sv.MSSV, SoSanhTheoMa);
            if (kq != null)
            {
                qlsv.Sua(sv, sv.MSSV, SoSanhTheoMa);
                MessageBox.Show("Cập nhật sinh viên thành công.");
            }
            else
            {
                qlsv.Them(sv);
                MessageBox.Show("Thêm sinh viên thành công.");
            }
            qlsv.GhiFile();
            LoadListView();
        }
        private bool ValidateInputs()
        {
            // Xác thực MSSV
            if (string.IsNullOrWhiteSpace(mtbMSSV.Text) || !IsValidMSSV(mtbMSSV.Text))
            {
                MessageBox.Show("MSSV không hợp lệ. Nó phải là 6 ký tự số.");
                return false;
            }

            // Xác thực Email
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ.");
                return false;
            }

            // Xác thực Số điện thoại
            if (!IsValidPhoneNumber(mtbSdt.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Nó phải là 10 ký tự số.");
                return false;
            }

            return true; // Tất cả các trường hợp đều hợp lệ
        }
        private void btnChonHinh_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                txtHinh.Text = opf.FileName;
                pbHinh.Image = Image.FromFile(opf.FileName);
            }
        }
        private void ThietLapThongSinhVien(SinhVien sv)
        {
            this.mtbMSSV.Text = sv.MSSV;
            this.txtHoTen.Text = sv.HoTen;
            this.txtEmail.Text = sv.Email;
            this.txtDiaChi.Text = sv.DiaChi;
            this.mtbSdt.Text = sv.Sdt;
            this.txtHinh.Text = sv.Hinh;
            this.dtpNgaySinh.Value = sv.NgaySinh;
            this.cbbLop.Text = sv.Lop;

            // Xử lý radio button cho giới tính
            if (sv.GioiTinh == true)
            {
                this.rdNam.Checked = true;
                this.rdNu.Checked = false;
            }
            else
            {
                this.rdNam.Checked = false;
                this.rdNu.Checked = true;
            }
        }
        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = this.lvSinhVien.SelectedItems.Count;
            if ( count > 0 )
            {
                ListViewItem lv = this.lvSinhVien.SelectedItems[0];
                SinhVien sv = GetSinhVienLV(lv);
                ThietLapThongSinhVien(sv);
            }    
        }
        private int SoSanhTheoMa(object obj1, object obj2)
        {
            SinhVien sv = obj2 as SinhVien;
            return sv.MSSV.CompareTo(obj1);
        }

        private void xóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int count = lvSinhVien.Items.Count - 1;
                for (int i = count; i >= 0; i--)
                {
                    ListViewItem lvitem = lvSinhVien.Items[i];
                    if (lvitem.Checked)
                        qlsv.Xoa(lvitem.SubItems[0].Text, SoSanhTheoMa);
                }
                LoadListView();
                btnMacDinh.PerformClick();
            }
        }
        private void LoadSinhVienToListView(SinhVien sv)
        {
            ListViewItem lv = new ListViewItem(sv.MSSV)
            {
                SubItems = {
            sv.HoTen,
            sv.GioiTinh ? "Nam" : "Nữ",
            sv.NgaySinh.ToShortDateString(),
            sv.Lop,
            sv.Sdt,
            sv.Email,
            sv.DiaChi,
            sv.Hinh
        }
            };
            lvSinhVien.Items.Add(lv);
        }
    }
}
