using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using website_projexts.Context;
using website_projexts.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using Microsoft.Ajax.Utilities;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace website_projexts.Controllers
{
    public class ProjectController : Controller
    {

        private OurDBContext _db = new OurDBContext();

        //public ActionResult ProjectList()
        //{
        //    return View(_db.Projects.ToList());
        //}

        public PartialViewResult ProjectPartial()
        {
            var topProjects = _db.Projects.OrderByDescending(p => p.Raised).Take(3).ToList();
            return PartialView(topProjects);
        }

        public ActionResult ProjectList(int? selectedCategoryId, int? page, string search)
        {
            int pageSize = 9;
            int pageNum = (page ?? 1);
            if (selectedCategoryId != null)
            {
                var projectList = _db.Projects.OrderByDescending(x => x.ProjectName).Where(p => p.Category.CategoryID == selectedCategoryId);
                return View(projectList.ToPagedList(pageNum, pageSize));
            }
            else if (search != null)
            {
                var projectList = _db.Projects.OrderByDescending(x => x.ProjectName).Where(p => p.ProjectName.Contains(search));
                return View(projectList.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var projectList = _db.Projects.OrderByDescending(x => x.ProjectName);
                return View(projectList.ToPagedList(pageNum, pageSize));
            }
        }
            //var errors =ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
        public ActionResult ProjectCreate()
        {
            if(Session["UserID"] != null)
            {
                var categories = _db.Category.ToList();
                ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");
                var model = new Projects();
                model.ProjectImage = "~/Content/img/default.jpeg";
                return View(model);
            }
            else
            {
                return RedirectToAction("Login","User");
            }
        }
        [HttpPost]
        public ActionResult ProjectCreate(Projects project)
        {

            project.ProjectImage = "~/Content/img/default.jpeg";

            if (ModelState.IsValid)
            {
                try
                {
                    if (project.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(project.UploadImage.FileName);
                        string extent = Path.GetExtension(project.UploadImage.FileName);
                        filename = filename + extent;
                        project.ProjectImage = "~/Content/img/" + filename;
                        project.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                    }
                    project.UserID = Convert.ToInt32(Session["UserID"]);
                    project.Raised = 0;
                    project.PostedTime = DateTime.Now;

                    _db.Projects.Add(project);
                    _db.SaveChanges();
                    return RedirectToAction("ProjectList");
                }
                catch
                {
                }
            }

            return RedirectToAction("ProjectCreate");
        }

        public ActionResult ProjectEdit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Login", "User");

            }
            else if (Session["UserID"] != null || Convert.ToString(Session["UserRoles"]) == "admin")
            {
                if (_db.Projects.Any(p => p.ProjectID == id))
                {
                    var project = _db.Projects.SingleOrDefault(p => p.ProjectID == id);
                    if (project.UserID != Convert.ToInt32(Session["UserID"]))
                        return Content("");
                    var categories = _db.Category.ToList();
                    ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");
                    ViewBag.ProjectID = id;
                    return View(project);
                }
                return Content("");
            }
            return RedirectToAction("Login", "User");
        }
        [HttpPost]
        public ActionResult ProjectEdit(int id, Projects project)
        {
            if(ModelState.IsValid)
            {
                if (project.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(project.UploadImage.FileName);
                    string extent = Path.GetExtension(project.UploadImage.FileName);
                    filename = filename + extent;
                    project.ProjectImage = "~/Content/img/" + filename;
                    project.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                }
                else
                {
                    var image = _db.Projects.AsNoTracking().SingleOrDefault(p => p.ProjectID == id);
                    project.ProjectImage = image.ProjectImage;                    
                }
                _db.Entry(project).State = EntityState.Modified;
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
            }
            return RedirectToAction("ProjectEdit");
        }
        public ActionResult ProjectDelete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Login", "User");

            }
            else if (_db.Projects.Any(p => p.ProjectID == id))
            {
                var project = _db.Projects.SingleOrDefault(p => p.ProjectID == id);
                if (project.UserID == Convert.ToInt32(Session["UserID"]) || Convert.ToString(Session["UserRoles"]) == "admin")
                {
                    if (Request.UrlReferrer != null)
                    {
                        TempData["ConfirmationMessage"] = "Bạn Đã Xóa Thành Công Dự Án!";
                    }
                    _db.Projects.Remove(project);
                    _db.SaveChanges();
                    return RedirectToAction("UserPage","User",new {id = Session["UserID"]});
                }
            }
            return RedirectToAction("Index", "Home");
        }
       
        //var errors = ModelState
        //     .Where(x => x.Value.Errors.Count > 0)
        //     .Select(x => new { x.Key, x.Value.Errors })
        //     .ToArray();


        public ActionResult ProjectDetail(int id)
        {
            if (id == null)
            {
                return RedirectToAction("ProjectList", "Project");
            }
            
            if (_db.Projects.Any(p => p.ProjectID == id))
            {
                var project = _db.Projects.Include(p => p.Donations).SingleOrDefault(p => p.ProjectID == id);
                return View(project);
            }
            
            return RedirectToAction("Login", "User");
        }

      
    }
}