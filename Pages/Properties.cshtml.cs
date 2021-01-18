using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuotesExchangeApp.Pages
{
    public class PropertiesModel : PageModel
    {
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
                decimal result = sum.Value;
                Math.Floor(result);
                Message = $"Частота обновления котировок изменена на {result.ToString()} минут.";
            }
        }
    }
}
