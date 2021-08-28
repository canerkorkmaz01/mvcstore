using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcStoreData;
using MvcStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Components
{
    public class SearchBarViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public SearchBarViewComponent(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.Categories = new SelectList(context.Categories.Where(p => p.Enabled).OrderBy(p => p.Name), "Id", "Name");
            var model = new SearchViewModel { Keywords = HttpContext.Request.Query["keywords"].ToString() ?? "" };
            return View(model);
        }
    }
}
