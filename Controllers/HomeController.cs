using System;
using System.Diagnostics;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Settlements.Models;

namespace Settlements.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        List<SettlementModel> settlements = new List<SettlementModel>
        {
            //new SettlementModel(1, "אשקלון"),
            //new SettlementModel(2, "אשדוד"),
            //new SettlementModel(3, "בייר שבה"),
            //new SettlementModel(4, "תל-אביב"),
            //new SettlementModel(5, "חדרה"),
            //new SettlementModel(6, "נהריה"),
            //new SettlementModel(7, "נתניה"),
            //new SettlementModel(8, "רחבות"),
            //new SettlementModel(9, "פרדס חנה"),
            //new SettlementModel(10, "כפר סבה"),
            //new SettlementModel(11, "חולון"),
            //new SettlementModel(12, "נוף הגליל"),
            //new SettlementModel(13, "בת ים"),
        };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(settlements);
        }

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["hello"] = HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
