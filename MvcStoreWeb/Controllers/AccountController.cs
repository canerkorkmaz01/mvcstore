using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MvcStoreData;
using MvcStoreWeb.Models;
using MvcStoreWeb.Sys;
using NETCore.MailKit.Core;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcStoreWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailService mailService;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly AppDbContext context;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService mailService,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment,
            AppDbContext context
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mailService = mailService;
            this.configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
            this.context = context;
        }

        public IActionResult Login()
        {
            var model = new LoginViewModel { RememberMe = true };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (!user.Enabled)
                {
                    ModelState.AddModelError("", "Yasaklı kullanıcı girişi");
                    return View(model);
                }
                return Redirect(model.ReturnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı girişi");
                return View(model);
            }

        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            var newUser = new User
            {
                UserName = model.Email,
                Name = model.Name,
                Email = model.Email,
                Gender = model.Gender,
                EmailConfirmed = false
            };

            var result = await userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            else
            {
                await userManager.AddClaimAsync(newUser, new Claim("FullName", newUser.Name));
                await userManager.AddToRoleAsync(newUser, "Members");

                var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var url = Url.Action("Confirmation", "Account", new { id = newUser.Id, token = token }, Request.Scheme);

                var body = string.Format(
                    System.IO.File.ReadAllText(System.IO.Path.Combine(hostingEnvironment.WebRootPath, "content", "templates", "confirmation.html")),
                    model.Name,
                    url
                    );

                await mailService.SendAsync(
                    mailTo: model.Email,
                    subject: $"{configuration.GetValue<string>("Application:Name")} Üyelik E-Posta Doğrulama Mesajı",
                    message: body,
                    isHtml: true
                    );

                return View("RegisterSuccess");
            }

        }

        public async Task<IActionResult> Confirmation(int id, string token)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var shoppingChart = new ShoppingCart();

            var value = Request.Cookies["shoppingCart"];

            if (value != null)
                shoppingChart = JsonConvert.DeserializeObject<ShoppingCart>(value);

            var product = await context.Products.FindAsync(id);
            shoppingChart.Add(product, quantity);
            value = JsonConvert.SerializeObject(shoppingChart);
            Response.Cookies.Append("shoppingCart", value, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            TempData["success"] = $"{product.Name} isimli ürün sepetinize eklenmiştir!";
            return RedirectToRoute("product", new { name = product.Name.ToSafeUrlString(), id = product.Id });
        }

        public async Task<IActionResult> Checkout()
        {
            var shoppingChart = new ShoppingCart();

            var value = Request.Cookies["shoppingCart"];
            if (value != null)
            {
                shoppingChart = JsonConvert.DeserializeObject<ShoppingCart>(value);

                var products = await context.Products.Where(p => shoppingChart.Items.Select(q => q.Id).Contains(p.Id)).ToListAsync();
                shoppingChart.Items.ToList().ForEach(p => p.Product = products.Single(q => q.Id == p.Id));
            }
            return View(shoppingChart);
        }

        public IActionResult AccessDenied() => View();

        public IActionResult ClearCart()
        {
            Response.Cookies.Delete("shoppingCart");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveFromCart(int id)
        {


            var value = Request.Cookies["shoppingCart"];
            if (value != null)
            {
                var shoppingChart = JsonConvert.DeserializeObject<ShoppingCart>(value);
                var item = shoppingChart.Items.Single(p => p.Id == id);
                shoppingChart.Items.Remove(item);
                value = JsonConvert.SerializeObject(shoppingChart);
                Response.Cookies.Append("shoppingCart", value, new CookieOptions { Expires = DateTime.Now.AddDays(7) });
                TempData["success"] = $"{item.Name} isimli ürün sepetinizden çıkartılmıştır!";
            }
            return RedirectToAction("Checkout");
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
            if (result.Succeeded)
            {
                TempData["success"] = "Parolanız başarıyla değiştirilmiştir. Bir sonraki girişinizde kullanabilirsiniz.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }


        [Authorize]
        public async Task<IActionResult> Payment()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> UserAddresses()
        {
            var user = await userManager.GetUserAsync(User);
            var addresses = new
            {
                deliveryAddresses = user.Addresses.OfType<DeliveryAddress>().ToList().Select(p => new { p.Id, p.Name, p.Address, p.PhoneNumber, p.ZipCode, province = p.City.Province.Name, city = p.City.Name }),
                invoiceAddresses = user.Addresses.OfType<InvoiceAddress>().ToList().Select(p => new { p.Id, p.Name, p.Address, p.TaxOffice, p.TaxNumber, province = p.City.Province.Name, city = p.City.Name })
            };
#if DEBUG
            System.Threading.Thread.Sleep(5000);
#endif
            return Json(addresses);
        }

        [Authorize]
        public async Task<IActionResult> Provinces()
        {
            return Json(await context.Provinces.OrderBy(p => p.Name).Select(p => new { p.Id, p.Name, Cities = p.Cities.Select(q => new { q.Id, q.Name }) }).ToListAsync());
        }

        [Authorize, HttpPost]
        public async Task<IActionResult> AddDeliveryAddress([FromBody] DeliveryAddress model)
        {
            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            context.Entry(model).State = EntityState.Added;
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
