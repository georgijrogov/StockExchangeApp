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
                decimal result = sum.Value;
                Math.Floor(result);
                Message = $"������� ���������� ��������� �������� �� {result.ToString()} �����.";
            }
        }
    }
}
