using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Projekt_StudieTips.Data;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Projekt_StudieTips.Repository;

namespace Projekt_StudieTips
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("server=localhost,1433; User Id = SA; Password=password_123; database =StudieTipsDB; trusted_connection = false;"));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<DegreeRepository>();
            services.AddScoped<CourseRepository>();
            services.AddScoped<TipRepository>();

            services.AddDefaultIdentity<IdentityUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;

                // Password Settings
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 4;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;

                // User settings
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+;:";
                options.User.RequireUniqueEmail = false;

            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "IsAdmin",
                    policyBuilder => policyBuilder
                     .RequireClaim("Admin"));
            });

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/UnauthorizedAccess");

            services.AddControllersWithViews();

            services.AddDbContext<DatabaseContext>(options =>
                    options.UseSqlServer(("server=localhost,1433; User Id = SA; Password=password_123; database =StudieTipsDB; trusted_connection = false;")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<IdentityUser> userManager, ILogger<Startup> log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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

            DbAdmin.SeedUsers(userManager, log);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "courses",
                    pattern: "{controller=Courses}/{action=Index}/{DegreeId?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
