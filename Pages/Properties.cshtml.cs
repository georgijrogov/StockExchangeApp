using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quartz;
using QuotesExchangeApp.Jobs;
using QuotesExchangeApp.Quartz;

namespace QuotesExchangeApp.Pages
{
    public class PropertiesModel : PageModel
    {
        public string Message { get; set; }
        private readonly IScheduler _scheduler;
        public PropertiesModel(IScheduler sheduler)
        {
            _scheduler = sheduler;
        }

        public void OnGet()
        {
            Message = "Введите частоту обновления котировок(в минутах)";
        }

        public void OnPost(int min)
        {
            if (min <= 0)
            {
                Message = "Введите число больше нуля";
            }
            else
            {
                QuartzServicesUtilities.ChangeJobInterval<FinnhubGrabberJob>(_scheduler, TimeSpan.FromMinutes(min));
                QuartzServicesUtilities.ChangeJobInterval<MoexGrabberJob>(_scheduler, TimeSpan.FromMinutes(min));
                Message = $"Частота обновления котировок изменена на {min} минут.";
            }
        }
    }
}