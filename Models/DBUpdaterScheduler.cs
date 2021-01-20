using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models
{
    public class DBUpdaterScheduler
    {

        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();


            IJobDetail job = JobBuilder.Create<DBUpdater>().Build();

            ITrigger trigger = TriggerBuilder.Create()  
                .WithIdentity("trigger1", "group1")     
                .StartNow()                            
                .WithSimpleSchedule(x => x            
                    .WithIntervalInMinutes(4)          
                    .RepeatForever())                   
                .Build();                               

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}
