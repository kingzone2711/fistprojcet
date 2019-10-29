using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CM.Controllers
{
    public class StatisticsController : Controller
    {
        Models.ECMDBEntities context = new Models.ECMDBEntities();
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Chart()
        {
            return View();
        }
        public ActionResult ReportEvent()
        {
            return View(context.Events.ToList());
        }
        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/ReportEvent.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "ECMDBDataSet";
            reportDataSource.Value = context.Events.ToList();
            localreport.DataSources.Add(reportDataSource);
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;
            if(reportType=="Excel")
            {
                fileNameExtension = "xlsx";
            }
            else if (reportType == "Word")
            {
                fileNameExtension = "docx";
            }
            else if (reportType == "PDF")
            {
                fileNameExtension = "pdf";
            }
            else
            {
                fileNameExtension = "jpg";
            }
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType,"",out mimeType,out encoding,out fileNameExtension, out streams,out warnings);
            Response.AddHeader("content-disposition", "attachment:filename= event_report." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
            
        }
        public ActionResult ReportCandidate()
        {
            return View();
        }
        public ActionResult GetData()
        {
            int INTE = context.Events.Where(x => x.CourseCode.Contains("INTE")).Count();
            int LITE = context.Events.Where(x => x.CourseCode.Contains("LITE")).Count();
            int GST = context.Events.Where(x => x.CourseCode.Contains("GST")).Count();
            int THESIS = context.Events.Where(x => x.CourseCode.Contains("THESIS")).Count();
            int SEMINAR = context.Events.Where(x => x.CourseCode.Contains("SEMI")).Count();
            int JOBFAIR = context.Events.Where(x => x.CourseCode.Contains("JOB")).Count();
            int TOUR = context.Events.Where(x => x.CourseCode.Contains("FTOUR")).Count();
            int Sponsor = context.Events.Where(x => x.CourseCode.Contains("CSR")).Count();
            int REC = context.Events.Where(x => x.CourseCode.Contains("REC")).Count();
            int ONLINE = context.Events.Where(x => x.CourseCode.Contains("ONLINE")).Count();
            int ecancel = context.Events.Where(x => x.EventStatus == 3).Count();
            int edone = context.Events.Where(x => x.EventStatus == 2).Count();
            int eplan = context.Events.Where(x => x.EventStatus == 0).Count();
            int eon = context.Events.Where(x => x.EventStatus ==1).Count();
            Ratio obj = new Ratio();
            obj.INTE = INTE;
            obj.LITE = LITE;
            obj.GST = GST;
            obj.THESIS = THESIS;
            obj.SEMINAR = SEMINAR;
            obj.JOBFAIR = JOBFAIR;
            obj.TOUR = TOUR;
            obj.Sponsor = Sponsor;
            obj.REC = REC;
            obj.ONLINE = ONLINE;
            obj.ecancel = ecancel;
            obj.edone = edone;
            obj.eplan = eplan;
            obj.eon = eon;


            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public class Ratio
        {
            public int INTE { get; set; }
            public int LITE { get; set; }
            public int GST { get; set; }
            public int THESIS { get; set; }
            public int SEMINAR { get; set; }
            public int JOBFAIR { get; set; }
            public int TOUR { get; set; }
            public int Sponsor { get; set; }
            public int REC { get; set; }
            public int ONLINE { get; set; }
            public int ecancel { get; set; }
            public int edone { get; set; }
            public int eplan { get; set; }
            public int eon { get; set; }



        }
        
    }
}