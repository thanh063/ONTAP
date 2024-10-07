using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONTAP
{
    public delegate int sosanh(object sv1,object sv2);
    internal class QuanLySinhVien
    {
      public List<SinhVien> DanhSach = new List<SinhVien>();
        public QuanLySinhVien ()
        {
            DanhSach = new List<SinhVien> ();
        }
        public void Them(SinhVien sv)
        {
            DanhSach.Add (sv);
        }
        public SinhVien this[int index]
        {
            get { return DanhSach[index]; }
            set { DanhSach[index] = value; }
        }
        public void Xoa(object obj, sosanh ss)
        {
            int i = DanhSach.Count - 1;
            for (; i >= 0; i--)
                if (ss(obj, this[i]) == 0)
                    this.DanhSach.RemoveAt(i);
        }
        public SinhVien Tim(object obj, sosanh ss)
        {
            SinhVien result = null;
            foreach (SinhVien sv in DanhSach)
            {
                if (ss(obj, sv) == 0)
                {
                    result = sv;
                    break;
                }
            }
            return result;
        }
        public bool Sua(SinhVien svsua, object obj, sosanh ss)
        {
            int i, count;
            bool kq = false;
            count = this.DanhSach.Count - 1;
            for( i = 0; i<= count; i++)
            {
                if (ss(obj, this[i] ) == 0)
                {
                    this[i] = svsua;
                    kq = true;
                    break;
                } 
            }    

            return kq;
        }
        public List<SinhVien> DocFile()
        {
            List<SinhVien> danhSach = new List<SinhVien>();
            try
            {
                using (StreamReader sr = new StreamReader("sinhvien.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var data = line.Split('|');
                        if (data.Length < 9) continue; // Đảm bảo có đủ thông tin
                        SinhVien sv = new SinhVien
                        {
                            MSSV = data[0],
                            HoTen = data[1],
                            GioiTinh = bool.Parse(data[2]),
                            NgaySinh = DateTime.Parse(data[3]),
                            Lop = data[4],
                            Sdt = data[5],
                            Email = data[6],
                            DiaChi = data[7],
                            Hinh = data[8]
                        };
                        danhSach.Add(sv);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Tệp không tồn tại: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc tệp: " + ex.Message);
            }
            return danhSach;
        }
        public void GhiFile()
        {
            using (StreamWriter sw = new StreamWriter("sinhvien.txt"))
            {
                foreach (var sv in DanhSach)
                {
                    sw.WriteLine($"{sv.MSSV}|{sv.HoTen}|{sv.GioiTinh}|{sv.NgaySinh}|{sv.Lop}|{sv.Sdt}|{sv.Email}|{sv.DiaChi}|{sv.Hinh}");
                }
            }
        }
    }
}
