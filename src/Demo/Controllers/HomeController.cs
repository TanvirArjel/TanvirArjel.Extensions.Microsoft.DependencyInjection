using System.Diagnostics;
using Demo.Factory;
using Demo.Models;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITestService _testService;
        private readonly ILoneService _loneService;

        public HomeController(
            ILogger<HomeController> logger,
            ITestService testService,
            ILoneServiceFactory loneServiceFactory)
        {
            _logger = logger;
            _testService = testService;
            _loneService = loneServiceFactory.GetLoneSerive("1");
        }

        public IActionResult Index()
        {
            string myName = _testService.GetMyName();
            string helloString = _loneService.GetString();
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
