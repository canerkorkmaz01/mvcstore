using Microsoft.AspNetCore.Mvc;
using MvcStoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Components
{
    [ViewComponent(Name = "NivoSlider")]
    public class NivoSliderViewComponent : ViewComponent
    {
        private readonly AppDbContext context;

        public NivoSliderViewComponent(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            
            return View(
                context
                .Banners
                .Where(p=> 
                    p.Enabled && 
                    (p.DateFirst < DateTime.Today || !p.DateFirst.HasValue) && 
                    (p.DateLast > DateTime.Today || !p.DateFirst.HasValue) 
                ).ToList());
        }
    }
}
