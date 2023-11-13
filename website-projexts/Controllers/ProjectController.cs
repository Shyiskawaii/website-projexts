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

namespace website_projexts.Controllers
{
    public class ProjectController : Controller
    {

        private OurDBContext _db = new OurDBContext();

        //public ActionResult ProjectList()
        //{
        //    return View(_db.Projects.ToList());
        //}

        public ActionResult AdminControler(string search)
        {
            if (search != null)
            {
                var projectList = _db.Projects.OrderByDescending(x => x.ProjectName).Where(p => p.ProjectName.Contains(search));
                return View(projectList.ToList());
            }
            return View(_db.Projects.ToList());
        }

        public ActionResult ProjectList(int? selectedCategoryId, int? page, string search)
        {
            int pageSize = 12;
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
            //var errors = ModelState
            //     .Where(x => x.Value.Errors.Count > 0)
            //     .Select(x => new { x.Key, x.Value.Errors })
            //     .ToArray();
        public ActionResult ProjectCreate()
        {
            var categories = _db.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryID", "Name");
            var model = new Projects();
            model.ProjectImage = "~/Content/img/default.jpeg";
            return View(model);
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
                    //project.UserID = Convert.ToInt32(Session["idUser"]);
                    project.UserID = 1;
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

        public ActionResult ProjectEdit(int id)
        {
            var categories = _db.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryID", "Name");
            var project = _db.Projects.SingleOrDefault(p => p.ProjectID == id);

            return View(project);
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
        public ActionResult ProjectDelete(int id)
        {
            var project = _db.Projects.SingleOrDefault(p => p.ProjectID == id);
            return View(project);
        }
        public ActionResult ProjectConfirmDelete(int projectID)
        {
            Projects product = _db.Projects.Where(s => s.ProjectID == projectID).FirstOrDefault();
            _db.Projects.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //var errors = ModelState
        //     .Where(x => x.Value.Errors.Count > 0)
        //     .Select(x => new { x.Key, x.Value.Errors })
        //     .ToArray();


        public ActionResult ProjectDetail(int id)
        {
            var project = _db.Projects.Include(p => p.Donations).SingleOrDefault(p => p.ProjectID == id);

            return View(project);
        }

 
        public ActionResult DonationCreate(int projectID)
        {
            Donation model = new Donation
            {
                ProjectID = projectID
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult DonationCreate(int projectID,Donation donation)
        {
            if (ModelState.IsValid)
            {
                donation.UserID = Convert.ToInt32(Session["idUser"]);
                donation.DonationTime = DateTime.Now;
                donation.ProjectID = projectID;

                _db.Donation.Add(donation);
                _db.SaveChanges();
                return RedirectToAction("ProjectList");
            }
            return View();
        }

        public PartialViewResult DonationPartial(int? projectID)
        {
            if (projectID.HasValue)
            {
                var donation = _db.Donation.Include(d => d.User).Where(u => u.ProjectID == projectID).ToList();
                if (donation.Count > 0)
                {
                    return PartialView(donation);
                }
            }
            return PartialView(null);
        }
    }
}