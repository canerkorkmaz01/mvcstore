using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcStoreData;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators, CatalogAdministrators")]
    public class BrandsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = new Brand { Enabled = true };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(Brand model)
        {
            if (model.PhotoFile != null)
            {
                using (var image = Image.Load(model.PhotoFile.OpenReadStream()))
                {
                    image.Mutate(p =>
                    {
                        p.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(600, 600)
                        });
                        model.Photo = image.ToBase64String(JpegFormat.Instance);
                    });
                }
            }
            else
            {
                ModelState.AddModelError("", "Lütfen bir logo yükleyiniz!");
                return View(model);
            }

            return RedirectToAction("Index");
        }


    }
}
