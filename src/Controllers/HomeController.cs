using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using src.Models;
using src.Services;

namespace src.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OFXTransactionService _oFXTransactionService;

        public HomeController(ILogger<HomeController> logger, OFXTransactionService oFXTransactionService)
        {
            _logger = logger;
            _oFXTransactionService = oFXTransactionService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _oFXTransactionService.getTransactions());
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
