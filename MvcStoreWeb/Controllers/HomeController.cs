using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcStoreData;
using MvcStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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

        public async Task<IActionResult> Index()
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

        [Authorize]
        public async Task<IActionResult> CreateComment(Comment model)
        {
            model.Enabled = false;
            model.DateCreated = DateTime.Now;
            model.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            context.Entry(model).State = EntityState.Added;
            await context.SaveChangesAsync();
            TempData["success"] = "Yorumunuz eklenmiştir. İncelemenin ardından yayına alınacaktır.";
            return RedirectToAction("Product", new { id = model.ProductId });
        }

        [HttpGet]
        public async Task<IActionResult> LikeComment(int id, bool like, int userId)
        {
            var comment = await context.Comments.FindAsync(id);

            var result = new LikeResultModel { Dislikes = 0, Likes = 0, Error = false };

            if (!comment.CommentLikes.Any(p => p.UserId == userId))
            {
                var model = new CommentLike
                {
                    DateCreated = DateTime.Now,
                    Enabled = true,
                    UserId = userId,
                    Like = like,
                    CommentId = id
                };
                context.Entry(model).State = EntityState.Added;
                await context.SaveChangesAsync();
                result.Likes = comment.CommentLikes.Where(p => p.Like).Count();
                result.Dislikes = comment.CommentLikes.Where(p => !p.Like).Count();
            }
            else
            {
                result.Error = true;
            }
            return Json(result);

            //return RedirectToAction("Product", new { id = productId });
        }
    }
}
