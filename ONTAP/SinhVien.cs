using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONTAP
{
    internal class SinhVien
    {
        public string MSSV { get; set; }
        public string HoTen  { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public string Hinh {  get; set; }
        public DateTime NgaySinh { get; set; }
        public string Lop {  get; set; }    
        public string Sdt { get; set; }
        public bool GioiTinh { get; set; }
        public SinhVien() { }   

        public SinhVien(string ms, string ten, string email , string diachi , string hinh , DateTime ngaysinh,string lop, string sdt, bool gt)
        {
            this.MSSV = ms;
            this.HoTen = ten;
            this.Email = email;
            this.DiaChi = diachi;
            this.Hinh = hinh;
            this.NgaySinh = ngaysinh;
            this.Lop = lop;
            this.Sdt = sdt;
            this.GioiTinh = gt;

        }
    }
}
