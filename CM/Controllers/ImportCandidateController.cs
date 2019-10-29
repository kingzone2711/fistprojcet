using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using CM.Models;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CM.Controllers
{
    public class ImportCandidateController : Controller
    {
        // GET: ImportCandidate
        private ECMDBEntities db = new ECMDBEntities();
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public JsonResult UploadData(HttpPostedFileBase upload)
        {
            System.Diagnostics.Debug.WriteLine("is upload");
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
                    System.Diagnostics.Debug.WriteLine("Doc file excel");
                    while (reader.NextResult())
                    {
                        System.Diagnostics.Debug.WriteLine("day la sheet" + reader.Name);
                        if (reader.Name == "Data List")
                        {
                            System.Diagnostics.Debug.WriteLine("Is in Data List");
                            
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

                               // string str;
                                if (reader.GetValue(0) != null)
                                {
                                    str = reader.GetValue(0).ToString();
                                }
                                ImportCandidate curr = new ImportCandidate();
                                //nationalID

                               
                                if (reader.GetValue(2) != null)
                                {
                                    if (reader.GetValue(2).GetType() == typeof(Int32))
                                    {
                                        curr.National_id = Convert.ToInt32(reader.GetValue(2));
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " success national id");
                                    }
                                    else
                                    {

                                        curr.isErrorNational_id = true;
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " error , should have a id number!");

                                    }
                                }
                                else
                                {

                                    curr.isErrorNational_id = true;
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error , should have a id number!");

                                }

                                //account
                                str = reader.GetString(3);
                                if (str != null)
                                {
                                    curr.Account = str;
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " success account");
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error account");
                                    curr.Account = str;
                                    curr.isErrorAccount = true;
                                    curr.candidateType = Models.Type.isError;
                                }


                                //candidateName
                                str = reader.GetString(5);
                                if (str != null)
                                {
                                    curr.CandidateName = str;
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " success candidate name");
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error Candidate name");
                                    curr.CandidateName = str;
                                    curr.isErrorCandidateName = true;
                                    curr.candidateType = Models.Type.isError;
                                }






                                //str = reader.GetString(5);
                                //if (str != null)
                                //{
                                //    curr.CandidateName = str;
                                //    System.Diagnostics.Debug.WriteLine(nline.ToString() + " success candidate name");
                                //}
                                //else
                                //{
                                //    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error Candidate name");
                                //    curr.CandidateName = str;
                                //    curr.isErrorCandidateName = true;
                                //    curr.candidateType = Models.Type.isError;
                                //}
                                //dob
                                if (reader.GetValue(6) != null)
                                {
                                    if (reader.GetValue(6).GetType() == typeof(DateTime))
                                    {
                                        curr.DOB = Convert.ToDateTime(reader.GetValue(6));
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " success dob");
                                    }
                                    else
                                    {
                                        
                                        curr.DOB = null;
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " null dob");

                                    }
                                }
                                else
                                {
                                  
                                    curr.DOB = null;
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " null dob");

                                }
                                //gender
                                str = reader.GetString(7);
                                if (str == "Male" || str == "FeMale")
                                {
                                    curr.Gender = str;
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " success gender");
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error Gender");
                                    curr.Gender = str;
                                    curr.isErrorGender = true;
                                    curr.candidateType = Models.Type.isError;
                                }
                                //email
                                str = reader.GetString(8);
                                if (str != null)
                                {
                                    curr.Email = str;
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + "email");
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error email");
                                    curr.Email = str;
                                    curr.isErrorEmail = true;
                                    curr.candidateType = Models.Type.isError;
                                }
                                //phone
                              

                                str = reader.GetString(9);
                                 if (Regex.Match(str, @"^(\+[0-9])$").Success)
                                    {
                                    curr.Phone = str;
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " success phone");
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error phone");
                                    curr.Phone = str;
                                    curr.isErrorPhone = true;
                                    curr.candidateType = Models.Type.isError;
                                }
                                //facebook
                                str = reader.GetString(10);
                                if (str != null)
                                {
                                    curr.Facebook = str;
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " success facebook");
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error Facebook");
                                    curr.Facebook = str;
                                    curr.isErrorFacebook = true;
                                    curr.candidateType = Models.Type.isError;
                                }

                                //graduationDate
                                if (reader.GetValue(11) != null)
                                {
                                    if (reader.GetValue(11).GetType() == typeof(DateTime))
                                    {
                                        curr.GraduationDate = Convert.ToDateTime(reader.GetValue(11));
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " success graduationdate");
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " error GradutionDate");
                                        curr.GraduationDate = null;
                                        curr.isErrorGraduationDate = true;
                                        curr.candidateType = Models.Type.isError;
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error GradutionDate");
                                    curr.GraduationDate = null;
                                    curr.isErrorGraduationDate = true;
                                    curr.candidateType = Models.Type.isError;
                                }
                                ////workingTimeAvaiable
                                //numm = reader.GetInt32(1);
                                //if (numm != 0)
                                //{
                                //    curr.WorkingTimeAvaiable = numm;
                                //}
                                //else
                                //{
                                //    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error workingtimeAvaiable");
                                //    curr.WorkingTimeAvaiable = numm;
                                //    curr.isErrorWorkingTimeAvaiable = true;
                                //    curr.candidateType = Models.Type.isError;
                                //}



                                //skill
                                if (reader.GetValue(13) != null)
                                    if (str == "Java" || str == ".Net" || str == "Mix")
                                {
                                    curr.Skill = str;
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " success skill");
                                    }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error site");
                                    curr.Skill = str;
                                    curr.isErrorSkill = true;
                                    curr.candidateType = Models.Type.isError;
                                }
                                //gpa
                                if (reader.GetValue(14) != null)
                                {
                                    if (reader.GetValue(14).GetType() == typeof(Double))
                                    {
                                        curr.GPA = Convert.ToDouble(reader.GetValue(14));
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " success gpa");
                                    }
                                    else
                                    {
                                        curr.GPA = null;
                                        System.Diagnostics.Debug.WriteLine(nline.ToString() + " null gpa");
                                    }
                                }
                                else
                                {
                                    curr.GPA = null;
                                    System.Diagnostics.Debug.WriteLine(nline.ToString() + " null gpa");
                                }
                                //faculty_of_university_id
                                //numm = reader.GetInt32(15);
                                //if (numm != 0)
                                //{
                                //    curr.National_id = numm;
                               // System.Diagnostics.Debug.WriteLine(nline.ToString() + " success faculty of university id");
                                //}
                                //else
                                //{
                                //    System.Diagnostics.Debug.WriteLine(nline.ToString() + " error national id");
                                //    curr.National_id = numm;
                                //    curr.isErrorNational_id = true;
                                //    curr.candidateType = Models.Type.isError;
                                //}

                                //faculty_of_university

                                //candidate_in_event
                                //candidateHistories

                            }
                            //this is for debug
                            System.Diagnostics.Debug.WriteLine("list Valid Event:");
                            foreach (ImportCandidate item in listEvent)
                            {
                                AddCandidateToDB(item);
                                System.Diagnostics.Debug.WriteLine(
                                    item.National_id + " | " +
                                    item.Account + " | " +
                                    item.CandidateName + " | " +
                                    item.DOB.ToString() + " | " +
                                    item.Gender + " | " +
                                    item.Email + " | " +
                                    item.Phone + " | " +
                                    item.Facebook + " | " +
                                    item.GraduationDate.ToString() + " | " +
                                    item.WorkingTimeAvaiable + " | " +
                                    item.Skill + " | " +
                                    item.GPA + " | " +
                                    item.Faculty_of_University_id + " | " +
                                    item.Faculty_Of_University + " | " +
                                    item.Candidate_In_Events + " | " +
                                    item.CandidateHistories + " | ");
                            }
                            System.Diagnostics.Debug.WriteLine("list Warning Event:");
                            foreach (ImportCandidate item in listWarning)
                            {

                                System.Diagnostics.Debug.WriteLine(
                                    item.National_id + " | " +
                                    item.Account + " | " +
                                    item.CandidateName + " | " +
                                    item.DOB.ToString() + " | " +
                                    item.Gender + " | " +
                                    item.Email + " | " +
                                    item.Phone + " | " +
                                    item.Facebook + " | " +
                                    item.GraduationDate.ToString() + " | " +
                                    item.WorkingTimeAvaiable + " | " +
                                    item.Skill + " | " +
                                    item.GPA + " | " +
                                    item.Faculty_of_University_id + " | " +
                                    item.Faculty_Of_University + " | " +
                                    item.Candidate_In_Events + " | " +
                                    item.CandidateHistories + " | ");
                            }
                            System.Diagnostics.Debug.WriteLine("list Error Event");
                            foreach (ImportCandidate item in listError)
                            {
                                System.Diagnostics.Debug.WriteLine(
                                    item.National_id + " | " +
                                    item.Account + " | " +
                                    item.CandidateName + " | " +
                                    item.DOB.ToString() + " | " +
                                    item.Gender + " | " +
                                    item.Email + " | " +
                                    item.Phone + " | " +
                                    item.Facebook + " | " +
                                    item.GraduationDate.ToString() + " | " +
                                    item.WorkingTimeAvaiable + " | " +
                                    item.Skill + " | " +
                                    item.GPA + " | " +
                                    item.Faculty_of_University_id + " | " +
                                    item.Faculty_Of_University + " | " +
                                    item.Candidate_In_Events + " | " +
                                    item.CandidateHistories + " | ");
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
        public void AddCandidateToDB(ImportCandidate iCandidate)
        {
            Candidate currE = new Candidate();
            currE.National_id = iCandidate.National_id;
            currE.Account = iCandidate.Account;
            currE.CandidateName = iCandidate.CandidateName;
            currE.DOB = iCandidate.DOB;
            currE.Email = iCandidate.Email;
            currE.Facebook = iCandidate.Facebook;
            currE.Phone = iCandidate.Phone;
            currE.Gender = iCandidate.Gender;         
            currE.GraduationDate = iCandidate.GraduationDate;
            currE.WorkingTimeAvaiable = iCandidate.WorkingTimeAvaiable;
            currE.Skill = iCandidate.Skill;
            currE.GPA = iCandidate.GPA;
            currE.Faculty_of_University_id = iCandidate.Faculty_of_University_id;
            currE.Faculty_of_University = iCandidate.Faculty_Of_University;
            currE.Candidate_in_Event = iCandidate.Candidate_In_Events;
            currE.CandidateHistories = iCandidate.CandidateHistories;

            db.Candidates.Add(currE);
            db.SaveChanges();
        }
    }
}
