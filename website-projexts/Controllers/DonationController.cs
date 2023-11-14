using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website_projexts.Context;
using website_projexts.Models;
using System.Data.Entity;

namespace website_projexts.Controllers
{
    public class DonationController : Controller
    {
        private OurDBContext _db = new OurDBContext();
        // GET: Donation

        public ActionResult DonationCreate(int? projectID)
        {
            if (projectID == null)
            {
                return RedirectToAction("Login", "User");

            }
            else if (Session["UserID"] != null || Session["UserRoles"] == "admin")
            {
                if (_db.Projects.Any(p => p.ProjectID == projectID))
                {
                    ViewBag.ProjectName = _db.Projects.FirstOrDefault(p => p.ProjectID == projectID).ProjectName;
                    Donation model = new Donation
                    {
                        ProjectID = Convert.ToInt32(projectID)
                    };
                    return View(model);
                }
            }
            return RedirectToAction("Login", "User");
        }
        [HttpPost]
        public ActionResult DonationCreate(int projectID, Donation donation)
        {
            if (ModelState.IsValid)
            {
                donation.UserID = Convert.ToInt32(Session["UserID"]);
                donation.DonationTime = DateTime.Now;
                donation.ProjectID = projectID;

                var project = _db.Projects.FirstOrDefault(p => p.ProjectID == projectID);
                project.Raised += donation.Donated;


                _db.Donation.Add(donation);
                _db.Entry(project).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("ProjectList","Project");
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