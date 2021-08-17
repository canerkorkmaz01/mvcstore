using Microsoft.AspNetCore.Mvc;
using MvcStoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Components
{
    public class RayonBarViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public RayonBarViewComponent(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(context.Rayons.Where(p=>p.Enabled).OrderBy(p=>p.SortOrder).ToList());
        }
    }
}
