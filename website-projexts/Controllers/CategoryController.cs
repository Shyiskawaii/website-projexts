
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using website_projexts.Context;

namespace website_projexts.Controllers
{
    public class CategoryController : Controller
    {
        private OurDBContext _db = new OurDBContext();
        // GET: Category
        public PartialViewResult CategoryPartial()
        {
            var cateList = _db.Category.ToList();
            return PartialView(cateList);
        }
        
    }
}