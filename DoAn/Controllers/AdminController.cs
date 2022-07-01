using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;
using PagedList;
using PagedList.Mvc;
namespace DoAn.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        DbWebGiayDataContext db = new DbWebGiayDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Product(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.Products.ToList().OrderBy(n => n.ID_Product).ToPagedList(pageNumber, pageSize));
            //return View(db.Products.ToList());
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên dăng nhập";
            }

            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else                
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                
            }
            return View();
        }
        [HttpGet]
        public ActionResult CreateNewProduct()
        {
            ViewBag.ID_Cat = new SelectList(db.Categories.ToList().OrderBy(n => n.Name), "ID_Cat", "Name");
            ViewBag.ID_NSX = new SelectList(db.NSXes.ToList().OrderBy(n => n.TenNsx), "ID_NSX", "TenNsx");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNewProduct(Product product, HttpPostedFileBase fileUpLoad)
        {
            
            ViewBag.ID_Cat = new SelectList(db.Categories.ToList().OrderBy(n => n.Name), "ID_Cat", "Name");
            ViewBag.ID_NSX = new SelectList(db.NSXes.ToList().OrderBy(n => n.TenNsx), "ID_NSX", "TenNsx");
            if(fileUpLoad == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    //luu ten file
                    var fileName = Path.GetFileName(fileUpLoad.FileName);
                    //duong dan
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    //kiem tra hih anh da ton tai   
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //luu hinh anh
                        fileUpLoad.SaveAs(path);
                    }
                    product.Img = fileName;
                    //luu vao csdl
                    db.Products.InsertOnSubmit(product);
                    db.SubmitChanges();
                }
                return RedirectToAction("Product");
            }
            
        }
        public ActionResult Detail(int id)
        {
            Product product = db.Products.SingleOrDefault(n => n.ID_Product == id);
            ViewBag.ID_Product = product.ID_Product;
            if(product== null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(product);
        }

        //xoa sp

        [HttpGet]
        public ActionResult Delete(int id)
        {
            // lay ra doi tuong sach can xoa
            Product product = db.Products.SingleOrDefault(n => n.ID_Product == id);
            ViewBag.ID_Product = product.ID_Product;
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult AccDelete(int id)
        {
            //lay ra doi tuong sach can xoa
           Product product = db.Products.SingleOrDefault(n => n.ID_Product == id);
            ViewBag.ID_Product = product.ID_Product;
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Products.DeleteOnSubmit(product);
            db.SubmitChanges();
            return RedirectToAction("product");
            //return View(product);
        }
        //chinhsua
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // lay ra doi tuong sach can eidt
            Product product = db.Products.SingleOrDefault(n => n.ID_Product == id);
           
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.ID_Cat = new SelectList(db.Categories.ToList().OrderBy(n => n.Name), "ID_Cat", "Name",product.ID_Cat);
            ViewBag.ID_NSX = new SelectList(db.NSXes.ToList().OrderBy(n => n.TenNsx), "ID_NSX", "TenNsx",product.ID_NSX);
            return View(product);
        }




        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product product, HttpPostedFileBase fileUpLoad)
        {

            ViewBag.ID_Cat = new SelectList(db.Categories.ToList().OrderBy(n => n.Name), "ID_Cat", "Name");
            ViewBag.ID_NSX = new SelectList(db.NSXes.ToList().OrderBy(n => n.TenNsx), "ID_NSX", "TenNsx");
            if (fileUpLoad == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file
                    var fileName = Path.GetFileName(fileUpLoad.FileName);
                    //duong dan
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    //kiem tra hih anh da ton tai   
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //luu hinh anh
                        fileUpLoad.SaveAs(path);
                    }
                    product.Img = fileName;
                    //luu vao csdl
                    UpdateModel(product);
                    db.SubmitChanges();
                }
                return RedirectToAction("Product");
            }

        }
    }

}