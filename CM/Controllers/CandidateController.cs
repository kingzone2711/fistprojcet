using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CM.Models;
using PagedList;

namespace CM.Controllers
{
    public class CandidateController : Controller
    {
        ECMDBEntities db = new ECMDBEntities();
        public ActionResult Index(int? page)
        {
            //int pageSize = 10;
            //int pageNumber = (page ?? 1);
            return View(/*.ToPagedList(pageNumber, pageSize)*/db.Candidates.ToList());
        }

        public JsonResult _List()
        {
            List<Candidate> cand = new List<Candidate>();
            cand = db.Candidates.ToList();            
            return Json(cand, JsonRequestBehavior.AllowGet);           
        }
        //public PartialViewResult _CreateOrUpdate()
        //{
        //    return PartialView("_CreateOrUpdate");
        //}
        [HttpPost]
        public JsonResult Add(Candidate cand)
        {
            db.Candidates.Add(cand);
            db.SaveChanges();
            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }
        public JsonResult GetbyID(int ID)
        {
            //var Candi = db.Candidates.Select(x => new
            //{
            //    id = x.id,
            //    Account = x.Account,
            //    CandidateName = x.CandidateName,
            //    DOB = x.DOB,
            //    Email = x.Email,
            //    Gender = x.Gender,
            //    GPA = x.GPA,
            //    Phone = x.Phone,
            //    Skill = x.Skill
            //}).ToList().Find(x => x.id.Equals(ID));
            var Candi = db.Candidates.ToList().Find(x => x.id.Equals(ID));
            return Json(Candi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(Candidate cand)
        {
            //using (db)
            //{
            //    Candidate updatedcand = (from c in db.Candidates
            //                                where c.id == cand.id
            //                                select c).FirstOrDefault();
            //    updatedcand.Account = cand.Account;
            //    updatedcand.National_id = cand.id;
            //    db.SaveChanges();
            //    return Json(cand, JsonRequestBehavior.AllowGet);
            //}
            db.Entry(cand).State = EntityState.Modified;
            db.SaveChanges();
            return Json(cand, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Delete(int ID)
        {
            Candidate cand = db.Candidates.Find(ID);
            db.Candidates.Remove(cand);
            db.SaveChanges();
            return Json(cand, JsonRequestBehavior.AllowGet);
        }
    }
}

