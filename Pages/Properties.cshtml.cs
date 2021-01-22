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
        private readonly ApplicationDbContext _context;
        public PropertiesModel(ApplicationDbContext db, IScheduler sheduler)
        {
            _context = db;
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
                double result = sum.Value;
                result = Math.Floor(result);
                PropertiesObject propObj = new PropertiesObject
                {
                    Minutes = sum.Value
                };
                //_context.PropertiesObjects.Add(propObj);
                _context.SaveChanges();
                QuartzServicesUtilities.ChangeJobInterval<DBUpdater>(_scheduler, TimeSpan.FromMinutes(result));
                Message = $"Частота обновления котировок изменена на {result.ToString()} минут.";
            }
        }
    }
}
