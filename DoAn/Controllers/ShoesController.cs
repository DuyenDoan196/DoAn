using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class ShoesController : Controller
    {
        // tạo 1 đối tượng chứa toàn bộ csdl
        DbWebGiayDataContext db = new DbWebGiayDataContext();

        private List<Product> LayGiayMoi(int count)
        {
            //sx giảm dần theo ngày update_at
            return db.Products.OrderByDescending(a => a.Update_at).Take(count).ToList();
        }
        public ActionResult Index()
        {
            var giaymoi = db.Products.OrderBy(a => a.Update_at).ToList();
            //lấy giày mới nhất
            //var giayMoi = LayGiayMoi();
            return View(giaymoi);
        }

        public List<Product> ListFusal (int id = 1)
        {
            return db.Products.OrderByDescending(a => a.ID_Cat == id).Take(id).ToList();
        }
        //[ValidateInput(false)]
        public ActionResult listF()
        {
            var list = ListFusal(100);
            return View(list);
        }


        // giày theo loại
        public ActionResult LoaiGiay()
        {
            var LoaiGiay = from lg in db.Categories select lg;
            return PartialView(LoaiGiay);
        }
        public ActionResult giayTheoLoai(int id)
        {
            var giay = from s in db.Products where s.ID_Cat == id select s;
            return View(giay);
        }


        // giày theo NSX
        public ActionResult NhaSX()
        {
            var Nsx = from nsx in db.NSXes select nsx;
            return PartialView(Nsx);
        }
        public ActionResult giayTheoNSX(int id)
        {
            var giay = from s in db.Products where s.ID_NSX == id select s;
            return View(giay);
        }

        public ActionResult Detail(int id)
        {
            //var giay = from s in db.Products
            //           where s.ID_Product == id
            //           select s;

            var giay = db.Products.FirstOrDefault(m => m.ID_Product == id);
            return View(giay);
        }
    }
}