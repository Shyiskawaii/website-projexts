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
    }
}