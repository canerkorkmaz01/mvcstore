using Microsoft.AspNetCore.Mvc;
using MvcStoreWeb.Sys;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStoreWeb.Components
{

    public class ShoppingCartButtonViewComponent : ViewComponent
    {
        public ShoppingCartButtonViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            var value = Request.Cookies["shoppingCart"];
            var total = 0;
            if (value != null)
            {
                var shoppingChart = JsonConvert.DeserializeObject<ShoppingCart>(value);
                total = shoppingChart.Items.Sum(p => p.Quantity);
            }
            return View(total);
        }
    }
}
