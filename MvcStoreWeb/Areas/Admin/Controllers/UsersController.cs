using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcStoreData;
using MvcStoreWeb.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace MvcStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators, AppAdministrators")]
    public class UsersController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;

        public UsersController(
            AppDbContext context,
            UserManager<User> userManager
            )
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var page = HttpContext.Request.Query["page"].ToString();
            var pageNumber = string.IsNullOrEmpty(page) ? 1 : int.Parse(page);

            ViewBag.Roles = new SelectList(await context.Roles.Where(p=>p.Name != "Administrators").ToListAsync(), "Name", "DisplayName");

            var model = (await context.Users.ToListAsync()).ToPagedList(pageNumber, 10);
            return View(model);
        }

        public async Task<IActionResult> SetRole(RoleListItemViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId.ToString());
            await userManager.RemoveFromRoleAsync(user, model.CurrentRoleName);
            await userManager.AddToRoleAsync(user, model.Role.Name);
            TempData["success"] = "Rol değiştirme işlemi başarıyla tamamlanmıştır";
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> BanUser(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            user.Enabled = false;
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            TempData["success"] = "Üye yasaklama işlemi başarıyla tamamlanmıştır";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UnbanUser(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            user.Enabled = true;
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            TempData["success"] = "Üye yasak kaldırma işlemi başarıyla tamamlanmıştır";
            return RedirectToAction("Index");
        }
    }
}
