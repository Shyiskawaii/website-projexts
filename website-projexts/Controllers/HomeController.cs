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
        
       
    }
}