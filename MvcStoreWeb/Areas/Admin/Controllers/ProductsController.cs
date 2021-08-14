using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcStoreData;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace MvcStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators, CatalogAdministrators")]
    public class ProductsController : Controller
    {
        private const string entityName = "Ürün";

        private readonly AppDbContext context;

        public ProductsController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            var model = await context.Products.OrderBy(p => p.Name).ToPagedListAsync(page ?? 1, pageSize ?? 10);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new Product { Enabled = true };
            FillDropdowns();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            model.Price = decimal.Parse(model.PriceText, CultureInfo.CreateSpecificCulture("tr-TR"));

            if (model.SelectedCategories != null)
            {
                model.SelectedCategories.ToList().ForEach(cat =>
                {
                    var category = new Category { Id = cat };
                    context.Categories.Attach(category);
                    model.Categories.Add(category);
                });

            }

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
                            model.Photo = image.ToBase64String(JpegFormat.Instance);
                        });
                    }
                }
                catch (UnknownImageFormatException)
                {
                    ModelState.AddModelError("", "Yüklenen dosya bilinen bir görsel biçiminde değil!");
                    FillDropdowns();
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Lütfen bir logo yükleyiniz!");
                return View(model);
            }

            if (model.PhotoFiles != null)
                foreach (var photoFile in model.PhotoFiles)
                {
                    try
                    {
                        using (var image = Image.Load(photoFile.OpenReadStream()))
                        {
                            image.Mutate(p =>
                            {
                                p.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = new Size(600, 600)
                                });
                                p.BackgroundColor(Color.White);
                                var photo = new ProductImage
                                {
                                    UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                                    Enabled = true,
                                    DateCreated = DateTime.Now,
                                    Photo = image.ToBase64String(JpegFormat.Instance)
                                };
                                context.Entry(photo).State = EntityState.Added;
                                model.ProductImages.Add(photo);
                            });
                        }
                    }
                    catch (UnknownImageFormatException)
                    {

                    }
                }

            model.DateCreated = DateTime.Now;
            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            context.Entry(model).State = EntityState.Added;

            try
            {

                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} ekleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                TempData["error"] = $"{entityName} ekleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                FillDropdowns();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.Products.FindAsync(id);
            model.PriceText = model.Price.ToString("f2", CultureInfo.CreateSpecificCulture("tr-TR"));
            FillDropdowns();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product model)
        {


            var original = await context.Products.FindAsync(model.Id);

            model.Price = decimal.Parse(model.PriceText, CultureInfo.CreateSpecificCulture("tr-TR"));

            var categoryIds = (await context.Products.FindAsync(model.Id)).Categories.Select(p => p.Id).ToList();
            var categories = await context.Categories.ToListAsync();

            if (model.SelectedCategories != null)
            {
                model.SelectedCategories
                    .Except(categoryIds).ToList()
                    .ForEach(p => original.Categories.Add(categories.Single(q => q.Id == p)));

                categoryIds
                    .Except(model.SelectedCategories)
                    .ToList()
                    .ForEach(p => original.Categories.Remove(categories.Single(q => q.Id == p)));
            }

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
                            model.Photo = image.ToBase64String(JpegFormat.Instance);
                        });
                    }
                }
                catch (UnknownImageFormatException)
                {
                    ModelState.AddModelError("", "Yüklenen dosya bilinen bir görsel biçiminde değil!");
                    FillDropdowns();
                    return View(model);
                }
            }

            if (model.PhotoFiles != null)
                foreach (var photoFile in model.PhotoFiles)
                {
                    try
                    {
                        using (var image = Image.Load(photoFile.OpenReadStream()))
                        {
                            image.Mutate(p =>
                            {
                                p.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,
                                    Size = new Size(600, 600)
                                });
                                p.BackgroundColor(Color.White);
                                var photo = new ProductImage
                                {
                                    UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                                    Enabled = true,
                                    DateCreated = DateTime.Now,
                                    Photo = image.ToBase64String(JpegFormat.Instance)
                                };
                                context.Entry(photo).State = EntityState.Added;
                                original.ProductImages.Add(photo);
                            });
                        }
                    }
                    catch (UnknownImageFormatException)
                    {

                    }
                }

            if (model.PicturesToDeleted != null)
            {
                foreach (var photoFileId in model.PicturesToDeleted)
                {
                    var image = original.ProductImages.SingleOrDefault(p => p.Id == photoFileId);
                    if (image != null)
                        context.Entry(image).State = EntityState.Deleted;
                }
            }

            context.Entry(original).CurrentValues.SetValues(model);

            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} güncelleme işlemi başarıyla tamamlanmıştır.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                TempData["error"] = $"{entityName} güncelleme işlemi aynı isimli bir kayıt olduğu için tamamlanamıyor.";
                FillDropdowns();
                return View(model);
            }
        }

        public async Task<IActionResult> Remove(int id)
        {
            var item = await context.Products.FindAsync(id);
            context.Entry(item).State = EntityState.Deleted;
            try
            {
                await context.SaveChangesAsync();
                TempData["success"] = $"{entityName} silme işlemi başarıyla tamamlanmıştır.";
            }
            catch (DbUpdateException)
            {
                TempData["error"] = $"{item.Name} isimli kayıt, bir ya da daha fazla kayıt ile ilişkili olduuğundan silme işlemi yapılamıyor!";
            }
            return RedirectToAction("Index");
        }


        private void FillDropdowns()
        {
            ViewBag.Rayons = context.Rayons.Where(p => p.Categories.Any()).OrderBy(p => p.Name).ToList();
            ViewBag.Brands = new SelectList(context.Brands.OrderBy(p => p.Name).Select(p => new { p.Id, p.Name }).ToList(), "Id", "Name");
        }
    }
}
