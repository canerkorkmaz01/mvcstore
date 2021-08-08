using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcStoreData;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace MvcStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators, CatalogAdministrators")]
    public class BrandsController : Controller
    {
        private const string entityName = "Marka";

        private readonly AppDbContext context;

        public BrandsController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var model = await context.Brands.ToPagedListAsync(page ?? 1, 10);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new Brand { Enabled = true };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand model)
        {
            if (model.PhotoFile != null)
            {
                try
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
                catch (UnknownImageFormatException)
                {
                    ModelState.AddModelError("", "Yüklenen dosya bilinen bir görsel biçiminde değil!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Lütfen bir logo yükleyiniz!");
                return View(model);
            }
            model.DateCreated = DateTime.Now;
            model.SortOrder = 1;
            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            context.Entry(model).State = EntityState.Added;

            await context.SaveChangesAsync();

            TempData["success"] = $"{entityName} ekleme işlemi başarıyla tamamlanmıştır.";

            return RedirectToAction("Index");
        }


    }
}
