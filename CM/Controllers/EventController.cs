using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using CM.Models;
using PagedList;


namespace CM.Controllers
{
    public class EventController : Controller
    {
        private ECMDBEntities db = new ECMDBEntities();
        // GET: Event
        public ActionResult Index(string courseCode, string site, DateTime? planStartDate, DateTime? planEndDate, int? page, string subjectType, int? eventStatus)
        {
            var events = db.Events.Include(a => a.CampuslinkProgram).Include(a => a.Faculty_of_University).Include(a => a.Sub_Subject_Type);

            //////// search using multiple field////////////
            events = from s in db.Events
                     select s;
            ViewBag.CourseCode = (from r in db.Events
                                  select r.CourseCode).Distinct();
            ViewBag.Site = (from r in db.Events
                            select r.Site).Distinct();
            ViewBag.PlanStartDate = (from r in db.Events
                                     select r.PlanStartDate).Distinct();
            ViewBag.PlanEndDate = (from r in db.Events
                                   select r.PlanEndDate).Distinct();
            ViewBag.SubjectType = (from r in db.Events
                                   select r.SubjectType).Distinct();
            ViewBag.EventStatus = (from r in db.Events
                                   select r.EventStatus).Distinct();
            var model = from r in db.Events
                        orderby r.CourseCode
                        where r.CourseCode == courseCode || courseCode == null || courseCode == ""
                        where r.SubjectType == subjectType || subjectType == null || subjectType == ""
                        where r.EventStatus == eventStatus || eventStatus == null
                        where r.Site == site || site == null || site == ""
                        where r.PlanStartDate == planStartDate || planStartDate == null
                        where r.PlanEndDate == planEndDate || planEndDate == null
                        select r;
            /////////////////////////////Paging////////////////////////////////
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }
        /// ////////checkbox delete///////////////////////
		/// 
		
        public ActionResult DeleteSelected(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                //throw error
                ModelState.AddModelError("", "No item selected to delete");
                return View();
            }

            //bind the task collection into list
            List<int> TaskIds = ids.Select(x => Int32.Parse(x)).ToList();

            for (var i = 0; i < TaskIds.Count(); i++)
            {
                var todo = db.Events.Find(TaskIds[i]);
                //remove the record from the database
                db.Events.Remove(todo);
                //call save changes action otherwise the table will not be update
                db.SaveChanges();
            }
            //redirect to index view once record is delete
            return RedirectToAction("Index");
        }

        public PartialViewResult _List()
        {

            return PartialView(db.Events.ToList());
        }
        // GET: University/CreateOrUpdate
        public ActionResult _CreateOrUpdate(int? id)
        {

            ViewBag.ProgramCode_id = new SelectList(db.CampuslinkPrograms, "id", "Code");
            ViewBag.Faculty_of_University_id = new SelectList(db.Faculty_of_University, "id", "id");
            ViewBag.SST_id = new SelectList(db.Sub_Subject_Type, "id", "SST_Name");
            if (id == null)
            {

                return View();
            }
            University university = db.Universities.Find(id);
            if (university == null)
            {
                return HttpNotFound();
            }
            return View(university);
        }
        // POST: University/CreateOrUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateOrUpdate([Bind(Include = "id,CourseCode,ProgramCode_id,SubjectType,SST_id,FormatType,PlanStartDate,PlanEndDate,PlanedExpense,BudgetCode,ActualStartDate,ActualEndDate,ActualExpense,TrainingFeedBack,TrainingFeedBackContent,TrainingFeedBackOrganization,EventStatus,Faculty_of_University_id,Note,Site,CountDay")] Event EV)
        {
            if (!ModelState.IsValid)
            {
                if (EV.id <= 0)
                {
                    db.Events.Add(EV);
                }
                else
                {
                    db.Entry(EV).State = EntityState.Modified;
                }
                ViewBag.ProgramCode_id = new SelectList(db.CampuslinkPrograms, "id", "Code", EV.ProgramCode_id);
                ViewBag.Faculty_of_University_id = new SelectList(db.Faculty_of_University, "id", "id", EV.Faculty_of_University_id);
                ViewBag.SST_id = new SelectList(db.Sub_Subject_Type, "id", "SST_Name", EV.SST_id);

                db.SaveChanges();

                //return Json(true, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index");
            }

            //return PartialView(university);
            return View(EV);
        }

        ///Details //////////////////
        ///// GET: Event/_Details/5
        public ActionResult _Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
    }

}
