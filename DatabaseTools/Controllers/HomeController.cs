using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DatabaseTools.Models;
using Domain.Application.IRepositories;
using Domain.Application;
using Domain.Application.Repositories;
using Domain.Application.Entities;
using Domain.Common.Enums;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Domain.Common.Security;

namespace DatabaseTools.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDBContext dbContext;
		private readonly IWebHostEnvironment env;
		public HomeController(ILogger<HomeController> logger, ApplicationDBContext dbContext, IWebHostEnvironment env)
		{
			_logger = logger;
			this.dbContext = dbContext;
			this.env = env;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult InitData()
		{
			return View();
		}

		private string GetFileContents(string filename)
		{
			var path = System.IO.Path.Combine(env.ContentRootPath, "Data", filename);
			return System.IO.File.ReadAllText(path);
		}
		[HttpPost]
		public IActionResult InitData(InitDataViewModel model)
		{
			if (ModelState.IsValid)
			{
				using (var trans = dbContext.Database.BeginTransaction())
				{
					try
					{
						var roles = JsonConvert.DeserializeObject<List<Role>>(GetFileContents("role.json"));
						var menus = JsonConvert.DeserializeObject<List<Menu>>(GetFileContents("menu.json"));
						var menu_role = JsonConvert.DeserializeObject<List<MenuRole>>(GetFileContents("menu-role.json"));
						var User = new User
						{
							Id = "1",
							FullName = "Quản trị hệ thống",
							Email = model.AdminEmail,
							UserName = model.AdminUsername,
							Password = Security.EncryptPassword(model.AdminPassword),
							Gender = (int)Gender.Male,
							UserRole = roles.Select(p=> new UserRole
							{
								Id = Guid.NewGuid().ToString(),
								RoleId = p.Id,
								UserId = "1"
							}).ToList()
						};
						dbContext.Roles.AddRange(roles);
						dbContext.Users.Add(User);
						dbContext.Menus.AddRange(menus);
						dbContext.MenuRoles.AddRange(menu_role);
						dbContext.SiteSettings.Add(new SiteSetting
						{
							Id = Guid.NewGuid().ToString(),
							PageEmail = model.AdminEmail,
							PageTitle = model.PageName
						});
						dbContext.SaveChanges();
						trans.Commit();
					}
					catch (Exception )
					{
						trans.Rollback();
					}
				}
			}
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
