using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models
{
    public class GioHang
    {
        DbWebGiayDataContext db = new DbWebGiayDataContext();
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string Hinh { get; set; }
        public double GiaSP { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien
        {
            get { return SoLuong * GiaSP; }
        }
        public GioHang(int id)
        {
            MaSP = id;
            Product sanpham = db.Products.Single(n => n.ID_Product == MaSP);
            TenSP = sanpham.NameP;
            Hinh = sanpham.Img;
            GiaSP = double.Parse(sanpham.Price.ToString());
            SoLuong = 1;
        }
    }
}