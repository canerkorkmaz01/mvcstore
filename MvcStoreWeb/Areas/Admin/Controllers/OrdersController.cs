using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcStoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrators,AppAdministrators,OrderAdministrators")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext context;

        public OrdersController(
            AppDbContext context
            )
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await context.Orders.Where(p => p.OrderStatus == OrderStatus.New).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var model = await context.Orders.FindAsync(id);
            model.OrderStatus = OrderStatus.Cancelled;
            context.Entry(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CompleteOrder(Order item)
        {
            var model = await context.Orders.FindAsync(item.Id);
            model.CargoCode = item.CargoCode;
            model.OrderStatus = OrderStatus.Shipped;
            context.Entry(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
