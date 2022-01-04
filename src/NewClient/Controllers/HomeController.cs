using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewClient.Models;
using NewClient.Services;

namespace NewClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnApiService _apiService;

        private readonly IWeatherService _weatherService;

        public HomeController(ILogger<HomeController> logger, IAnApiService apiService, IWeatherService weatherService)
        {
            _logger = logger;
            _apiService = apiService;
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _apiService.UserData();
            //var data = await _weatherService.WeatherData();

            var model = new UserModel() { Data = data };

            return View(model);
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
