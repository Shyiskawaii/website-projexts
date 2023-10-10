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

namespace website_projexts.Controllers
{
    public class ProjectController : Controller
    {

        private OurDBContext _db = new OurDBContext();

        //public ActionResult ProjectList()
        //{
        //    return View(_db.Projects.ToList());
        //}
        public ActionResult ProjectList(Category category, int? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);
            if (category == null)
            {
                var projectList = _db.Projects.OrderByDescending(x => x.ProjectName);
                return View(projectList.ToPagedList(pageNum, pageSize));
            }
            else
            {
                var projectList = _db.Projects.OrderByDescending(x => x.ProjectName).Where(p => p.Category == category);
                return View(projectList);
            }
        }
        public ActionResult ProjectCreate()
        {
            var categories = _db.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryID", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult ProjectCreate(Projects project)
        {
            if (ModelState.IsValid)
            {
                project.CategoryID = 1;
                project.UserID = Convert.ToInt32(Session["idUser"]);
                project.Raised = 0;
                project.PostedTime = DateTime.Now;

                _db.Projects.Add(project);
                _db.SaveChanges();
                return RedirectToAction("ProjectList");
            }

            return View();
        }

        //var errors = ModelState
        //     .Where(x => x.Value.Errors.Count > 0)
        //     .Select(x => new { x.Key, x.Value.Errors })
        //     .ToArray();
        public ActionResult ProjectDetail(int id)
        {
            return View(_db.Projects.Where(s => s.ProjectID == id).FirstOrDefault());
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
    }
}