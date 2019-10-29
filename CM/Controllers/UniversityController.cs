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
    public class UniversityController : Controller
    {
        private ECMDBEntities db = new ECMDBEntities();

        // GET: University
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult _List()
        {
            ViewData["Faculty_of_University"] = db.Faculty_of_University.ToList();
            return PartialView(db.Universities.ToList());
        }

        // GET: University/_CreateOrUpdate
        public ActionResult _CreateOrUpdate(int? id)
        {
            if (id == null)
            {
                //ViewBag.CreateOrUpdate = "Create University";
                //return PartialView();
                return View();
            }
            //ViewBag.CreateOrUpdate = "Update University";
            University university = db.Universities.Find(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            //return PartialView(university);
            return View(university);
        }

        // POST: University/_CreateOrUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateOrUpdate([Bind(Include = "id,Site,UniversityName,UniversityCode,HotKey")] University university)
        {
            if (ModelState.IsValid)
            {
                university.UniversityCode = university.UniversityCode.ToUpper();
                university.Site = university.UniversityCode.Substring(0, university.UniversityCode.IndexOf('.'));
                if (university.id <= 0)
                {
                    bool check = true;
                    foreach (var item in db.Universities)
                    {
                        if (item.UniversityName.ToLower().Replace(" ", "") == university.UniversityName.ToLower().Replace(" ", "") || item.UniversityCode == university.UniversityCode)
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        db.Universities.Add(university);
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Already exist!');</script>");
                    }
                }
                else
                {                    
                    db.Entry(university).State = EntityState.Modified;
                }
                db.SaveChanges();
                //return Json(true, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index");
            }
            //return PartialView(university);
            return View(university);
        }

        // POST: University/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            University university = db.Universities.Find(id);
            db.Universities.Remove(university);
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