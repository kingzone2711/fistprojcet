using CM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;

namespace CM.Comon
{
    public class Send
    {
        private ECMDBEntities db = new ECMDBEntities();
        public void sendEmailCreate(string kh)
        {
            string CC = "";

            var CCmail = db.Candidates.ToList();
            for (int i = 0; i < CCmail.Count(); i++)
            {

                CC += CCmail[i].Email + ",";
                while (CC.IndexOf(" ") >= 0)    //tim trong chuoi vi tri co 2 khoang trong tro len      
                    CC = CC.Replace(" ", ""); //sau do thay the bang 1 khoang trong   
            }
            var resultCC = CC.Substring(0, CC.Length - 1);
            string listlink = "";
            var allemail = db.Candidates.ToList();
            var alllink = db.Candidate_in_Event.ToList();
            string listEmail = "";
            string courecode = "";
            var listEvent = db.Events.ToList();

            for (int j = 0; j < listEvent.Count(); j++)
            {
                courecode += listEvent[j].CourseCode;
                if (kh == courecode)
                {
                    for (int k = 0; k < alllink.Count(); k++)
                    {
                        listlink += alllink[k].Event_id;
                        string idcadidate = alllink[k].Candidate_id.ToString();
                        string idevent = listEvent[j].id.ToString();
                        if (listlink == idevent)
                        {
                            for (int i = 0; i < allemail.Count(); i++)
                            {

                                string idCD = allemail[i].id.ToString();

                                if (idCD == idcadidate)
                                {

                                    listEmail += allemail[i].Email + ",";
                                    while (listEmail.IndexOf(" ") >= 0)    //tim trong chuoi vi tri co 2 khoang trong tro len      
                                        listEmail = listEmail.Replace(" ", ""); //sau do thay the bang 1 khoang trong   

                                    var result1 = listEmail.Substring(0, listEmail.Length - 1);


                                    var senderEmail = new MailAddress("linhnguyen1998125@gmail.com", "linh admin");/*người gửi*/
                                    var receiverEmail = new MailAddress(result1);/*người nhận*/

                                    string FilePath = HostingEnvironment.MapPath(@"~/Template/Create.html");
                                    StreamReader str = new StreamReader(FilePath);
                                    string MailText = str.ReadToEnd();/*Đọc file*/
                                    var nameevent = listEvent[j].CourseCode.ToString();
                                    var ten = allemail[i].CandidateName.ToString();
                                    var email = allemail[i].Email.ToString();
                                    var ngaysinh = allemail[i].DOB.ToString();
                                    //Xóa giờ đi còn ngày thôi
                                    int date = Convert.ToInt32(ngaysinh.Substring(0, 2));
                                    int month = Convert.ToInt32(ngaysinh.Substring(3, 2));

                                    int year = Convert.ToInt32(ngaysinh.Substring(6, 4));
                                    DateTime ngaydate = new DateTime(year, month, date);
                                    string truedate = ngaydate.ToString("dd/MM/yyyy");
                                    var datestar = listEvent[j].PlanStartDate.ToString();



                                    MailText = MailText.Replace("{{name}}", nameevent.ToString());
                                    MailText = MailText.Replace("{{ten}}", ten.ToString());
                                    MailText = MailText.Replace("{{ngaysinh}}", truedate.ToString());
                                    MailText = MailText.Replace("{{Email}}", email.ToString());

                                    str.Close(); /*đóng*/
                                    var password = "01672325249aA";/*password của người gửi*/
                                    var sub = "Chủ đề " + DateTime.Now;/*chủ để*/
                                    var body = MailText;/*nội dung*/


                                    var smtp = new SmtpClient/* khởi tạo phương thức SMTP*//*nhận hay truyền tải dữ liệu trong email của người dùng*/
                                    {
                                        Host = "smtp.gmail.com",  /*trả về host name của SMTP Server đích cho tên miền đó*/
                                        Port = 587,
                                        EnableSsl = true,
                                        DeliveryMethod = SmtpDeliveryMethod.Network,
                                        UseDefaultCredentials = false,
                                        Credentials = new NetworkCredential(senderEmail.Address, password) /*xác thực thông tin của tài khoản người gửi*/
                                    };
                                    using (var mess = new MailMessage(senderEmail, receiverEmail)  /*chuẩn bị các thành phần gửi mail */
                                    {
                                        Subject = sub,
                                        Body = body
                                    })

                                    {
                                        mess.IsBodyHtml = true;
                                        smtp.Send(mess); /*gửi mail */

                                    }
                                    idCD = "";

                                    listEmail = "";
                                }
                                else
                                {
                                    idCD = "";

                                    listEmail = "";
                                }
                                listEmail = "";
                            }
                            listlink = "";
                            idevent = null;
                            idcadidate = "";

                        }
                        else
                        {
                            idevent = null;
                            listlink = "";
                            idcadidate = "";
                        }
                        listlink = "";
                    }
                }
                else
                {
                    courecode = "";
                }
            }

        }
        public void sendEmailUpdate(string kh)
        {
            string CC = "";

            var CCmail = db.Candidates.ToList();
            for (int i = 0; i < CCmail.Count(); i++)
            {

                CC += CCmail[i].Email + ",";
                while (CC.IndexOf(" ") >= 0)    //tim trong chuoi vi tri co 2 khoang trong tro len      
                    CC = CC.Replace(" ", ""); //sau do thay the bang 1 khoang trong   
            }
            var resultCC = CC.Substring(0, CC.Length - 1);

            string listlink = "";
            var allemail = db.Candidates.ToList();
            var alllink = db.Candidate_in_Event.ToList();
            string listEmail = "";
            string courecode = "";
            var listEvent = db.Events.ToList();

            for (int j = 0; j < listEvent.Count(); j++)
            {
                courecode += listEvent[j].CourseCode;
                if (kh == courecode)
                {
                    for (int k = 0; k < alllink.Count(); k++)
                    {
                        listlink += alllink[k].Event_id;
                        string idcadidate = alllink[k].Candidate_id.ToString();
                        string idevent = listEvent[j].id.ToString();
                        if (listlink == idevent)
                        {
                            for (int i = 0; i < allemail.Count(); i++)
                            {

                                string idCD = allemail[i].id.ToString();

                                if (idCD == idcadidate)
                                {

                                    listEmail += allemail[i].Email + ",";
                                    while (listEmail.IndexOf(" ") >= 0)    //tim trong chuoi vi tri co 2 khoang trong tro len      
                                        listEmail = listEmail.Replace(" ", ""); //sau do thay the bang 1 khoang trong   

                                    var result1 = listEmail.Substring(0, listEmail.Length - 1);


                                    var senderEmail = new MailAddress("linhnguyen1998125@gmail.com", "linh admin");/*người gửi*/
                                    var receiverEmail = new MailAddress(result1);/*người nhận*/

                                    string FilePath = HostingEnvironment.MapPath(@"~/Template/Update.html");
                                    StreamReader str = new StreamReader(FilePath);
                                    string MailText = str.ReadToEnd();/*Đọc file*/
                                    var nameevent = listEvent[j].CourseCode.ToString();
                                    var ten = allemail[i].CandidateName.ToString();
                                    var email = allemail[i].Email.ToString();
                                    var ngaysinh = allemail[i].DOB.ToString();
                                    //Xóa giờ đi còn ngày thôi
                                    int date = Convert.ToInt32(ngaysinh.Substring(0, 2));
                                    int month = Convert.ToInt32(ngaysinh.Substring(3, 2));

                                    int year = Convert.ToInt32(ngaysinh.Substring(6, 4));
                                    DateTime ngaydate = new DateTime(year, month, date);
                                    string truedate = ngaydate.ToString("dd/MM/yyyy");
                                    var datestar = listEvent[j].PlanStartDate.ToString();



                                    MailText = MailText.Replace("{{name}}", nameevent.ToString());
                                    MailText = MailText.Replace("{{ten}}", ten.ToString());
                                    MailText = MailText.Replace("{{ngaysinh}}", truedate.ToString());
                                    MailText = MailText.Replace("{{Email}}", email.ToString());

                                    str.Close(); /*đóng*/
                                    var password = "01672325249aA";/*password của người gửi*/
                                    var sub = "Chủ đề " + DateTime.Now;/*chủ để*/
                                    var body = MailText;/*nội dung*/


                                    var smtp = new SmtpClient/* khởi tạo phương thức SMTP*//*nhận hay truyền tải dữ liệu trong email của người dùng*/
                                    {
                                        Host = "smtp.gmail.com",  /*trả về host name của SMTP Server đích cho tên miền đó*/
                                        Port = 587,
                                        EnableSsl = true,
                                        DeliveryMethod = SmtpDeliveryMethod.Network,
                                        UseDefaultCredentials = false,
                                        Credentials = new NetworkCredential(senderEmail.Address, password) /*xác thực thông tin của tài khoản người gửi*/
                                    };
                                    using (var mess = new MailMessage(senderEmail, receiverEmail)  /*chuẩn bị các thành phần gửi mail */
                                    {
                                        Subject = sub,
                                        Body = body
                                    })

                                    {
                                        mess.IsBodyHtml = true;
                                        smtp.Send(mess); /*gửi mail */

                                    }
                                    idCD = "";

                                    listEmail = "";
                                }
                                else
                                {
                                    idCD = "";

                                    listEmail = "";
                                }
                                listEmail = "";
                            }
                            listlink = "";
                            idevent = null;
                            idcadidate = "";

                        }
                        else
                        {
                            idevent = null;
                            listlink = "";
                            idcadidate = "";
                        }
                        listlink = "";
                    }
                }
                else
                {
                    courecode = "";
                }
            }

        }
    }
}