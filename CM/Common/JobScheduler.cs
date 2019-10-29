using Common.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;

using CM.Models;
using System.Linq;
using System.Web.Helpers;


namespace CM
{
    public class JobScheduler
    {
        static ECMDBEntities db = new ECMDBEntities();

        private static ILog Log = LogManager.GetCurrentClassLogger();

        public static void Start(Event kh, int second, int minute, int hour)
        {
            try
            {

                // construct a scheduler factory// xây dựng một lịch trình mới 
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                // get a scheduler// lấy lịch trình
                IScheduler sched = schedFact.GetScheduler();
                sched.Start();
                // gọi class công việc cần làm theo lịch 
                IJobDetail job = JobBuilder.Create<EmailJob>()
                .Build();

                ITrigger trigger = TriggerBuilder.Create()/*tạo lịch*/

                    .WithDailyTimeIntervalSchedule
                    (s =>
                         s.WithIntervalInHours(24)//bao nhiêu giờ thì lặp lại
                                                  //.WithIntervalInSeconds(10)//bao nhiêu giây thì lặp lại
                                                  //.WithIntervalInMinutes(2)//bao nhiêu phút thì lặp lại
                            .OnEveryDay()//thực hiện mỗi ngày
                                         //.WithRepeatCount(times-1)//gửi mấy lần
                                         //.EndingDailyAt(TimeOfDay.HourMinuteAndSecondOfDay(10, 00, 01))
                            .StartingDailyAt(TimeOfDay.HourMinuteAndSecondOfDay(hour, minute, second)

                            )
                    )
                    .Build();

                sched.ScheduleJob(job, trigger);  //Đưa công việc cần làm va schedule với điều kiện trigger
            }

            catch (ArgumentException e)
            {
                Log.Error(e);
            }
        }
    }
}