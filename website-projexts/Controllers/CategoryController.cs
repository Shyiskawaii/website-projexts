
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website_projexts.Context;
using website_projexts.Models;

namespace website_projexts.Controllers
{
    public class CategoryController : Controller
    {
        private OurDBContext _db = new OurDBContext();
        // GET: Category
        public PartialViewResult CategoryPartial()
        {
            var cateList = _db.Category.ToList();
            return PartialView(cateList);
        }
        
        public ActionResult CategoryList()
        {
            var cateList = _db.Category.ToList();
            return View(cateList);
        }
        public ActionResult CategoryCreate()
        {
            if (Convert.ToString(Session["UserRoles"]) == "admin")
            {
                var model = new Category();
                model.CategoryImage = "~/Content/img/default.jpeg";
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        [HttpPost]
        public ActionResult CategoryCreate(Category category) 
        {
            category.CategoryImage = "~/Content/img/default.jpeg";
            if (ModelState.IsValid)
            {
                try
                {
                    if (category.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(category.UploadImage.FileName);
                        string extent = Path.GetExtension(category.UploadImage.FileName);
                        filename = filename + extent;
                        category.CategoryImage = "~/Content/img/" + filename;
                        category.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                    }
                    _db.Category.Add(category);
                    _db.SaveChanges();
                    return RedirectToAction("CategoryList");
                }
                catch
                {
                }
            }
            return RedirectToAction("CategoryCreate");
        }
        public ActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("CategoryControl", "Home");
            }
            else if (Convert.ToString(Session["UserRoles"]) == "admin")
            {
                if (_db.Category.Any(p => p.CategoryID == id))
                {
                    var cate = _db.Category.SingleOrDefault(p => p.CategoryID == id);
                    return View(cate);
                }
                return RedirectToAction("CategoryControl", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult CategoryEdit(int? id, Category cate)
        {
            if (ModelState.IsValid)
            {
                if (cate.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(cate.UploadImage.FileName);
                    string extent = Path.GetExtension(cate.UploadImage.FileName);
                    filename = filename + extent;
                    cate.CategoryImage = "~/Content/img/" + filename;
                    cate.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                }
                else
                {
                    var image = _db.Projects.AsNoTracking().SingleOrDefault(p => p.ProjectID == id);
                    cate.CategoryImage = image.ProjectImage;
                }
                _db.Entry(cate).State = EntityState.Modified;
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
            }
            return RedirectToAction("CategoryControl", "Home");
        }

        public ActionResult CategoryDelete(int id)
        {
            if (_db.Category.Any(p => p.CategoryID == id))
            {
                var cate = _db.Category.SingleOrDefault(p => p.CategoryID == id);
                if (Convert.ToString(Session["UserRoles"]) == "admin")
                {
                    if (Request.UrlReferrer != null)
                    {
                        TempData["ConfirmationMessage"] = "Bạn Đã Xóa Thành Công Danh Mục!";
                    }
                    _db.Category.Remove(cate);
                    _db.SaveChanges();
                    return RedirectToAction("CategoryControl", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}