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
            Message = "������� ������� ���������� ���������(� �������)";
        }

        public void OnPost(int? sum)
        {
            if (sum == null)
            {
                Message = "������� �����";
            }
            else
            {
                QuartzServicesUtilities.ChangeJobInterval<FinnhubGrabberJob>(_scheduler, TimeSpan.FromMinutes(sum.Value));
                QuartzServicesUtilities.ChangeJobInterval<MoexGrabberJob>(_scheduler, TimeSpan.FromMinutes(sum.Value));
                Message = $"������� ���������� ��������� �������� �� {sum.Value.ToString()} �����.";
            }
        }
    }
}
