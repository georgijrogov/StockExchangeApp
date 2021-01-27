using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Quartz;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;

namespace QuotesExchangeApp.Pages
{
    public class PropertiesModel : PageModel
    {
        private readonly IScheduler _scheduler;
        public PropertiesModel(IScheduler sheduler)
        {
            _scheduler = sheduler;
        }
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "Введите частоту обновления котировок(в минутах)";
        }
        public void OnPost(int? sum)
        {
            if (sum == null)
            {
                Message = "Введите число";
            }
            else
            {
                QuartzServicesUtilities.ChangeJobInterval<FinnhubGrabberJob>(_scheduler, TimeSpan.FromMinutes(sum.Value));
                QuartzServicesUtilities.ChangeJobInterval<MoexGrabberJob>(_scheduler, TimeSpan.FromMinutes(sum.Value));
                Message = $"Частота обновления котировок изменена на {sum.Value.ToString()} минут.";
            }
        }
    }
}
