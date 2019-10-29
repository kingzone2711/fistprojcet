using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CM.Models;

namespace QLUV.Controllers
{
    public class CampuslinkProgramController : Controller
    {
        private ECMDBEntities db = new ECMDBEntities();

        // GET: CampuslinkProgram
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult _List()
        {
            ViewData["Events"] = db.Events.ToList();
            return PartialView(db.CampuslinkPrograms.ToList());
        }

        // GET: CampuslinkProgram/_CreateOrUpdate
        public ActionResult _CreateOrUpdate(int? id)
        {
            if (id == null)
            {
                //ViewBag.CreateOrUpdate = "Create Campuslink Program";
                //return PartialView();
                return View();
            }
            //ViewBag.CreateOrUpdate = "Update Campuslink Program";
            CampuslinkProgram cp = db.CampuslinkPrograms.Find(id);
            if (cp == null)
            {
                return HttpNotFound();
            }
            //return PartialView(cp);
            return View(cp);
        }

        // POST: CampuslinkProgram/_CreateOrUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateOrUpdate([Bind(Include = "id,Code,CP_Name,LearningTime")] CampuslinkProgram cp)
        {
            if (ModelState.IsValid)
            {
                cp.Code = cp.Code.ToUpper();
                if (cp.id <= 0)
                {
                    bool check = true;
                    foreach (var item in db.CampuslinkPrograms)
                    {
                        if (item.CP_Name.ToLower().Replace(" ", "") == cp.CP_Name.ToLower().Replace(" ", "") || item.Code == cp.Code)
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        db.CampuslinkPrograms.Add(cp);
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Already exist!');</script>");
                    }
                }
                else
                {
                    db.Entry(cp).State = EntityState.Modified;
                }
                db.SaveChanges();
                //return Json(true, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index");
            }
            return View(cp);
            //return PartialView(cp);
        }

        // POST: CampuslinkProgram/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            CampuslinkProgram cp = db.CampuslinkPrograms.Find(id);
            db.CampuslinkPrograms.Remove(cp);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}