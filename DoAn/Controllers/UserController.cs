using DoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn.Controllers
{
    public class UserController : Controller
    {
        DbWebGiayDataContext db = new DbWebGiayDataContext();
        // GET: User

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //gán giá trị cho biến người dùng nhập
            var Username = collection["Username"];
            var Password = collection["Password"];
            if (String.IsNullOrEmpty(Username))
            {
                ViewData["Loi1"] = "Nhập tên";
            }
            else if (String.IsNullOrEmpty(Password))
            {
                ViewData["Loi2"] = "Nhập mật khẩu";
            }
            else
            {
                //gán đối tượng cho đối tượng tạo mới
                User user = db.Users.SingleOrDefault(n => n.UserName == Username && n.Password == Password);
                if (user != null)
                {
                    ViewBag.ThongBao = " Đăng nhập thành công";
                    return RedirectToAction("Index","Shoes");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult DangKi()
        {
            return View();
        }

        //POST: hàm đăng ký
        [HttpPost]
        public ActionResult DangKi(FormCollection collection, User user)
        {
            var Username = collection["HoTenKh"];
            var Email = collection["Email"];
            var PhoneNumber = collection["PhoneNumber"];
            var Address = collection["Address"];
            var Password = collection["Password"];

            if (String.IsNullOrEmpty(Username))
            {
                ViewData["Loi1"] = "Họ tên không được để trống";
            }
            else if (String.IsNullOrEmpty(Email))
            {
                ViewData["Loi2"] = "Email không được để trống";
            }
            else if (String.IsNullOrEmpty(PhoneNumber))
            {
                ViewData["Loi3"] = "số điện thoại không được để trống";
            }
            else if (String.IsNullOrEmpty(Address))
            {
                ViewData["Loi4"] = "Địa chỉ không được để trống";
            }
            if (String.IsNullOrEmpty(Password))
            {
                ViewData["Loi5"] = "Mật khẩu không được để trống";
            }
            else
            {
                //gán giá trị mới cho đối tượng được tạo mới

                user.UserName = Username;
                user.Email = Email;
                user.Phone = PhoneNumber;
                user.Address = Address;
                user.Password = Password;
                db.Users.InsertOnSubmit(user);
                db.SubmitChanges();
                return RedirectToAction("Login");
            }
            return this.DangKi();
        }
    }
}