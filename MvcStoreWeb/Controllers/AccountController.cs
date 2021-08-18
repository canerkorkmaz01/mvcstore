using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcStoreData;
using MvcStoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcStoreWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                await userManager.AddClaimAsync(newUser, new Claim("", newUser.Name));
                await userManager.AddToRoleAsync(newUser, "Members");
                return View("RegisterSuccess");
            }

        }


    }
}
