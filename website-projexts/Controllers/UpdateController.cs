using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website_projexts.Context;
using website_projexts.Models;

namespace website_projexts.Controllers
{
    public class UpdateController : Controller
    {
        private OurDBContext _db = new OurDBContext();
        // GET: Update
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult UpdatePartial(int? projectID)
        {
            if (projectID.HasValue)
            {
                var updates = _db.Update.Where(u => u.ProjectID == projectID).ToList();
                if (updates.Count > 0)
                {
                    return PartialView(updates);
                }
            }

            return PartialView(null);
        }
        public ActionResult UpdateCreate(int projectID)
        {
            Update model = new Update
            {
                ProjectID = projectID
            };
            return View();
        }
        [HttpPost]
        public ActionResult UpdateCreate(Update update, int projectID) 
        {
            if (ModelState.IsValid)
            {
                update.UpdateTime = DateTime.Now;
                update.ProjectID = projectID;

                _db.Update.Add(update);
                _db.SaveChanges();
                return RedirectToAction("ProjectList");
            }
            return View();
        }
    }
}