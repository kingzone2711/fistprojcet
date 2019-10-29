using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

		public JsonResult GetEvents()
		{
			using (ECMDBEntities db = new ECMDBEntities())
			{
				db.Configuration.LazyLoadingEnabled = false;
				var events = db.Events.ToList();
				return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
			}
			
		}
	}
}