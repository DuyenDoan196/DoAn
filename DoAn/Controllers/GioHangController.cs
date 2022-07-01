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

        public ActionResult ThemGioHang (int id)
        {
            List<GioHang> ListGH = LayGioHang();
            GioHang sanpham = ListGH.Find(n=> n.MaSP == id);
            if(sanpham == null)
            {
                sanpham = new GioHang(id);
                ListGH.Add(sanpham);

            }
            else
            {
                sanpham.SoLuong++;
            }
            return Content("");
        }
        
        public ActionResult GioHang()
        {
            //List<GioHang> ListGH = LayGioHang();
            return View();
        }
    }
}