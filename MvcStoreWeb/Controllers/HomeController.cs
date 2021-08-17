using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcStoreData;
using MvcStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;

        public HomeController(
            ILogger<HomeController> logger,
            AppDbContext context
            )
        {
            _logger = logger;
            this.context = context;
        }

        public async Task< IActionResult> Index()
        {
            ViewBag.Products = await context.Products.OrderBy(p => Guid.NewGuid()).Take(12).ToListAsync();
            return View();
        }

        public async Task<IActionResult> Category(int id)
        {
            var model = await context.Categories.FindAsync(id);
            return View(model);
        }
        public async Task<IActionResult> Product(int id)
        {
            var model = await context.Products.FindAsync(id);
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
