using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Domain.Application.Repositories;
using Domain.Application.Services;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public ApplicationDBContext() { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuRole> MenuRoles { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<SSOSetting> SSOSettings { get; set; }
        public DbSet<MailSetting> MailSettings { get; set; }
      

        public static void ConfigureServices(IServiceCollection services)
		{
            services.AddScoped<IUnitOfWork<ApplicationDBContext>, ApplicationUnitOfWork>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuRoleRepository, MenuRoleRepository>();
            services.AddScoped<ISiteSettingRepository, SiteSettingRepository>();
            services.AddScoped<IMailSettingRepository, MailSettingRepository>();
            services.AddScoped<ISSOSettingRepository, SSOSettingRepository>();
            services.AddScoped<AuthService>();
        }
    }

}
