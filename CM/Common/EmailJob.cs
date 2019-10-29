using CM.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using CM.Controllers;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;

namespace CM
{
    public class EmailJob : IJob
    {
        private const string BulkSetPriceFile = "Remind.html";
        static ECMDBEntities db = new ECMDBEntities();
        public void Execute(IJobExecutionContext context)
        {
            try
            {

                string listevent = "";
                string listlink = "";
                var allevent = db.Events.ToList();
                var allemail = db.Candidates.ToList();
                var alllink = db.Candidate_in_Event.ToList();

                string ngayHeThong = DateTime.Now.ToString("dd/MM/yyyy");
                int ngaycuahethong = Convert.ToInt32(ngayHeThong.Substring(0, 2)); /*Ngày của hệ thống*/
                string listEmail = "";

                for (int j = 0; j < allevent.Count(); j++)
                {
                    listevent += allevent[j].PlanStartDate + ",";
                    var converdate = listevent;
                    int ngay = Convert.ToInt32(converdate.Substring(0, 2));
                    int thang = Convert.ToInt32(converdate.Substring(3, 2));

                    int nam = Convert.ToInt32(converdate.Substring(6, 4));
                    DateTime dt = new DateTime(nam, thang, ngay);
                    string datetime = dt.ToString("dd/MM/yyyy");
                    int ketqua = ngay - ngaycuahethong;

                    var datesend = allevent[j].CountDay;
                    int result = datetime.CompareTo(ngayHeThong);/*Nếu bằng nhau thì result = 0; băng 1 thì lớn hơn;-1 thì nhỏ hơn*/
                    if (ketqua <= datesend && ketqua > 0) 
                    {

                        for (int k = 0; k < alllink.Count(); k++)
                        {
                            listlink += alllink[k].Event_id;
                            string idcadidate = alllink[k].Candidate_id.ToString();
                            string idevent = allevent[j].id.ToString();
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


                                        string FilePath = HostingEnvironment.MapPath(@"~/Template/Remind.html");
                                        StreamReader str = new StreamReader(FilePath);
                                        string MailText = str.ReadToEnd();/*Đọc file*/
                                        var nameevent = allevent[j].CourseCode.ToString();
                                        var ten = allemail[i].CandidateName.ToString();
                                        var email = allemail[i].Email.ToString();
                                        var ngaysinh = allemail[i].DOB.ToString();
                                        //Xóa giờ đi còn ngày thôi
                                        int date = Convert.ToInt32(ngaysinh.Substring(0, 2));
                                        int month = Convert.ToInt32(ngaysinh.Substring(3, 2));

                                        int year = Convert.ToInt32(ngaysinh.Substring(6, 4));
                                        DateTime ngaydate = new DateTime(year, month, date);
                                        string truedate = ngaydate.ToString("dd/MM/yyyy");
                                        var datestar = allevent[j].PlanStartDate.ToString();

                                        MailText = MailText.Replace("{{name}}", nameevent.ToString());
                                        MailText = MailText.Replace("{{ten}}", ten.ToString());
                                        MailText = MailText.Replace("{{ngaysinh}}", truedate.ToString());
                                        MailText = MailText.Replace("{{Email}}", email.ToString());
                                        MailText = MailText.Replace("{{date}}", datetime.ToString());
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

                        listevent = "";
                    }
                    else
                    {
                        listevent = "";
                    }
                    listevent = "";
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
        }

    }
}