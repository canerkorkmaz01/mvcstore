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
    public class BannersController : Controller
    {
        private const string entityName = "Tanıtım Görseli";

        private readonly AppDbContext context;

        public BannersController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var model = await context.Banners.OrderBy(p => p.SortOrder).ToPagedListAsync(page ?? 1, pageSize ?? 10);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new Banner { Enabled = true };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner model)
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
                                Size = new Size(1400, 480)
                            });
                            p.BackgroundColor(Color.White);
                            model.Image = image.ToBase64String(JpegFormat.Instance);
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
            model.SortOrder = ((await context.Banners.OrderByDescending(p => p.SortOrder).FirstOrDefaultAsync())?.SortOrder ?? 0) + 1;
            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            context.Entry(model).State = EntityState.Added;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} ekleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityName} ekleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(model);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.Banners.FindAsync(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Banner model)
        {
            var original = await context.Banners.FindAsync(model.Id);

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
                            p.BackgroundColor(Color.White);
                            original.Image = image.ToBase64String(JpegFormat.Instance);
                        });
                    }
                }
                catch (UnknownImageFormatException)
                {
                    ModelState.AddModelError("", "Yüklenen dosya bilinen bir görsel biçiminde değil!");
                    return View(model);
                }
            }

            //context.Entry(model).State = EntityState.Modified;
            context.Entry(original).CurrentValues.SetValues(model);

            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} güncelleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{entityName} güncelleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                return View(model);
            }
        }


        public async Task<IActionResult> Remove(int id)
        {
            var item = await context.Banners.FindAsync(id);
            context.Entry(item).State = EntityState.Deleted;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} silme işlemi başarıyla tamamlanmıştır.";
            }
            catch (DbUpdateException)
            {
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MoveUp(int id)
        {
            var subject = await context.Banners.FindAsync(id);
            var target = await context.Banners.Where(p => p.SortOrder < subject.SortOrder).OrderByDescending(p => p.SortOrder).FirstOrDefaultAsync();
            if (target != null)
            {
                var m = target.SortOrder;
                target.SortOrder = subject.SortOrder;
                subject.SortOrder = m;

                context.Entry(subject).State = EntityState.Modified;
                context.Entry(target).State = EntityState.Modified;

                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} sıralama işlemi başarıyla tamamlanmıştır.";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MoveDn(int id)
        {
            var subject = await context.Banners.FindAsync(id);
            var target = await context.Banners.Where(p => p.SortOrder > subject.SortOrder).OrderBy(p => p.SortOrder).FirstOrDefaultAsync();
            if (target != null)
            {
                var m = target.SortOrder;
                target.SortOrder = subject.SortOrder;
                subject.SortOrder = m;

                context.Entry(subject).State = EntityState.Modified;
                context.Entry(target).State = EntityState.Modified;

                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} sıralama işlemi başarıyla tamamlanmıştır.";
            }

            return RedirectToAction("Index");
        }

    }
}
