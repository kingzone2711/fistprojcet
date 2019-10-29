using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using CM.Models;
using System.IO;
using Newtonsoft.Json;
using Type = CM.Models.Type;

namespace CM.Controllers
{
    public class ImportEventController : Controller
    {
        private ECMDBEntities db = new ECMDBEntities();

        // GET: ImportEvent
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult UploadData(HttpPostedFileBase upload)
        {
            //if (upload == null)
            //{
            //    if (Request.Files.Count > 0)
            //    {
            //        upload = Request.Files[0];
            //    }
            //}
            System.Diagnostics.Debug.WriteLine("is upload");
            //System.Diagnostics.Debug.WriteLine(ModelState.IsValid.ToString() + upload.ToString());
            List<ImportEvent> listEvent = new List<ImportEvent>();
            List<ImportEvent> listWarning = new List<ImportEvent>();
            List<ImportEvent> listError = new List<ImportEvent>();
            List<ImportEvent> ListAll = new List<ImportEvent>();
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    Stream stream = upload.InputStream;
                    IExcelDataReader reader = null;
                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    System.Diagnostics.Debug.WriteLine("Doc file excel");
                    while (reader.NextResult())
                    {
                        System.Diagnostics.Debug.WriteLine("day la sheet" + reader.Name);
                        if (reader.Name == "Event Code")
                        {
                            System.Diagnostics.Debug.WriteLine("is in Event Code");
                            int cnt = 0;
                            int nline = 0;
                            reader.Read();
                            reader.Read();
                            reader.Read();
                            while (reader.Read())
                            {
                                nline++;
                                System.Diagnostics.Debug.WriteLine("is in Row" + nline.ToString());
                                if (reader.GetString(1) == null) break;
                                string str = reader.GetString(0);
                                ImportEvent curr = new ImportEvent();
                                //Get site of course, only 3 site and not have db for site, 3 stie is HCM,HN,DN
                                if (str == "HCM" || str == "HN" || str == "DN")
                                {
                                    curr.site = str;
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error site");
                                    curr.site = str;
                                    curr.isErrorSite = true;
                                    curr.eventType = Type.isError;
                                }
                                //Course Name
                                str = reader.GetString(2);
							//	var ex = db.CampuslinkPrograms.ToList();
                                if (db.CampuslinkPrograms.Where(o => o.CP_Name == str).Count() != 0)
                                {
                                    curr.courseName = str;
                                }
                                else //false
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error course name");
                                    curr.courseName = str;
                                    curr.isErrorCourseName = true;
                                    curr.eventType = Type.isError;
                                }
                                //Subject Type
                                str = reader.GetString(3);
                                if (true)
                                {
                                    curr.subjectType = str;
                                }
                                else
                                {
                                    //error, cha biet error kieu gi khi ko co db :))
                                }
                                //Sub Subject Type
                                str = reader.GetString(4);
                                if (db.Sub_Subject_Type.Where(o => o.SST_Name == str).Count() != 0)
                                {
                                    curr.subSubjectType = str;
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error subsubject type");
                                    curr.subSubjectType = str;
                                    curr.isErrorSubSubjectType = true;
                                    curr.eventType = Type.isError;
                                }
                                //Format Type
                                str = reader.GetString(5);
                                if (str == "Blended" || str == "Offline")//ko co db Format Type ??????? wtf
                                {
                                    curr.formatType = str;
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error format type");
                                    curr.formatType = str;
                                    curr.isErrorFormatType = true;
                                    curr.eventType = Type.isError;
                                }
                                //Supplier/Partner
                                str = string.Join(" ", reader.GetString(6).Split('(')[0].Split(' '));
                                if (db.Universities.Where(o => o.UniversityName.ToLower() == str.ToLower()).Count() != 0)
                                {
                                    curr.supplierPartner = str;
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error supplier");
                                    curr.supplierPartner = str;
                                    curr.isErrorSupplierPartner = true;
                                    curr.eventType = Type.isError;
                                }
                                //Planned Start
                                if (reader.GetValue(7) != null)
                                {
                                    if (reader.GetValue(7).GetType() == typeof(DateTime))
                                    {
                                        curr.plannedStartDate = Convert.ToDateTime(reader.GetValue(7));
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " error psd");
                                        curr.plannedStartDate = null;
                                        curr.isErrorPlannedStartDate = true;
                                        curr.eventType = Type.isError;
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error psd");
                                    curr.plannedStartDate = null;
                                    curr.isErrorPlannedStartDate = true;
                                    curr.eventType = Type.isError;
                                }
                                //Planned End
                                if (reader.GetValue(8) != null)
                                {
                                    if (reader.GetValue(8).GetType() == typeof(DateTime))
                                    {
                                        curr.plannedEndDate = Convert.ToDateTime(reader.GetValue(8));
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " error ped");
                                        curr.plannedEndDate = null;
                                        curr.isErrorPlannedEndDate = true;
                                        curr.eventType = Type.isError;
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error ped");
                                    curr.plannedEndDate = null;
                                    curr.isErrorPlannedEndDate = true;
                                    curr.eventType = Type.isError;
                                }
                                //Planned Expense
                                if (reader.GetValue(11) != null)
                                {
                                    if(reader.GetValue(11).GetType() == typeof(Double))
                                    {
                                        curr.plannedExpense = Convert.ToDouble(reader.GetValue(11));
                                    }
                                    else
                                    {
                                        curr.plannedExpense = null;
                                    }
                                }
                                else
                                {
                                    curr.plannedExpense = null;
                                }
                                //Actual Start
                                ////////////
                                if (reader.GetValue(13) != null)
                                {
                                    if (reader.GetValue(13).GetType() == typeof(DateTime))
                                    {
                                        curr.actualStartDate = Convert.ToDateTime(reader.GetValue(13));
                                    }
                                    else
                                    {
                                        curr.actualStartDate = null;
                                        curr.isErrorActualStartDate = true;
                                        curr.eventType = Type.isError;
                                    }
                                }
                                else
                                {
                                    curr.actualStartDate = null;
                                }
                                //Actual End
                                //////////
                                if (reader.GetValue(14) != null)
                                {
                                    if (reader.GetValue(14).GetType() == typeof(DateTime))
                                    {
                                        curr.actualEndDate = Convert.ToDateTime(reader.GetValue(14));
                                    }
                                    else
                                    {
                                        curr.actualEndDate = null;
                                        curr.isErrorActualEndDate = true;
                                        curr.eventType = Type.isError;
                                    }
                                }
                                else
                                {
                                    curr.actualEndDate = null;
                                }
                                //Actual Expense
                                if (reader.GetValue(19) != null)
                                {
                                    if (reader.GetValue(19).GetType() == typeof(Double))
                                    {
                                        curr.actualExpense = Convert.ToDouble(reader.GetValue(19));
                                    }
                                    else
                                    {
                                        curr.actualExpense = null;
                                    }
                                }
                                else
                                {
                                    curr.actualExpense = null;
                                }
                                //Faculty
                                str = reader.GetString(40);
                                if (db.Faculties.Where(o => o.FacultyCode == str).Count() != 0)
                                {
                                    curr.faculty = str;
                                }
                                else
                                {
                                    curr.faculty = str;
                                    curr.isErrorFaculty = true;
                                    curr.eventType = Type.isError;
                                }
                                //Status
                                str = reader.GetString(32);
                                if(str == "Done" || str == "In progress" || str == "Planned")
                                {
                                    curr.status = str;
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error status");
                                    curr.status = null;
                                    curr.isErrorStatus = true;
                                    curr.eventType = Type.isError;
                                }
                                //Budget Code
                                curr.budgetCode = reader.GetString(12);
                                //Feedback
                                curr.feedback = reader.GetString(20);
                                //Feedback Content
                                curr.feedback = reader.GetString(21);
                                //Feedback Teacher
                                curr.feedback = reader.GetString(22);
                                //Feedback Organization
                                curr.feedback = reader.GetString(23);
                                //Note
                                curr.feedback = reader.GetString(26);
                                //Event Number
                                
                                //Course Code Gen and compare
                                str = reader.GetString(1);
                                if (curr.eventType == Type.isValid)
                                {
                                    cnt++;
                                    curr.courseCode = str;
                                    //count number event
                                    curr.number = cnt + db.Events.Where(o => o.PlanStartDate.Value.Year == curr.plannedStartDate.Value.Year).Count();
                                    string genCC = GenerateCourseCode(curr);
                                    if (str != genCC)
                                    {
                                        curr.changeCourseCode = genCC;
                                        curr.eventType = Type.isWarning;
                                    }
                                }

                                if (curr.eventType == Type.isValid)
                                {
                                    System.Diagnostics.Debug.WriteLine("add valid");
                                    listEvent.Add(curr);
                                } 
                                else if (curr.eventType == Type.isError)
                                {
                                    System.Diagnostics.Debug.WriteLine("add error");
                                    listError.Add(curr);
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine("add warning");
                                    listWarning.Add(curr);
                                }

                            }
                            //this is for debug
                            System.Diagnostics.Debug.WriteLine("list Valid Event:");
                            foreach (ImportEvent item in listEvent)
                            {
                                AddEventToDB(item);
                                System.Diagnostics.Debug.WriteLine(
                                    item.site + " | " +
                                    item.courseCode + " | " +
                                    item.courseName + " | " +
                                    item.subjectType + " | " +
                                    item.subSubjectType + " | " +
                                    item.formatType + " | " +
                                    item.supplierPartner + " | " +
                                    item.plannedStartDate.ToString() + " | " +
                                    item.plannedEndDate.ToString() + " | " +
                                    item.plannedExpense.ToString() + " | " +
                                    item.actualStartDate.ToString() + " | " +
                                    item.actualEndDate.ToString() + " | " +
                                    item.actualExpense.ToString() + " | " +
                                    item.budgetCode + " | " +
                                    item.feedback + " | " +
                                    item.feedbackContent + " | " +
                                    item.feedbackTeacher+ " | " +
                                    item.feedbackOrganization + " | " +
                                    item.note + " | " +
                                    item.status + " | ");
                            }
                            System.Diagnostics.Debug.WriteLine("list Warning Event:");
                            foreach (ImportEvent item in listWarning)
                            {

                                System.Diagnostics.Debug.WriteLine(
                                    item.site + " | *" +
                                    item.courseCode + "* | " +
                                    item.changeCourseCode + " | " +
                                    item.courseName + " | " +
                                    item.subjectType + " | " +
                                    item.subSubjectType + " | " +
                                    item.formatType + " | " +
                                    item.supplierPartner + " | " +
                                    item.plannedStartDate.ToString() + " | " +
                                    item.plannedEndDate.ToString() + " | " +
                                    item.plannedExpense.ToString() + " | " +
                                    item.actualStartDate.ToString() + " | " +
                                    item.actualEndDate.ToString() + " | " +
                                    item.actualExpense.ToString() + " | " +
                                    item.budgetCode + " | " +
                                    item.feedback + " | " +
                                    item.feedbackContent + " | " +
                                    item.feedbackTeacher + " | " +
                                    item.feedbackOrganization + " | " +
                                    item.note + " | " +
                                    item.status + " | ");
                            }
                            System.Diagnostics.Debug.WriteLine("list Error Event");
                            foreach (ImportEvent item in listError)
                            {
                                System.Diagnostics.Debug.WriteLine(
                                    item.site + " | " +
                                    item.courseCode + " | " +
                                    item.courseName + " | " +
                                    item.subjectType + " | " +
                                    item.subSubjectType + " | " +
                                    item.formatType + " | " +
                                    item.supplierPartner + " | " +
                                    item.plannedStartDate.ToString() + " | " +
                                    item.plannedEndDate.ToString() + " | " +
                                    item.plannedExpense.ToString() + " | " +
                                    item.actualStartDate.ToString() + " | " +
                                    item.actualEndDate.ToString() + " | " +
                                    item.actualExpense.ToString() + " | " +
                                    item.budgetCode + " | " +
                                    item.feedback + " | " +
                                    item.feedbackContent + " | " +
                                    item.feedbackTeacher + " | " +
                                    item.feedbackOrganization + " | " +
                                    item.note + " | " +
                                    item.status + " | ");
                            }
                            ListAll.AddRange(listEvent);
                            ListAll.AddRange(listWarning);
                            ListAll.AddRange(listError);
                            break;
                        }
                    }
                    reader.Close();
                }
            }
            string JsonList = JsonConvert.SerializeObject(ListAll);
            return Json(JsonList, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public string GenerateCourseCode(ImportEvent iEvent)
        {
            string universityCode = db.Universities.Where(o => o.UniversityName.ToLower() == iEvent.supplierPartner.ToLower()).First().UniversityCode;
            string courseNameCode = db.CampuslinkPrograms.Where(o => o.CP_Name == iEvent.courseName).First().Code;
            string subSubjectType = iEvent.subSubjectType;
            string site = iEvent.site;
            string year = iEvent.plannedStartDate.Value.Year.ToString().Substring(2);
            string number = iEvent.number.ToString("000");
            return
                universityCode + "_" +
                courseNameCode + "_" +
                subSubjectType + "_" +
                site + year + "_" +
                number;    
        }
        [NonAction]
        public void AddEventToDB(ImportEvent iEvent)
        {
            Event currE = new Event();

            currE.CourseCode = iEvent.courseCode;
            currE.Site = iEvent.site;
            currE.EventStatus = iEvent.status == "Done" ? 2 : iEvent.status == "In progress" ? 1 : 0;
            int facultyId = db.Faculties.Where(o => o.FacultyCode == iEvent.faculty).First().id;
            int universityId = db.Universities.Where(o => o.UniversityName.ToLower() == iEvent.supplierPartner.ToLower()).First().id;
            currE.Faculty_of_University_id = db.Faculty_of_University.Where(o => o.Faculty_id == facultyId).Where(o => o.University_id == universityId).First().id;
            currE.FormatType = iEvent.formatType;
            currE.ProgramCode_id = db.CampuslinkPrograms.Where(o => o.CP_Name == iEvent.courseName).First().id;
            currE.SubjectType = iEvent.subjectType;
            currE.SST_id = db.Sub_Subject_Type.Where(o => o.SST_Name == iEvent.subSubjectType).First().id;
            currE.PlanStartDate = iEvent.plannedStartDate;
            currE.PlanEndDate = iEvent.plannedEndDate;
            currE.PlanedExpense = iEvent.plannedExpense;
            currE.ActualStartDate = iEvent.actualStartDate;
            currE.ActualEndDate = iEvent.actualEndDate;
            currE.ActualExpense = iEvent.actualExpense;
            currE.BudgetCode = iEvent.budgetCode;
            currE.TrainingFeedBack = iEvent.feedback;
            currE.TrainingFeedBackContent = iEvent.feedbackContent;
            currE.TrainingFeedBackOrganization = iEvent.feedbackOrganization;
            currE.Note = iEvent.note;
            db.Events.Add(currE);
            db.SaveChanges();
        }
    }
}
