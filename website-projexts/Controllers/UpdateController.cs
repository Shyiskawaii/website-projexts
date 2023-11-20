using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website_projexts.Context;
using website_projexts.Models;
using website_projexts.ViewModels;

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
            Update model = new Update();
            model.ProjectID = projectID;
            model.UpdateImage = "~/Content/img/default.jpeg";
            ViewBag.ProjectName = _db.Projects.FirstOrDefault(p => p.ProjectID == projectID).ProjectName;
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateCreate(Update update, int projectID) 
        {
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (ModelState.IsValid)
            {
                if (update.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(update.UploadImage.FileName);
                    string extent = Path.GetExtension(update.UploadImage.FileName);
                    filename = filename + extent;
                    update.UpdateImage = "~/Content/img/" + filename;
                    update.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                }
                update.UpdateTime = DateTime.Now;
                update.ProjectID = projectID;

                _db.Update.Add(update);
                _db.SaveChanges();
                return RedirectToAction("ProjectList","Project");
            }
            return RedirectToAction("UpdateCreate","Update",new { projectID = projectID } );
        }
        public PartialViewResult UpdateEdit(int? projectID)
        {
            if (projectID.HasValue)
            {
                if (_db.Update.Any(u => u.ProjectID == projectID))
                {
                    var updates = _db.Update.Where(u => u.ProjectID == projectID).ToList(); 
                    return PartialView(updates);
                }
            }
            return PartialView(null);
        }
        [HttpPost]
        public ActionResult UpdateEdit(int? projectID, Update update)
        {
            if (ModelState.IsValid)
            {
                var old = _db.Update.AsNoTracking().SingleOrDefault(u => u.UpdateId == update.UpdateId);
                update.UpdateTime = old.UpdateTime;
                if (update.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(update.UploadImage.FileName);
                    string extent = Path.GetExtension(update.UploadImage.FileName);
                    filename = filename + extent;
                    update.UpdateImage = "~/Content/img/" + filename;
                    update.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/img/"), filename));
                }
                else
                {
                    update.UpdateImage = old.UpdateImage;
                }
                _db.Entry(update).State = EntityState.Modified;
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.SaveChanges();
            }
            return RedirectToAction("ProjectEdit", "Project", new { id = projectID });
        }

        public ActionResult UpdateDelete(int updateID,int projectID)
        {
            if (Request.UrlReferrer != null)
            {
                TempData["ConfirmationMessage"] = "Bạn Đã Xóa Thành Công Cập Nhật";
            }
            var update = _db.Update.Find(updateID);
            _db.Update.Remove(update);
            _db.SaveChanges();

            return RedirectToAction("ProjectEdit","Project", new{ id = projectID }); 
        }

    }
}