using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using website_projexts.Context;
using website_projexts.Models;
using website_projexts.ViewModels;

namespace website_projexts.Controllers
{
    public class UserController : Controller
    {
        private OurDBContext _db = new OurDBContext();
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = _db.User.FirstOrDefault(s => s.Email == user.Email);

                if (check == null)
                {
                    user.Password = GetMD5(user.Password);
                    _db.Configuration.ValidateOnSaveEnabled = false;

                    user.UserImage = "~/Content/img/userdefault.jpeg";
                    _db.User.Add(user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }

            }
         ;

            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = _db.User.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().UserID;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult UserPage(int id)
        {
            var user = _db.User.SingleOrDefault(p => p.UserID == id);

            // Calculate the additional information
            int createdProjectsCount = _db.Projects.Count(p => p.UserID == id);
             decimal totalFundsRaised = _db.Projects.Where(p => p.UserID == id).Sum(p => (decimal?)p.Raised) ?? 0;
            int donationsCount = _db.Donation.Count(d => d.UserID == id);
            decimal totalDonatedAmount = _db.Donation.Where(d => d.UserID == id).Sum(d => (decimal?)d.Donated) ?? 0;

            var viewModel = new UserEdit
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                UserImage = user.UserImage,
                CreatedProjectsCount = createdProjectsCount,
                TotalFundsRaised = totalFundsRaised,
                DonationsCount = donationsCount,
                TotalDonatedAmount = totalDonatedAmount
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult UserPage(UserEdit useredit)
        {
            var user = _db.User.SingleOrDefault(p => p.UserID == useredit.UserID);
            user.UserID = useredit.UserID;
            user.FirstName = useredit.FirstName;
            user.LastName = useredit.LastName;
            user.UserName = useredit.UserName;
            user.Email = useredit.Email;
            
            if (ModelState.IsValid)
            {
                if (useredit.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(useredit.UploadImage.FileName);
                    string extent = Path.GetExtension(useredit.UploadImage.FileName);
                    filename = filename + extent;
                    user.UserImage = "~/Content/img/" + filename;
                    useredit.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                }

                _db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();

                return RedirectToAction("UserPage", new { id = user.UserID });
            }

            return View(user);
        }
        public PartialViewResult UserProject(int id)
        {
            if (_db.Projects.Any(p => p.UserID == id))
            {
                var userProject = _db.Projects.Where(p => p.UserID == id);
            }
            return PartialView(null);
        }

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }


        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}