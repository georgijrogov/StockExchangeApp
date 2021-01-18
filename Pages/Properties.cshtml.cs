using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuotesExchangeApp.Data;
using QuotesExchangeApp.Models;

namespace QuotesExchangeApp.Pages
{
    public class PropertiesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public PropertiesModel(ApplicationDbContext db)
        {
            _context = db;
        }
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
                PropertiesObject propObj = new PropertiesObject
                {
                    Minutes = sum.Value
                };
                _context.PropertiesObjects.Add(propObj);
                _context.SaveChanges();
                Message = $"������� ���������� ��������� �������� �� {result.ToString()} �����. ��������� ������� � ���� ����� ����������� �������.";
            }
        }
    }
}
