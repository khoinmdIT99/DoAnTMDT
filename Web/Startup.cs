using System;
using System.Text.Json.Serialization;
using Domain.Application;
using Infrastructure.Common;
using Infrastructure.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shop.Application;

namespace Web
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
            services.AddControllersWithViews()
                 .AddNewtonsoftJson()
                 .AddJsonOptions(jsonOptions =>
               {
                   jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
                   jsonOptions.JsonSerializerOptions.WriteIndented = true;
                   jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                   jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
               });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddRazorPages();
            services.AddDbContext<ApplicationDBContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("NpgsqlConection"),
                    b => b.MigrationsAssembly("DatabaseTools"));
            }, ServiceLifetime.Transient);
            ApplicationDBContext.ConfigureServices(services);

            services.AddDbContext<ShopDBContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("NpgsqlConection"),
                    b => b.MigrationsAssembly("DatabaseTools"));
            }, ServiceLifetime.Transient);
            ShopDBContext.ConfigureServices(services);
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddMemoryCache();
            services.AddScoped<ICacheBase, CacheBase>();
            services.AddScoped<UserInfoCache>();
            services.AddScoped<ConfigurationCache>();
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddScoped<AppSettings>();
            services.AddScoped<EnCryptography>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;

            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Login/Index/";
                options.LogoutPath = new PathString("/Login/logout");
                options.AccessDeniedPath = "/Login/Index/";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.SlidingExpiration = false;

            });
            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ConfigurationCache configurationCache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting(); 
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Default}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            configurationCache.SetConfiguration();
        }
    }
}
