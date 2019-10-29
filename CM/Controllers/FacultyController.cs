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
    public class FacultyController : Controller
    {
        private ECMDBEntities db = new ECMDBEntities();

        // GET: Faculty
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult _List()
        {
            ViewData["Faculty_of_University"] = db.Faculty_of_University.ToList();
            return PartialView(db.Faculties.ToList());
        }

        // GET: Faculty/_CreateOrUpdate
        public ActionResult _CreateOrUpdate(int? id)
        {
            if (id == null)
            {
                //ViewBag.CreateOrUpdate = "Create Faculty";
                //return PartialView();
                return View();
            }
            //ViewBag.CreateOrUpdate = "Update Faculty";
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            //return PartialView(faculty);
            return View(faculty);
        }

        // POST: Faculty/_CreateOrUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateOrUpdate([Bind(Include = "id,FacultyCode,FacultyName")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                if (faculty.FacultyCode == null)
                {
                    faculty.FacultyCode = "ICT";
                }
                else
                {
                    faculty.FacultyCode = faculty.FacultyCode.ToUpper();
                }
                if (faculty.id <= 0)
                {
                    bool check = true;
                    foreach (var item in db.Faculties)
                    {
                        if (item.FacultyName.ToLower().Replace(" ", "") == faculty.FacultyName.ToLower().Replace(" ", ""))
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        db.Faculties.Add(faculty);
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Already exist!');</script>");
                    }
                }
                else
                {
                    db.Entry(faculty).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
                //return Json(true, JsonRequestBehavior.AllowGet);
            }
            return View(faculty);
            //return PartialView(faculty);
        }

        // POST: Faculty/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Faculty faculty = db.Faculties.Find(id);
            db.Faculties.Remove(faculty);
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