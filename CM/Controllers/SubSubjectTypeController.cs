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
    public class SubSubjectTypeController : Controller
    {
        private ECMDBEntities db = new ECMDBEntities();

        // GET: SubSubjectType
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult _List()
        {
            ViewData["Events"] = db.Events.ToList();
            return PartialView(db.Sub_Subject_Type.ToList());
        }

        // GET: SubSubjectType/_CreateOrUpdate
        public ActionResult _CreateOrUpdate(int? id)
        {
            if (id == null)
            {
                //ViewBag.CreateOrUpdate = "Create Sub-Subject Type";
                //return PartialView();
                return View();
            }
            //ViewBag.CreateOrUpdate = "Update Sub-Subject Type";
            Sub_Subject_Type SubSubjectType = db.Sub_Subject_Type.Find(id);
            if (SubSubjectType == null)
            {
                return HttpNotFound();
            }
            //return PartialView(SubSubjectType);
            return View(SubSubjectType);
        }

        // POST: SubSubjectType/_CreateOrUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateOrUpdate([Bind(Include = "id,SST_Name")] Sub_Subject_Type SubSubjectType)
        {
            if (ModelState.IsValid)
            {
                SubSubjectType.SST_Name = SubSubjectType.SST_Name.ToUpper().Replace(" ", "");
                if (SubSubjectType.id <= 0)
                {
                    bool check = true;
                    foreach (var item in db.Sub_Subject_Type)
                    {
                        if (item.SST_Name == SubSubjectType.SST_Name)
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        db.Sub_Subject_Type.Add(SubSubjectType);
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Already exist!');</script>");
                    }
                }
                else
                {
                    db.Entry(SubSubjectType).State = EntityState.Modified;
                }
                db.SaveChanges();
                //return Json(true, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index");
            }
            return View(SubSubjectType);
            //return PartialView(SubSubjectType);
        }

        // POST: SubSubjectType/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Sub_Subject_Type SubSubjectType = db.Sub_Subject_Type.Find(id);
            db.Sub_Subject_Type.Remove(SubSubjectType);
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