using System.Diagnostics;
using AspNetCore.Factory;
using AspNetCore.GenericServices;
using AspNetCore.Models;
using AspNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITestService _testService;
        private readonly ILoneService _loneService;
        private readonly IRepository<Employee> _repository;

        public HomeController(
            ILogger<HomeController> logger,
            ITestService testService,
            ILoneServiceFactory loneServiceFactory,
            IRepository<Employee> repository)
        {
            _logger = logger;
            _testService = testService;
            _loneService = loneServiceFactory?.GetLoneSerive("1");
            _repository = repository;
        }

        public IActionResult Index()
        {
            string welcomeMessage = _repository.Welcome("Tanvir");
            ////string myName = _testService.GetMyName();
            ////string helloString = _loneService.GetString();
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
