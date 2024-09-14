using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CuaHangNongSan.Models
{
    public class LatestProduct
    {
        public int Id { get; set; }
        public string Loai { get; set; }
        public int CategoryId { get; set; }
        public string Hinh { get; set; }
        public string Tensp { get; set; }
        public int Gia { get; set; }
        public string Mota { get; set; }
    }
}