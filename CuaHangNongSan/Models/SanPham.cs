using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangNongSan.Models
{
    public class SanPham
    {
        public int Id { get; set; }
        public string Loaisp { get; set; }
        public string Tensp { get; set; }
        public string Mota { get; set; }
        public double Gia { get; set; }
        public string Hinh { get; set; }
    }
}