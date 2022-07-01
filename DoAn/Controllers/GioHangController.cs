using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Linq;

namespace DoAn.Controllers
{
    public class GioHangController : Controller
    {
        DbWebGiayDataContext db = new DbWebGiayDataContext();
        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> ListGH = Session["GioHang"] as List<GioHang>;
            if (ListGH == null)
            {
                ListGH = new List<GioHang>();
                Session["GioHang"] = ListGH;
            }
            return ListGH;
        }
        //thêm vào giỏ
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            List<GioHang> ListGH = LayGioHang();
            GioHang sanpham = ListGH.Find(n=> n.MaSP == MaSP);
            if(sanpham == null)
            {
                sanpham = new GioHang(MaSP);
                ListGH.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.SoLuong++;
                return Redirect(strURL);
            }
        }
        //tổng số lượng
        private int TongSL()
        {
            int TSL = 0;
            List<GioHang> ListGH = Session["GioHang"] as List<GioHang>;
            if (ListGH != null)
            {
                TSL = ListGH.Sum(n => n.SoLuong);
            }
            return TSL;
        }

        //tính tổng tiền
        private double TongTien()
        {
            double TT = 0;
            List<GioHang> ListGH = Session["GioHang"] as List<GioHang>;
            if (ListGH != null)
            {
                TT = ListGH.Sum(n => n.ThanhTien);
            }
            return TT;
        }
        //tạo partial view để hiển thị thông tin giỏ hàng
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TongSL();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //xóa giỏ hàng
        public ActionResult XoaGH(int MaGiay)
        {
            //lấy hàng từ session
            List<GioHang> ListGH = LayGioHang();
            GioHang sanpham = ListGH.SingleOrDefault(n=>n.MaSP == MaGiay);
            if(sanpham != null)
            {
                ListGH.RemoveAll (n=>n.MaSP == MaGiay);
                return RedirectToAction("GioHang");
            }
            if(ListGH.Count == 0)
            {
                return RedirectToAction("Index","Shoes");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGH()
        {
            List<GioHang> ListGH= LayGioHang();
            ListGH.Clear();
            return RedirectToAction("Index","Shoes");
        }

        //cập nhật giỏ hàng
        public ActionResult updateGH(int MaGiay, FormCollection f)
        {
            List<GioHang> ListGH = LayGioHang();
            GioHang sanpham = ListGH.SingleOrDefault(n=>n.MaSP == MaGiay);
            if(sanpham != null)
            {
                sanpham.SoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        // Hiển thị giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> ListGH = LayGioHang();
            if (ListGH.Count == 0)
            {
                return RedirectToAction("Index", "Shoes");
            }
            ViewBag.TongSL = TongSL();
            ViewBag.TongTien = TongTien();
            return View(ListGH);
        }
    }
}