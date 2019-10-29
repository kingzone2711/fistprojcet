using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CM.Comon;
using System.Web.Mvc;

namespace CM.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        static Send s = new Send();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Mail()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MAIL(Event kh, string chude)
        {
             //int time = Convert.ToInt32(formCollection["time"].ToString());
            // JobScheduler.Start(kh, second, minute, hour);
              s.sendEmailCreate(chude);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleMail(Event kh, int second, int minute, int hour)
        {
            //int time = Convert.ToInt32(formCollection["time"].ToString());
            JobScheduler.Start(kh, second, minute, hour);
            //s.sendEmailCreate(chude);
            return View();
        }
    }
}