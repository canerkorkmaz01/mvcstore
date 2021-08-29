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
    [Authorize(Roles = "Administrators, AppAdministrators, CatalogAdministrators")]
    public class RayonsController : Controller
    {
        private const string entityName = "Reyon";

        private readonly AppDbContext context;

        public RayonsController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var model = await context.Rayons.OrderBy(p => p.SortOrder).ToPagedListAsync(page ?? 1, pageSize ?? 10);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new Rayon { Enabled = true };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rayon model)
        {
            model.DateCreated = DateTime.Now;
            model.SortOrder = ((await context.Rayons.OrderByDescending(p => p.SortOrder).FirstOrDefaultAsync())?.SortOrder ?? 0) + 1;
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
            var model = await context.Rayons.FindAsync(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Rayon model)
        {
            context.Entry(model).State = EntityState.Modified;

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
            var item = await context.Rayons.FindAsync(id);
            context.Entry(item).State = EntityState.Deleted;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} silme işlemi başarıyla tamamlanmıştır.";
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{item.Name} isimli kayıt, bir ya da daha fazla kayıt ile ilişkili olduğundan silme işlemi yapılamıyor!";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MoveUp(int id)
        {
            var subject = await context.Rayons.FindAsync(id);
            var target = await context.Rayons.Where(p => p.SortOrder < subject.SortOrder).OrderByDescending(p => p.SortOrder).FirstOrDefaultAsync();
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
            var subject = await context.Rayons.FindAsync(id);
            var target = await context.Rayons.Where(p => p.SortOrder > subject.SortOrder).OrderBy(p => p.SortOrder).FirstOrDefaultAsync();
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
