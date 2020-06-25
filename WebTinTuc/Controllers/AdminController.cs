using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTinTuc.Models;

using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebTinTuc.Controllers
{
    public class AdminController : Controller
    { 
        // GET: Admin

        dbWebTinTucDataContext db = new dbWebTinTucDataContext();
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
            var username = collection["Username"];
            var password = collection["Password"];
            if (String.IsNullOrEmpty(username))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(password))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Account admin = db.Accounts.SingleOrDefault(n => n.Username == username && n.Password == password);

                if (admin != null && admin.Role == false)
                {
                    Session["Taikhoan"] = admin;
                    return RedirectToAction("Index", "Admin");
                }
                else if (admin != null && admin.Role == true)
                    return RedirectToAction("Index", "Home");
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult News(int ? page)
        {
            int pageNum = (page ?? 1);
            int pageSize = 5;
            return View(db.News.ToList().OrderByDescending(n=>n.DateSubmitted).ToPagedList(pageNum,pageSize));
        }

        [HttpGet]
        public ActionResult CreateNew()
        {
            ViewBag.ID_Category = new SelectList(db.categories.ToList().OrderBy(n => n.Name), "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNew(New @new,HttpPostedFileBase file)
        {
            ViewBag.ID_Category = new SelectList(db.categories.ToList().OrderBy(n => n.Name), "ID", "Name");

            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Img_News"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    file.SaveAs(path);
                }
                @new.image = fileName;
                @new.DateSubmitted = DateTime.Now;

                db.News.InsertOnSubmit(@new);
                db.SubmitChanges();
            }

            return RedirectToAction("News");
        }

        public ActionResult Details(int id)
        {
            New @new = db.News.SingleOrDefault(n => n.ID == id);
            if(@new==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(@new);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            New @new = db.News.SingleOrDefault(n => n.ID == id);
            if(@new==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(@new);
        }

        [HttpPost,ActionName("Delete")]
        public ActionResult Deletes(int id)
        {
            New @new = db.News.SingleOrDefault(n => n.ID == id);
            if (@new == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.News.DeleteOnSubmit(@new);
            db.SubmitChanges();
            return RedirectToAction("News");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            New @new = db.News.SingleOrDefault(n => n.ID == id);
            if(@new==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.ID_Category = new SelectList(db.categories.ToList().OrderBy(n => n.Name), "ID", "Name");
            return View(@new);
        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Edit(New @new,HttpPostedFileBase file)
        {
            ViewBag.ID_Category = new SelectList(db.categories.ToList().OrderBy(n => n.Name), "ID", "Name");
            if(ModelState.IsValid)
            {

                //var fileName = Path.GetFileName(file.FileName);
                //var path = Path.Combine(Server.MapPath("~/Img_News"), fileName);
                //if (System.IO.File.Exists(path))
                //{
                //    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                //}
                //else
                //{
                //    file.SaveAs(path);
                //}
                //@new.image = fileName;
                UpdateModel(@new);
                db.SubmitChanges();
            }
            return RedirectToAction("News");
        }
    }
}