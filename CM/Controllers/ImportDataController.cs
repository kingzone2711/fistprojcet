using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using System.IO;
using CM.Models;
using System.Data;
using System.Data.Entity.Validation;
using Type = CM.Models.Type;
using Newtonsoft.Json;

namespace CM.Controllers
{
    public class ImportDataController : Controller
    {
        private ECMDBEntities db = new ECMDBEntities();
        // GET: ImportData
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
			System.Diagnostics.Debug.WriteLine("is upload");
			//System.Diagnostics.Debug.WriteLine(ModelState.IsValid.ToString() + upload.ToString());
			List<ImportCandidate> listEvent = new List<ImportCandidate>();
		List<ImportCandidate> listWarning = new List<ImportCandidate>();
		List<ImportCandidate> listError = new List<ImportCandidate>();
		List<ImportCandidate> ListAll = new List<ImportCandidate>();
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
                    while (reader.NextResult())
                    {
                        if (reader.Name == "Sheet2")
                        {
                            reader.Read();
							int nline = 0;
							reader.Read();
							reader.Read();
							reader.Read();
							while (reader.Read())
                            {
								nline++;
								System.Diagnostics.Debug.WriteLine("is in Row" + nline.ToString());
								ImportCandidate curr = new ImportCandidate();
								//Candidate curr = new Candidate();
                             //   if (reader.GetString(3) == null) break;

			                   //  string str = string.Join(" ", reader.GetString(3).Split('(')[0].Split(' '));
								//	string	str = reader.GetString(3);
			     //                if (db.Universities.Where(o => o.UniversityName == str).Count() != 0)
			     //                {
								//	curr.Faculty_Of_University = str;
        ////                            University currU = new University();
        ////                            currU.Site = reader.GetString(1);
        ////                            currU.UniversityName = string.Join(" ", reader.GetString(3).Split('(')[0].Split(' '));
        ////                            currU.UniversityCode = reader.GetString(5);
        ////                            currU.HotKey = Convert.ToString(reader.GetValue(6));
        ////                            db.Universities.Add(currU);
        ////                            db.SaveChanges();
							 //    }
								//else
								//{
								//	System.Diagnostics.Debug.WriteLine("faculty is error" + nline.ToString());
								//	curr.Faculty_Of_University =null;
								//	curr.isErrorFaculty_of_University = true;
								//	curr.candidateType = Type.isError;
								//}
								curr.Account = reader.GetString(2);
								curr.National_id = Convert.ToInt32(reader.GetValue(0));
								curr.Gender = reader.GetString(6);
								curr.Email = reader.GetString(7);
								curr.Phone = Convert.ToString(reader.GetValue(8));
								curr.Facebook = reader.GetString(9);
								curr.CandidateName = reader.GetString(14);
								curr.DOB = Convert.ToDateTime(reader.GetValue(5));
								if (reader.GetValue(10).GetType()==typeof(DateTime))
								{
									curr.GraduationDate = Convert.ToDateTime(reader.GetValue(10));
								}
								else
								{
									System.Diagnostics.Debug.WriteLine("GraduationDate  error");
									curr.isErrorGraduationDate = true;
									curr.GraduationDate = null;
								}
								//if (reader.GetValue(11).GetType()==typeof(int))
								//{
									curr.WorkingTimeAvaiable = Convert.ToInt32(reader.GetValue(11));
								//}
								//else{
								//	System.Diagnostics.Debug.WriteLine("work fail");
								//	curr.isErrorWorkingTimeAvaiable = true;
								//	curr.candidateType = Type.isError;

								//}
								curr.Skill = reader.GetString(12);
								//if(reader.GetValue(38).GetType()==typeof(Double))
								//{
									curr.GPA = Convert.ToDouble(reader.GetValue(38));
								//}
								//else
								//{
								//	System.Diagnostics.Debug.WriteLine("gpa not null");
								//	curr.isErrorGPA = true;
								//	curr.GPA = null;

								//}
								//if(reader.GetValue(31).GetType()==typeof(int))
								//{
									curr.Faculty_of_University_id = Convert.ToInt32(reader.GetValue(32));
								//}
								//else
								//{
								//	System.Diagnostics.Debug.WriteLine("faculty of university fail");
								//	curr.isErrorFaculty_of_University_id = true;
									
								//}
								
								if (curr.candidateType == Type.isValid)
								{
									System.Diagnostics.Debug.WriteLine("add valid");
									listEvent.Add(curr);
								}
								else if (curr.candidateType == Type.isError)
								{
									System.Diagnostics.Debug.WriteLine("add error");
									listError.Add(curr);
								}
								else
								{
									System.Diagnostics.Debug.WriteLine("add warning");
									listWarning.Add(curr);
								}


								//                        str = reader.GetString(7).Split('-')[0];
								//                        if (db.Faculties.Where(o => o.FacultyCode == str).Count() == 0)
								//                        {
								//                            Faculty currF = new Faculty();
								//                            //System.Diagnostics.Debug.WriteLine(currF.id.GetType());
								//                            //System.Diagnostics.Debug.WriteLine(currF.FacultyCode.GetType());
								//                            //System.Diagnostics.Debug.WriteLine(currF.FacultyName.GetType());
								//                            currF.FacultyCode = reader.GetString(7).Split('-')[0];
								//                            currF.FacultyName = "khong biet";
								//                            db.Faculties.Add(currF);
								//                            db.SaveChanges();
								//                        }
								//                        Faculty_of_University currFU = new Faculty_of_University();
								//                        str = string.Join(" ", reader.GetString(3).Split('(')[0].Split(' '));
								//                        currFU.University_id = db.Universities.Where(o => o.UniversityName == str).First().id;
								//                        str = reader.GetString(7).Split('-')[0];
								//                        currFU.Faculty_id = db.Faculties.Where(o => o.FacultyCode == str).First().id;
								//                        currFU.Lv = Convert.ToInt32(reader.GetDouble(8));
								//                        if (db.Faculty_of_University.Where(o => o.University_id == currFU.University_id).Where(o => o.Faculty_id == currFU.Faculty_id).Count() == 0)
								//                        {
								//                            db.Faculty_of_University.Add(currFU);
								//                            db.SaveChanges();

								////str = reader.GetString(3);
								//if()

							}
							System.Diagnostics.Debug.WriteLine("list Valid Event:");
							foreach (ImportCandidate item in listEvent)
							{
								AddEventToDB(item);
								System.Diagnostics.Debug.WriteLine(
									
									item.CandidateName + " | " +
									item.Account + " | " +
									item.Email + " | " +
									item.Facebook + " | " +
									item.Phone + " | " +
									item.Gender + " | " +
									item.GraduationDate.ToString() + " | " +
									item.WorkingTimeAvaiable + " | " +
									item.Skill + " | " +
									item.GPA + " | " +
									item.Faculty_of_University_id + " | " +								
									item.National_id + " | " );
									//item.feedback + " | " +
									//item.feedbackContent + " | " +
									//item.feedbackTeacher + " | " +
									//item.feedbackOrganization + " | " +
									//item.note + " | " +
									//item.status + " | ");
							}
							System.Diagnostics.Debug.WriteLine("list Warning Event:");
							foreach (ImportCandidate item in listWarning)
							{

								System.Diagnostics.Debug.WriteLine(
									item.CandidateName + " | " +
									item.Account + " | " +
									item.Email + " | " +
									item.Facebook + " | " +
									item.Phone + " | " +
									item.Gender + " | " +
									item.GraduationDate.ToString() + " | " +
									item.WorkingTimeAvaiable + " | " +
									item.Skill + " | " +
									item.GPA + " | " +
									item.Faculty_of_University_id + " | " +
									item.National_id + " | ");
							}
							System.Diagnostics.Debug.WriteLine("list error Event:");
							foreach (ImportCandidate item in listError)
							{

								System.Diagnostics.Debug.WriteLine(
									item.CandidateName + " | " +
									item.Account + " | " +
									item.Email + " | " +
									item.Facebook + " | " +
									item.Phone + " | " +
									item.Gender + " | " +
									item.GraduationDate.ToString() + " | " +
									item.WorkingTimeAvaiable + " | " +
									item.Skill + " | " +
									item.GPA + " | " +
									item.Faculty_of_University_id + " | " +
									item.National_id + " | ");
							}
							ListAll.AddRange(listEvent);
							ListAll.AddRange(listWarning);
							ListAll.AddRange(listError);
							break;
						}

                        //tim sheet param
                        //if (reader.Name == "Param")
                        //{
                        //    //import campuslink
                        //    reader.Read();
                        //    if (reader.GetString(0) == "Campuslink program")
                        //    {
                        //        while (reader.Read())
                        //        {
                        //            System.Diagnostics.Debug.WriteLine("da vo duoc day roi \"CP\"");
                        //            if (reader.GetString(1) == null) break;
                        //            CampuslinkProgram currCP = new CampuslinkProgram();
                        //            currCP.CP_Name = reader.GetString(1);
                        //            currCP.Code = reader.GetString(2);
                        //            if (reader.GetValue(3).ToString() != "-") currCP.LearningTime = Convert.ToInt32(reader.GetValue(3));
                        //            if (db.CampuslinkPrograms.Where(o => o.Code == currCP.Code).Count() == 0 &&
                        //                db.CampuslinkPrograms.Where(o => o.CP_Name == currCP.CP_Name).Count() == 0)
                        //            {
                        //                db.CampuslinkPrograms.Add(currCP);
                        //                db.SaveChanges();
                        //            }
                        //        }
                        //    }

                        //    //tim va import ssst
                        //    while (reader.GetString(0) != "Sub-Subject Type") reader.Read();

                        //    System.Diagnostics.Debug.WriteLine("da vo duoc day roi \"SST\"");
                        //    while (reader.Read())
                        //    {
                        //        if (reader.GetString(1) == null) break;
                        //        Sub_Subject_Type currSST = new Sub_Subject_Type();
                        //        currSST.SST_Name = reader.GetString(1);
                        //        if (db.Sub_Subject_Type.Where(o => o.SST_Name == currSST.SST_Name).Count() == 0)
                        //        {
                        //            db.Sub_Subject_Type.Add(currSST);
                        //            db.SaveChanges();
                        //        }
                        //    }
                        //}
                    }
                    reader.Close();
                }

            }
			string JsonList = JsonConvert.SerializeObject(ListAll);
			return Json(JsonList, JsonRequestBehavior.AllowGet);
		}
		public void AddEventToDB(ImportCandidate itemc)
		{
			Candidate currc = new Candidate();
			currc.Account = itemc.Account;
			currc.National_id = itemc.National_id;
			currc.CandidateName = itemc.CandidateName;
			currc.DOB = itemc.DOB;
			currc.Gender = itemc.Gender;
			currc.Email = itemc.Email;
			currc.Phone = itemc.Phone;
			currc.Facebook = itemc.Facebook;
			currc.GraduationDate = itemc.GraduationDate;
			currc.WorkingTimeAvaiable = itemc.WorkingTimeAvaiable;
			currc.Skill = itemc.Skill;
			currc.GPA = itemc.GPA;
			currc.Faculty_of_University_id = itemc.Faculty_of_University_id;
			db.Candidates.Add(currc);
			db.SaveChanges();

		}

	}
}