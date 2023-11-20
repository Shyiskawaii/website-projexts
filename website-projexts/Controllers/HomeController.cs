using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using website_projexts.Context;
using website_projexts.Models;

namespace website_projexts.Controllers
{
    public class HomeController : Controller
    {
        private OurDBContext _db = new OurDBContext();
        
        public PartialViewResult UserStuff()
        {
            if (Session["UserID"] != null)
            {
                int id = Convert.ToInt32(Session["UserID"]);
                ViewBag.UserImage = _db.User.FirstOrDefault(u => u.UserID == id).UserImage;
                ViewBag.UserName = _db.User.FirstOrDefault(u => u.UserID == id).UserName;
            }
            return PartialView();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        public ActionResult ProjectControl(string search)
        {
            if (Convert.ToString(Session["UserRoles"]) == "admin")
            {
                if (search != null)
                {
                    var projectList = _db.Projects.OrderByDescending(x => x.ProjectName).Where(p => p.ProjectName.Contains(search));
                    return View(projectList.ToList());
                }
                return View(_db.Projects.ToList());
            }
            return RedirectToAction("Index");
        }
        public ActionResult CategoryControl()
        {
            if (Convert.ToString(Session["UserRoles"]) == "admin")
            {
                return View(_db.Category.ToList());
            }
            return RedirectToAction("Index");
        }
        public ActionResult UserControl()
        {
            if (Convert.ToString(Session["UserRoles"]) == "admin")
            {
                return View(_db.User.ToList());
            }
            return RedirectToAction("Index");
        }

    }
}