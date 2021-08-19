using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcStoreData;
using MvcStoreWeb.Models;
using MvcStoreWeb.Sys;
using NETCore.MailKit.Core;
using Newtonsoft.Json;
using System;
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
    }
}
