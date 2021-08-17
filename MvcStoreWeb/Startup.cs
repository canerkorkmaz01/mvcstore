using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcStoreData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcStoreWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                var dataBase = Configuration.GetValue<string>("Application:Database");
                switch (dataBase)
                {
                    case "mysql":
                        options.UseMySql(
                            Configuration.GetConnectionString("MySql"),
                            ServerVersion.AutoDetect(Configuration.GetConnectionString("Mysql")),
                            config =>
                            {
                                config.MigrationsAssembly("MigrationMySql");
                            }
                            );
                        break;
                    case "sqlserver":
                    default:
                        options.UseSqlServer(
                            Configuration.GetConnectionString("SqlServer"),
                            config =>
                            {
                                config.MigrationsAssembly("MigrationSqlServer");
                            });
                        break;
                }
                
            });

            services
                .AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireDigit = Configuration.GetValue<bool>("Application:Password:RequireDigit");
                    options.Password.RequiredLength = Configuration.GetValue<int>("Application:Password:RequiredLength");
                    options.Password.RequireLowercase = Configuration.GetValue<bool>("Application:Password:RequireLowercase");
                    options.Password.RequireUppercase = Configuration.GetValue<bool>("Application:Password:RequireUppercase");
                    options.Password.RequireNonAlphanumeric = Configuration.GetValue<bool>("Application:Password:RequireNonAlphanumeric");

                    options.Lockout.MaxFailedAccessAttempts = Configuration.GetValue<int>("Application:Lockout:MaxFailedAccessAttempts");
                    options.Lockout.DefaultLockoutTimeSpan = Configuration.GetValue<TimeSpan>("Application:Lockout:DefaultLockoutTimeSpan");
                })
                .AddEntityFrameworkStores<AppDbContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {


                endpoints.MapControllerRoute(
                    name: "category",
                    pattern: "{name}-c-{id}.html",
                    defaults: new { controller = "Home", action = "Category" });

                endpoints.MapControllerRoute(
                    name: "product",
                    pattern: "{name}-p-{id}.html",
                    defaults: new { controller = "Home", action = "Product" });

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });

            context.Database.Migrate();

            //Admin, CatalogAdmin, OrderAdmin, Members

            new[]
            {
                new Role { Name = "Administrators", DisplayName = "Yöneticiler" },
                new Role { Name = "CatalogAdministrators", DisplayName = "Ürün Yöneticileri" },
                new Role { Name = "OrderAdministrators", DisplayName = "Sipariþ Yöneticileri" },
                new Role { Name = "Members", DisplayName = "Üyeler" }
            }
            .ToList()
            .ForEach(p =>
            {
                if (!roleManager.RoleExistsAsync(p.Name).Result)
                {
                    roleManager.CreateAsync(p).Wait();
                }
            });

            if (userManager.FindByNameAsync(Configuration.GetValue<string>("Application:DefaultAdmin:UserName")).Result == null)
            {
                var newUser = new User
                {
                    UserName = Configuration.GetValue<string>("Application:DefaultAdmin:UserName"),
                    Name = Configuration.GetValue<string>("Application:DefaultAdmin:Name"),
                    Email = Configuration.GetValue<string>("Application:DefaultAdmin:UserName"),
                    EmailConfirmed = true
                };
                userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Name, newUser.Name)).Wait();
                userManager.CreateAsync(newUser, Configuration.GetValue<string>("Application:DefaultAdmin:Password")).Wait();
                userManager.AddToRoleAsync(newUser, "Administrators").Wait();
            }

        }
    }
}
